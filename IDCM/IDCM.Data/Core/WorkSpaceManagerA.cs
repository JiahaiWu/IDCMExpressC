using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Common;
using IDCM.Data.DHCP;

namespace IDCM.Data.Core
{
    /// <summary>
    /// 基于特定数据存储引擎的工作空间管理器的抽象类定义
    /// @author JiahaiWu 2014-12-26
    /// </summary>
    public abstract class WorkSpaceManagerA
    {
        #region 默认实现部分
        /// <summary>
        /// 工作空间管理器构造方法，要求指定标准格式的连接句柄
        /// </summary>
        /// <param name="dbPath">数据存档文件路径</param>
        /// <param name="password">数据存档加密字符串</param>
        public WorkSpaceManagerA(string dbPath,string pwd=null)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(dbPath != null && dbPath.Length > 0, "DB Path should not be null or empty char sequence.");
#endif
            this.DBPath = dbPath;
            this.password = pwd;
        }
        
        /// <summary>
        /// 获取当前数据源状态标识
        /// </summary>
        /// <returns></returns>
        public string getStatus()
        {
            return _status.ToString();
        }
        /// <summary>
        /// 获取当前有效连接句柄标识
        /// 说明：
        /// 如果连接串不可用，则返回null
        /// </summary>
        /// <returns></returns>
        public string getValidConnectStr()
        {
            if (_status.Equals(WSStatus.InWorking))
            {
                return sconn.connectStr;
            }
            return null;
        }
        /// <summary>
        /// 获取最近一次的错误消息描述
        /// 说明:
        /// 如果没有记录到的错误,则返回null
        /// </summary>
        /// <returns></returns>
        public string getLastError()
        {
            return _lastError==null?null:_lastError.ToString();
        }
        /// <summary>
        /// 设置最近一次的错误消息描述
        /// </summary>
        /// <returns></returns>
        internal void setLastError(ErrorNote error)
        {
            log.Error(error);
            _lastError = error;
        }
        /// <summary>
        /// 获取数据库连接器对象实例
        /// </summary>
        /// <param name="renew"></param>
        /// <returns></returns>
        internal ConnLabel getConnection(bool doCheck = false)
        {
            if (doCheck == true)
            {
                if (sconn != null)
                {
                    try
                    {
                        using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
                        {
                            picker.getConnection();
                        }
                    }
                    catch (Exception)
                    {
                        sconn = null;
                    }
                }
            }
            return sconn;
        }
        #endregion
        #region 虚拟定义部分
        public virtual bool connect()
        {
            return false;
        }
        public virtual bool prepare()
        {
            return false;
        }
        public virtual bool disconnect()
        {
            return false;
        }
        #endregion
        #region 内置实例对象保持部分
        /// <summary>
        /// 用户工作空间运营状态标识
        /// </summary>
        protected volatile WSStatus _status = WSStatus.Idle;
        /// <summary>
        /// 数据库连接句柄标识
        /// </summary>
        protected volatile ConnLabel sconn = null;
        /// <summary>
        /// 最近一次错误记录
        /// </summary>
        private volatile ErrorNote _lastError = null;
        /// <summary>
        /// 数据库目标存档路径
        /// </summary>
        public readonly string DBPath;
        /// <summary>
        /// 数据库访问密码
        /// </summary>
        public readonly string password;
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        #endregion
    }
}
