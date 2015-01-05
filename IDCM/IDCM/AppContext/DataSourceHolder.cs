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

namespace IDCM.AppContext
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
        public static bool connectWorkspace(string location,string loginName)
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
        public static bool connectGCM(string loginName,string gcmPassword,bool autoLogin=true)
        {
            AuthInfo auth = SignInExecutor.SignIn(loginName, gcmPassword,autoLogin);
            if (auth != null && auth.autoLogin)
            {
                signMonitor = new System.Windows.Forms.Timer();
                signMonitor.Interval = 600000;
                signMonitor.Tick += OnSignInHold;
                signMonitor.Start();
                new System.Threading.Thread(delegate() { OnSignInHold(null, null); }).Start();
            }
            return auth!=null && auth.LoginFlag;
        }
        /// <summary>
        /// 登录状态验证与保持
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnSignInHold(object sender, EventArgs e)
        {
#if DEBUG
            Console.WriteLine("* Sign in hold For Login()");
#endif
            if (authInfo != null && authInfo.autoLogin && authInfo.Username != null && authInfo.Password != null)
            {
                authInfo = SignInExecutor.SignIn(authInfo.Username, authInfo.Password, authInfo.autoLogin);
            }
        }
        /// <summary>
        /// 获取有效登录用户身份信息，如果登录状态无效则返回null
        /// </summary>
        /// <returns></returns>
        public static AuthInfo getSignedAuthInfo()
        {
            if (authInfo != null)
            {
                long elapsedTicks = DateTime.Now.Ticks - authInfo.Timestamp;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                if (elapsedSpan.TotalMinutes > 15)
                {
                    if (authInfo.Username != null && authInfo.Password != null)
                    {
                        authInfo = SignInExecutor.SignIn(authInfo.Username, authInfo.Password, authInfo.autoLogin,3000);
                    }
                }
                return (authInfo != null && authInfo.LoginFlag) ? authInfo : null;
            }
            return null;
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
                    log.Debug("Prepare datasource succeed.");
                    return true;
                }else
                {
                    MessageBox.Show("Failed to open the data source. ");
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
            if (signMonitor != null)
            {
                signMonitor.Stop();
                signMonitor = null;
                authInfo = null;
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
        #region 实例对象保持部分
        /// <summary>
        /// 数据源实例
        /// </summary>
        private static volatile DataSourceMHub dataSource = null;

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// SignIn hold Monitor
        /// </summary>
        private static System.Windows.Forms.Timer signMonitor = null;
        private static AuthInfo authInfo = null;
        #endregion
    }
}
