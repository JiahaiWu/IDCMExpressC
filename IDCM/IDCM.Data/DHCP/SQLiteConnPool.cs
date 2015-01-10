using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;
using System.Collections.Concurrent;
using IDCM.Data.Base;

namespace IDCM.Data.DHCP
{
    /// <summary>
    /// 单源数据库连接选择器。
    /// 说明：
    /// 1.单点数据库连接访问的串行/并行保护类,封装SQLiteConnection用于多线程串行/并行共享。
    /// 2.本类允许多实例化策略，但外部获取SQLiteConnection及SQLiteConnPicker句柄后不应二次缓存使用或长时复用。
    /// 3.获取
    /// 4.连接异常信息需外部捕获，异常类型主要包括SQLiteException、IDCMException，具体异常抛出说明有待补充。
    /// 5.有关线程池连接超时设定值，请参考SysConstants常量设定值.
    /// @author JiahaiWu 2014-11-06
    internal class SQLiteConnPool
    {
        public SQLiteConnPool(string connString,int poolSize=1)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(connString!=null);
            System.Diagnostics.Debug.Assert(poolSize>0);
#endif
            poolSemaphore = new Semaphore(poolSize, poolSize);
            sconnHodlers = new ConcurrentDictionary<SQLiteConnHolder,long>();
            this.connString = connString;
            this.poolSize = poolSize;
        }

        /// <summary>
        /// 销毁数据库连接池
        /// 说明：
        /// 1.一旦调用了Dispose方法被调用，不再允许通过该实例获取任何新的数据库连接
        /// </summary>
        public void shutdown()
        {
            if (poolSemaphore != null)
            {
                for (int i = 0; i < poolSize; i++)
                {
                    poolSemaphore.WaitOne(SysConstants.MAX_DB_REQUEST_TIME_OUT);
                }
                if (sconnHodlers != null)
                {
                    foreach (SQLiteConnHolder holder in sconnHodlers.Keys)
                    {
                        long lts = 0;
                        if (sconnHodlers.TryRemove(holder, out lts))
                        {
                            holder.kill();
                        }
                    }
                    sconnHodlers = null;
                }
                poolSemaphore.Close();
                poolSemaphore = null;
            }
        }
        /// <summary>
        /// 解开对象封装获得SQLite连接对象。
        /// 说明:
        /// 1.该方法可重入，可并用。
        /// 注意
        /// 1.该方法仅用于一次性SQL事务处理流程，且不需要外部的连接释放管理操作。
        /// 2.请注意安全使用本方法获取的连接实例，SQLiteConnPicker对象实例可重用。
        /// 3.但外部对于获取到的SQLiteConnection句柄不得长时（$time > MAX_WAIT_TIME_OUT）占用及再次缓存利用是不能许可的，暂时对此连接对象暴露暂无良好封装。
        /// @author JiahaiWu 2014-11-06
        /// </summary>
        /// <returns>SQLiteConnection (null able)</returns>
        public SQLiteConnHolder getConnection()
        {
            if (poolSemaphore.WaitOne(SysConstants.MAX_DB_REQUEST_TIME_OUT))
            {
                SQLiteConnHolder holder = new SQLiteConnHolder(this);
                sconnHodlers.TryAdd(holder,DateTime.Now.Ticks);
                return holder;
            }
            else
            {
                //信号量等待超时！！
                throw new IDCMDataException("Waiting Time out for SQLiteConnPool get DB Connection, please check relative program coding.");
            }
        }
        /// <summary>
        /// 释放连接池内关于SQLiteConnHolder实例的引用关联
        /// </summary>
        /// <param name="holder"></param>
        /// <returns></returns>
        protected bool release(SQLiteConnHolder holder)
        {
            poolSemaphore.Release();
            long lts = 0;
            bool removed= sconnHodlers.TryRemove(holder,out lts);
            return removed;
        }

        /// <summary>
        /// 同步信号量
        /// </summary>
        /// 
        protected volatile Semaphore poolSemaphore;
        private readonly int poolSize;
        protected readonly string connString;
        protected volatile ConcurrentDictionary<SQLiteConnHolder,long> sconnHodlers;

        #region SQLiteConnHolder
        /// <summary>
        /// Inner Class for Connection Holding Obeject Definition
        /// 单点数据库连接访问保持句柄类
        /// @author JiahaiWu 2014-11-06
        /// </summary>
        internal class SQLiteConnHolder
        {
            internal SQLiteConnHolder(SQLiteConnPool pool)
            {
                ppool = pool;
                sconn = new SQLiteConnection(pool.connString);
            }
            /// <summary>
            /// 数据库连接句柄
            /// </summary>
            private volatile SQLiteConnection sconn = null;
            private volatile SQLiteConnPool ppool = null;
            internal SQLiteConnection Sconn
            {
                get { return sconn; }
            }
            /// <summary>
            /// 尝试打开单点数据库连接
            /// 说明：
            /// 1.借助于信号量机制实现串行获取连接过程。
            /// </summary>
            /// <param name="connectionStr"></param>
            /// <returns></returns>
            internal bool tryOpen(string connectionStr = null)
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
                return sconn.State.Equals(ConnectionState.Open); 
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
                    sconn = null;
                    if(ppool!=null)
                        ppool.release(this);
                }
            }
            /// <summary>
            /// 彻底销毁连接资源
            /// </summary>
            internal void kill()
            {
                SQLiteConnection oldSconn = sconn;
                release();
                if (oldSconn != null)
                {
                    SQLiteConnection.ClearPool(oldSconn);
                    oldSconn.Dispose();
                    oldSconn = null;
                }
            }
        }
        #endregion
    }
}
