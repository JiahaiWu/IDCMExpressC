using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Data.Common;
using System.Threading;
using IDCM.Data.Base;
using System.Collections.Concurrent;

namespace IDCM.Data.DHCP
{
    
    /// <summary>
    /// 多源数据库连接选择器。
    /// 说明：
    /// 1.单点数据库连接访问的串行/并行保护类,封装SQLiteConnection用于多线程串行/并行共享。
    /// 2.内置全局多点数据库连接的连接池,对多数据库实例提供支持。
    /// 3.本类允许多实例化策略，但外部获取SQLiteConnection及SQLiteConnPicker句柄后不应二次缓存使用或长时复用。
    /// 4.连接异常信息需外部捕获，异常类型主要包括SQLiteException、IDCMException，具体异常抛出说明有待补充。
    /// 5.有关线程池并发数和连接超时设定值，请参考SysConstants常量设定值.
    /// @author JiahaiWu 2014-11-06
    /// </summary>
    internal class SQLiteConnPicker : IDisposable
    {
        /// <summary>
        /// 获取单点数据库连接的构造方法
        /// </summary>
        /// <param name="sconn"></param>
        public SQLiteConnPicker(ConnLabel connLabel)
        {
#if DEBUG
            if (connLabel.connectStr == null || connLabel.connectStr.Length == 0)//检查传进来的数据库连接url是否合法
                throw new IDCMException("connLabel should not be NULL for SQLiteConnPicker(ConnLabel)!");
#endif
            this.connectionStr = connLabel.connectStr;//赋值给当前类的connectionStr
            SQLiteConnPool poolHolder = null;//这个类保证了单源的数据库的连接池保护
            GConnectPool.TryGetValue(connectionStr, out poolHolder);//查看池中有没有指定的数据库连接池实例
            if (poolHolder == null)
            {
                poolHolder = new SQLiteConnPool(connectionStr, SysConstants.MAX_DB_REQUEST_POOL_NUM);//创建一个数据库连接池
                bool SQLiteConnAdded = GConnectPool.TryAdd(connectionStr, poolHolder);//把新建的链接str与链接存入池
                if (!SQLiteConnAdded)
                {
                    throw new IDCMDataException("Add SQLite Connection pool instance into GConnectPoll Failed.");
                }
            }
            holder = poolHolder.getConnection();
            PickerConnected = holder != null && holder.tryOpen(connectionStr);//尝试获取并打开数据库连接
        }

        /// <summary>
        /// 实现IDisposable中的接口定义Dispose方法，销毁当前信号灯资源
        /// 说明：
        /// 1.一旦调用了Dispose方法被调用，不再允许通过该实例获取任何新的数据库连接
        /// </summary>
        public void Dispose()
        {
            if (PickerConnected == false)
                return;
            if (holder == null)
                return;
            if (holder != null)
            {
                PickerConnected = false;
                holder.release();
                holder = null;
            }
        }
        /// <summary>
        /// 解开对象封装获得SQLite连接对象。
        /// 说明:
        /// 1.该方法可重入，可并用。
        /// 注意
        /// 1.该方法仅用于一次性SQL事务处理流程，且不需要外部的连接释放管理操作。
        /// 2.请注意安全使用本方法获取的连接实例，SQLiteConnPicker对象实例可重用。
        /// 3.但外部对于获取到的SQLiteConnection句柄不得长时占用及再次缓存利用是不能许可的，暂时对此连接对象暴露暂无良好封装。
        /// @author JiahaiWu 2014-11-06
        /// </summary>
        /// <returns>SQLiteConnection (null able)</returns>
        public SQLiteConnection getConnection()
        {
            return (this.PickerConnected && holder != null) ? holder.Sconn : null;
        }

        /// <summary>
        /// 销毁单源数据库连接池<br/>
        /// 说明：
        /// 1.Passes a shutdown request to the SQLite core library. Does not throw an exception if the shutdown request fails.
        /// </summary>
        internal static void shutdown(ConnLabel connLabel)
        {
            SQLiteConnPool poolHolder = null;
            GConnectPool.TryRemove(connLabel.connectStr, out poolHolder);
            if (poolHolder != null)
            {
                poolHolder.shutdown();
            }
        }
        /// <summary>
        /// 销毁所有库的所有连接资源
        /// 注意：
        /// 1.正常情况下无需使用全局销毁方法
        /// </summary>
        internal static void shutdownAll()
        {
            foreach (SQLiteConnPool holder in GConnectPool.Values)
            {
                holder.shutdown();
            }
            GConnectPool.Clear();
            SQLiteConnection.ClearAllPools();
            //////////////////////////////////////////////////////////////
            //SQLiteConnection.Shutdown(true,true);
            //GC.WaitForPendingFinalizers();
            //GC.Collect();
            //////////////////////////////////////////////////////////////
            //Safely Operation but take long time for GC Collect.
            //@Deprecated
        }

        #region 内置实例对象保持部分
        /// <summary>
        /// 数据库连接串
        /// </summary>
        private readonly string connectionStr = null;
        private volatile SQLiteConnPool.SQLiteConnHolder holder = null;
        /// <summary>
        /// picker连接状态标记
        /// </summary>
        private volatile bool PickerConnected = false;
        /// <summary>
        /// 全局多点数据库连接的连接池缓存对象
        /// </summary>
        private static volatile ConcurrentDictionary<string, SQLiteConnPool> GConnectPool = new ConcurrentDictionary<string, SQLiteConnPool>();
        #endregion
    }
}
