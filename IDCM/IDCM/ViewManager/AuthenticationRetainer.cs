using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.AppContext;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Forms;
using IDCM.Service;

namespace IDCM.ViewManager
{
    /// <summary>
    /// 用户身份认证管理
    /// </summary>
    public class AuthenticationRetainer:ManagerI
    {
        
        #region 构造&析构
        public AuthenticationRetainer()
        {
            signMonitor = new System.Windows.Forms.Timer();
            signMonitor.Interval = 600000;
            signMonitor.Tick += OnSignInHold;
            signMonitor.Start();
            new System.Threading.Thread(delegate() { OnSignInHold(null, null); }).Start();
        }

        public static AuthenticationRetainer getInstance()
        {
            ManagerI am = ViewManagerHolder.getManager(typeof(AuthenticationRetainer));
            return am == null ? null : (am as AuthenticationRetainer);
        }
        ~AuthenticationRetainer()
        {
            dispose();
        }
        #endregion
        #region 实例对象保持部分

        /// <summary>
        /// SignIn hold Monitor
        /// </summary>
        private System.Windows.Forms.Timer signMonitor = null;
        private AuthInfo authInfo =null;
        #endregion
        #region 接口实例化部分
        public void setMaxToNormal()
        {
        }
        public void setToMaxmize(bool activeFront = false)
        {
        }
        public void setMdiParent(Form pForm)
        {
        }
        public void dispose()
        {
            if (signMonitor != null && !signMonitor.Enabled)
            {
                signMonitor.Stop();
                signMonitor.Dispose();
                signMonitor = null;
            }
        }
        public bool isActive()
        {
            return false;
        }
        public bool isDisposed()
        {
            return _isDisposed;
        }
        protected volatile bool _isDisposed = false;
        /// <summary>
        /// 对象实例化初始化方法
        /// </summary>
        /// <returns></returns>
        public bool initView(bool activeShow = true)
        {
            if (signMonitor == null)
            {
                signMonitor = new System.Windows.Forms.Timer();
                signMonitor.Interval = 600000;
                signMonitor.Tick += OnSignInHold;
                signMonitor.Start();
            }
            if (activeShow)
            {
                if (checkLoginStatus())
                {
                    LoginStatusDlg loginStatus = new LoginStatusDlg();
                    loginStatus.setSignInInfo(authInfo.Username, authInfo.Timestamp);
                    loginStatus.ShowDialog();
                    loginStatus.Dispose();
                }
                else
                {
                    SignInDlg signin = new SignInDlg();
                    signin.setReferAuthInfo(authInfo);
                    DialogResult res = signin.ShowDialog();
                    authInfo=signin.getAuthInfo();
                    signin.Dispose();
                    if (authInfo.LoginFlag)
                    {
                        //AuthInfoDAM.updateLastAuthInfo(authInfo);
                        IDCMFormManger.getInstance().updateUserStatus(authInfo.Username);
                    }
                    else
                        IDCMFormManger.getInstance().updateUserStatus(null);
                }
            }
            return true;
        }
        #endregion
        /// <summary>
        /// 尝试发起即时的网络请求操作
        /// </summary>
        /// <returns></returns>
        protected bool checkLoginStatus()
        {
            if (authInfo == null)
            {
                authInfo = new AuthInfo();
            }
            else
            {
                long elapsedTicks = DateTime.Now.Ticks - authInfo.Timestamp;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                if (elapsedSpan.TotalMinutes > 15)
                {
                    if (authInfo.Username != null && authInfo.Password != null)
                    {
                        //authInfo = SignInExecutor.SignIn(authInfo.Username, authInfo.Password, 2000, authInfo.autoLogin);
                    }
                    return false;
                }
            }
            return authInfo.LoginFlag;
        }
        /// <summary>
        /// 获取有效登录用户身份信息，如果登录状态无效则返回null
        /// </summary>
        /// <returns></returns>
        public AuthInfo getLoginAuthInfo()
        {
            if (authInfo != null)
            {
                long elapsedTicks = DateTime.Now.Ticks - authInfo.Timestamp;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                if (elapsedSpan.TotalMinutes > 15)
                {
                    if (authInfo.Username != null && authInfo.Password != null)
                    {
                        //authInfo = SignInExecutor.SignIn(authInfo.Username, authInfo.Password, 2000, authInfo.autoLogin);
                    }
                }
                return (authInfo != null && authInfo.LoginFlag) ? authInfo : null;
            }
            return null;
        }
        /// <summary>
        /// 登录状态验证与保持
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSignInHold(object sender, EventArgs e)
        {
#if DEBUG
            Console.WriteLine("* Sign in hold For Login()");
#endif
            //if (authInfo == null)
            //    authInfo = AuthInfoDAM.queryLastAuthInfo();
            //if (authInfo != null && authInfo.autoLogin && authInfo.Username != null && authInfo.Password != null)
            //{
            //    authInfo = SignInExecutor.SignIn(authInfo.Username, authInfo.Password, 10000, authInfo.autoLogin);
            //    authInfo.autoLogin = true;
            //    if (authInfo.LoginFlag)
            //    {
            //        AuthInfoDAM.updateLastAuthInfo(authInfo);
            //        IDCMFormManger.getInstance().updateUserStatus(authInfo.Username);
            //    }
            //    else
            //        IDCMFormManger.getInstance().updateUserStatus(null);
            //}
        }
    }
}
