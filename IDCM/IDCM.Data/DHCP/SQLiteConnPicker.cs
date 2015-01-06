using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;
using System.Collections.Concurrent;


namespace IDCM.Data.DHCP
{
    /// <summary>
    /// 数据库连接池，获取连接选择器。
    /// 说明：
    /// 1.单点数据库连接访问的串行保护类,封装SQLiteConnection用于多线程串行共享。
    /// 2.内置全局多点数据库连接的连接池,对多数据库实例提供支持。
    /// 3.本类允许多实例化及单实例复用策略，但外部获取SQLiteConnection句柄后不得长时占用及缓存复用。
    /// @author JiahaiWu 2014-11-06
    /// </summary>
    internal class SQLiteConnPicker : IDisposable
    {
        /// <summary>
        /// 获取单点数据库连接的构造方法
        /// </summary>
        /// <param name="connStr"></param>
        public SQLiteConnPicker(string connStr)
        {
#if DEBUG
            if (connStr == null || connStr.Length == 0)//检查传进来的数据库连接url是否合法
                throw new ArgumentNullException("connStr is NULL for SQLiteConnPicker(string)!");
#endif
            this.connectionStr = connStr;//赋值给当前类的connectionStr
            SQLiteConnHolder holder = null;//这个类是为了保证数据库单点链接，构造方法中有个信号灯
            connectPool.TryGetValue(connectionStr, out holder);//查看池中有没有指定的数据库连接str与链接
            if (holder == null)
            {
                holder = new SQLiteConnHolder(connectionStr);//创建一个数据库连接，创建一个信号灯
                lock (SQLiteConnPicker_Lock)//因为连接池是公共资源，所以在池中存取都要加锁
                {
                    bool SQLiteConnAdded = connectPool.TryAdd(connectionStr, holder);//把新建的链接str与链接存入池
#if DEBUG
                    System.Diagnostics.Debug.Assert(SQLiteConnAdded);//添加成功返回一个消息
#endif
                }
            }
        }
        /// <summary>
        /// 实现IDisposable中的接口定义，销毁当前连接资源
        /// </summary>
        public void Dispose()
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(connectionStr != null && connectionStr.Length > 0);
#endif
            SQLiteConnHolder holder = null;
            connectPool.TryGetValue(connectionStr, out holder);
            if (holder != null)
            {
                holder.release();
            }
        }
        /// <summary>
        /// 解开对象封装获得SQLite连接对象。
        /// 说明:
        /// 1.该方法可重入，可并用。
        /// 注意
        /// 1.该方法仅用于一次性SQL事务处理流程，且不需要外部的连接释放管理操作。
        /// 2.请注意安全使用本方法获取的连接实例，SQLiteConnPicker对象实例可重用。
        /// 3.但外部对于获取到的SQLiteConnection句柄不得长时（$time > MAX_WAIT_TIME_OUT）占用及再次缓存利用，对此更进一步的封装未能有效实现。
        /// @author JiahaiWu 2014-11-06
        /// </summary>
        /// <returns>SQLiteConnection (null able)</returns>
        public SQLiteConnection getConnection()
        {
            SQLiteConnHolder holder = null;
            connectPool.TryGetValue(connectionStr, out holder);
            if(holder != null)
            {
                if(holder.tryOpen(connectionStr))
                    return holder.Sconn;
            }
            return null;
        }
        /// <summary>
        /// 销毁数据库资源连接<br/>
        /// 说明：
        /// 1.Passes a shutdown request to the SQLite core library. Does not throw an exception if the shutdown request fails.
        /// </summary>
        internal void shutdown()
        {
            SQLiteConnHolder holder = null;
            connectPool.TryRemove(connectionStr, out holder);
            if (holder != null)
            {
                holder.kill();
            }
        }
        /// <summary>
        /// 销毁所有库的所有连接资源
        /// 注意：
        /// 1.正常情况下无需使用全局销毁方法
        /// </summary>
        internal static void shutdownAll()
        {
            lock (SQLiteConnPicker_Lock)
            {
                foreach (SQLiteConnHolder holder in connectPool.Values)
                {
                    holder.kill();
                }
                connectPool.Clear();
            }
        }

        #region 内置实例对象保持部分
        /// <summary>
        /// 数据库连接串
        /// </summary>
        private volatile string connectionStr = null;

        /// <summary>
        /// 多点数据库连接的连接池缓存对象
        /// </summary>
        private static volatile ConcurrentDictionary<string, SQLiteConnHolder> connectPool = new ConcurrentDictionary<string, SQLiteConnHolder>();
        /// <summary>
        /// 最长等待毫秒数（默认为5000ms）
        /// </summary>
        protected static int MAX_WAIT_TIME_OUT = 5000;
        /// <summary>
        /// 用于保持串行获取数据库连接的共享锁对象
        /// </summary>
        public static object SQLiteConnPicker_Lock = new object();
        #endregion


        #region SQLiteConnHolder
        /// <summary>
        /// Inner Class for Connection Holding Obeject Definition
        /// 单点数据库连接访问保持句柄类
        /// @author JiahaiWu 2014-11-06
        /// </summary>
        protected class SQLiteConnHolder
        {
            public SQLiteConnHolder(string connString)
            {
                sconn = new SQLiteConnection(connString);
                semaphore = new Semaphore(1, 1);
            }
            /// <summary>
            /// 数据库连接句柄
            /// </summary>
            private SQLiteConnection sconn = null;

            public SQLiteConnection Sconn
            {
                get { return sconn; }
            }
            /// <summary>
            /// 同步信号量
            /// </summary>
            private Semaphore semaphore;
            /// <summary>
            /// 尝试打开单点数据库连接
            /// 说明：
            /// 1.借助于信号量机制实现串行获取连接过程。
            /// </summary>
            /// <param name="connectionStr"></param>
            /// <returns></returns>
            internal bool tryOpen(string connectionStr = null)
            {
                if (semaphore.WaitOne(MAX_WAIT_TIME_OUT, false))
                {
                    try
                    {
                        if (sconn != null)//如果链接不为空
                        {
                            //链接处于非打开状态，且连接接没有关闭
                            if (!sconn.State.Equals(ConnectionState.Open) && !sconn.State.Equals(ConnectionState.Closed))
                            {
                                sconn.Close();//关闭连接
                            }
                        }
                        else
                        {
                            sconn = new SQLiteConnection(connectionStr);//如果链接为空
                        }
                        if (!sconn.State.Equals(ConnectionState.Open))//如果链接没有打开           
                            sconn.Open();//打开链接
                        return true;//成功打开链接返回true;
                    }
                    catch (Exception ex)
                    {
                        throw new SQLiteException(ex.Message, ex);
                    }
                    finally
                    {
                        semaphore.Release();  //释放被阻塞的信号量计数值
                    }
                }
                //信号量等待超时！！
                //则试图阻断长时占用的SQLiteConnect连接实例
                try
                {
                    if (sconn != null)//如果链接不为空
                    {
                        //链接处于打开状态，且链接没有关闭
                        if (!sconn.State.Equals(ConnectionState.Open) && !sconn.State.Equals(ConnectionState.Closed))
                        {
                            sconn.Close();//关闭连接
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new SQLiteException(ex.Message, ex);
                }
                finally
                {
                    semaphore.Release();  //释放被阻塞的信号量计数值
                }
                return false;
            }
            /// <summary>
            /// 销毁连接资源
            /// </summary>
            internal void release()
            {
                if (sconn != null)
                {
                    if (!sconn.State.Equals(ConnectionState.Closed))
                        sconn.Close();
                }
                //释放信号量控制
                //如果 Release 方法引发了 SemaphoreFullException，不一定表示调用线程有问题。 另一个线程中的编程错误可能导致该线程退出信号量的次数超过它进入的次数。
                semaphore.Release();
            }
            /// <summary>
            /// 彻底销毁连接资源
            /// </summary>
            internal void kill()
            {
                if (semaphore.WaitOne(MAX_WAIT_TIME_OUT, true))
                {
                    if (sconn != null && sconn.State != ConnectionState.Closed)
                    {
                        sconn.Close();
                    }
                    sconn.Dispose();
                    sconn.Shutdown();
                    sconn = null;
                }
                semaphore.Close();
                semaphore.Dispose();
            }
        }
        #endregion 
    }
}
