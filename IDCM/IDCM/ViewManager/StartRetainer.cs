using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.ViewManager
{
    /// <summary>
    /// 初始化载入信息管理
    /// 说明：
    /// 1.每次初始化显示欢迎登陆页面，主要包含数据文件地址、访问用户及其密码三部分。
    /// </summary>
    class StartRetainer:RetainerA
    {
         
        #region 构造&析构
        public StartRetainer()
        {
            new System.Threading.Thread(delegate() { OnSignInHold(null, null); }).Start();
        }

        public static StartRetainer getInstance()
        {
            ManagerI am = IDCMAppContext.MainManger.getManager(typeof(StartRetainer));
            return am == null ? null : (am as StartRetainer);
        }
        ~StartRetainer()
        {
            dispose();
        }
        #endregion
        #region 实例对象保持部分

        private StartInfo startInfo =null;
        #endregion
        #region 接口实例化部分
        public override void dispose()
        {
            base.dispose();
            if (signMonitor != null && !signMonitor.Enabled)
            {
                signMonitor.Stop();
                signMonitor.Dispose();
                signMonitor = null;
            }
        }
        
        /// <summary>
        /// 对象实例化初始化方法
        /// </summary>
        /// <returns></returns>
        public override bool initView(bool activeShow = true)
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
                        AuthInfoDAM.updateLastAuthInfo(authInfo);
                        IDCMFormManger.getInstance().updateUserStatus(authInfo.Username);
                    }
                    else
                        IDCMFormManger.getInstance().updateUserStatus(null);
                }
            }
            return true;
        }
        public override bool isDisposed()
        {
            return _isDisposed;
        }
        #endregion
    }
}
