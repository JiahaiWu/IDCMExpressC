using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using IDCM.Service;
using IDCM.Service.Common;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Service.BGHandler;

namespace IDCM.Service
{
    /// <summary>
    /// 工作空间保持及验证工具类
    /// @author Jiahai Wu
    /// @Date 2014-08-19
    /// </summary>
    class DataSourceHolder
    {
        /// <summary>
        /// Connect Workspace, it willl choose or create a new workspace in local disk.
        /// </summary>
        /// <returns></returns>
        internal static bool connectWorkspace(string location,string loginName)
        {
            dataSource = new DataSourceMHub(location, loginName);
            return dataSource.connect();
        }
        /// <summary>
        /// Connect GCM server with specific Login Name and password.
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="gcmPassword"></param>
        /// <param name="autoLogin"></param>
        /// <returns></returns>
        internal static bool connectGCM(string loginName, string gcmPassword, bool autoLogin = true)
        {
            gcmHolder = new GCMSiteMHub(loginName, gcmPassword, autoLogin);
            return gcmHolder.connect(2000);
        }
        /// <summary>
        /// Connect Workspace and Connect GCM server asynchronous with specific Login Name and password.
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="gcmPassword"></param>
        /// <param name="autoLogin"></param>
        /// <returns></returns>
        internal static bool quickConnect(string location, string loginName, string gcmPassword, bool autoLogin = true)
        {
            dataSource = new DataSourceMHub(location, loginName);
            gcmHolder = new GCMSiteMHub(loginName, gcmPassword, autoLogin);
            DWorkMHub.callAsyncHandle(new SignInHandler(gcmHolder));
            return dataSource.connect();
        }

        /// <summary>
        /// 启动工作空间实例
        /// </summary>
        public static bool prepareInstance()
        {
            if (dataSource.Connected)
            {
                if (dataSource.prepare())
                {
                    log.Info("Prepare datasource succeed.");
                    return true;
                }else
                {
                    log.Error(new IDCMServCommonException("Failed to open the data source. Cause:"+dataSource.LastError));
                    MessageBox.Show("Failed to open the data source. Cause:"+dataSource.LastError);
                }
            }
            return false;
        }
        /// <summary>
        /// 关闭用户工作空间
        /// </summary>
        /// <returns></returns>
        public static bool close()
        {
            if (gcmHolder != null)
            {
                gcmHolder.disconnect();
                gcmHolder = null;
            }
            bool res = false;
            if (dataSource != null)
            {
                res=dataSource.disconnect();
                dataSource = null;
            }
            return res;
        }

        /// <summary>
        /// 返回是否处于数据源连接保持中
        /// </summary>
        public static bool InWorking
        {
            get { return dataSource!=null && dataSource.InWorking; }
        }
        /// <summary>
        /// 获取连接数据源
        /// </summary>
        public static DataSourceMHub DataSource
        {
            get { return dataSource; }
        }
        #region 实例对象保持部分
        /// <summary>
        /// 数据源实例
        /// </summary>
        private static volatile DataSourceMHub dataSource = null;
        private static volatile GCMSiteMHub gcmHolder = null;

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        #endregion
    }
}
