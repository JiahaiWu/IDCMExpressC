using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data;
using IDCM.Service.Common.GCMDAM;
using IDCM.Data.Base;
using IDCM.Data.Base.Utils;
/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.Service.Common
{
    /// <summary>
    /// GCM网站授权资源管理集线器类
    /// 说明：
    /// 1.本实现类主要支持访问GCM的用户身份保持与连接状态保持.
    /// </summary>
    public class GCMSiteMHub
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="loginName">GCM ID</param>
        /// <param name="gcmPassword">GCM Password</param>
        /// <param name="autoLogin"></param>
        public GCMSiteMHub(string loginName, string gcmPassword, bool autoLogin = true)
        {
            this.authInfo = new AuthInfo();
            this.loadedNoter = new Dictionary<string, int>();
            authInfo.Username = loginName;
            authInfo.Password = gcmPassword;
            authInfo.autoLogin = autoLogin;
        }
        /// <summary>
        /// Connect GCM server with specific Login Name and password.
        /// </summary>
        /// <returns></returns>
        public bool connect(int timeout = 10000)
        {
            try
            {
                AuthInfo auth = SignExecutor.SignIn(authInfo.Username, authInfo.Password, authInfo.autoLogin, timeout);
                if (auth != null && auth.autoLogin)
                {
                    signMonitor = new System.Windows.Forms.Timer();
                    signMonitor.Interval = 600000;
                    signMonitor.Tick += OnSignInHold;
                    signMonitor.Start();
                    new System.Threading.Thread(delegate() { OnSignInHold(null, null); }).Start();
                }
                return auth != null && auth.LoginFlag;
            }
            catch (Exception ex)
            {
                log.Warn(ex);
                return false;
            }
        }
        /// <summary>
        /// 登录状态验证与保持
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSignInHold(object sender, EventArgs e)
        {
#if DEBUG
            log.Info("* Sign in hold For Login()");
#endif
            if (authInfo != null && authInfo.autoLogin && authInfo.Username != null && authInfo.Password != null)
            {
                AuthInfo auth = SignExecutor.SignIn(authInfo.Username, authInfo.Password, authInfo.autoLogin);
                authInfo.LoginFlag = auth.LoginFlag;
                authInfo.Jsessionid = auth.Jsessionid;
                authInfo.Timestamp = auth.Timestamp;
                string tip = authInfo.LoginFlag ? authInfo.Username : null;
                DWorkMHub.note(new AsyncMessage(AsyncMessage.UpdateGCMSignTip, tip == null ? null : new string[] { tip }));
            }
        }
        /// <summary>
        /// 获取有效登录用户身份信息，如果登录状态无效则返回null
        /// </summary>
        /// <returns></returns>
        public AuthInfo getSignedAuthInfo()
        {
            try{
                if (authInfo != null)
                {
                    long elapsedTicks = DateTime.Now.Ticks - authInfo.Timestamp;
                    TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                    if (elapsedSpan.TotalMinutes > 15)
                    {
                        if (authInfo.Username != null && authInfo.Password != null)
                        {
                            AuthInfo auth = SignExecutor.SignIn(authInfo.Username, authInfo.Password, authInfo.autoLogin, 3000);
                            authInfo.LoginFlag = auth.LoginFlag;
                            authInfo.Jsessionid = auth.Jsessionid;
                            authInfo.Timestamp = auth.Timestamp;
                            string tip = authInfo.LoginFlag ? authInfo.Username : null;
                            DWorkMHub.note(new AsyncMessage(AsyncMessage.UpdateGCMSignTip, tip == null ? null : new string[] { tip }));
                        }
                    }
                    return (authInfo != null && authInfo.LoginFlag) ? authInfo : null;
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex);
            }
            return null;
        }
        /// <summary>
        /// 关闭用户工作空间
        /// </summary>
        /// <returns></returns>
        public bool disconnect(bool cancelDefaultWorkSpace=false)
        {
            if (signMonitor != null)
            {
                signMonitor.Stop();
                signMonitor = null;
                SignExecutor.SignOff(authInfo);
                string tip = authInfo.LoginFlag ? authInfo.Username : null;
                DWorkMHub.note(new AsyncMessage(AsyncMessage.UpdateGCMSignTip, tip == null ? null : new string[] { tip }));
            }
            signMonitor = null;
            if (loadedNoter != null)
            {
                loadedNoter.Clear();
                DWorkMHub.note(AsyncMessage.UpdateLocalLinkTags);
            }
            loadedNoter = null;
            if (cancelDefaultWorkSpace)
            {
                ConfigurationHelper.SetAppConfig(SysConstants.LWSAsDefault, "False");
            }
            return true;
        }
        public Dictionary<string, int> getLoadedNoter()
        {
            return loadedNoter;
        }
        /// <summary>
        /// 返回所有strain_id
        /// </summary>
        /// <param name="strainID_cellIndex_Map"></param>
        /// <returns></returns>
        public string[] getStrainID()
        {
            string[] idArray = new string[loadedNoter.Count];
            int i = 0;
            foreach (KeyValuePair<string, int> kvp in loadedNoter)
            {
                idArray[i++] = kvp.Key;
            }
            return idArray;
        }

        private Dictionary<string, int> loadedNoter = null;
        /// <summary>
        /// SignIn hold Monitor
        /// </summary>
        private System.Windows.Forms.Timer signMonitor = null;
        private readonly AuthInfo authInfo = null;
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
