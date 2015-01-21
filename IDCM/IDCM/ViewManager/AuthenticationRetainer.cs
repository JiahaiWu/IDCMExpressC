using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.AppContext;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Forms;
using IDCM.Service;
using IDCM.Service.Common;
using IDCM.Core;

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
        private GCMSiteMHub gcmMHub;
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
            if (activeShow)
            {
                AuthInfo authInfo=getLoginAuthInfo();
                if (authInfo != null && authInfo.LoginFlag == true) //登录成功
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
                    authInfo = getLoginAuthInfo();
                    signin.Dispose();
                    string tip = authInfo.LoginFlag ? authInfo.Username : null;
                    DWorkMHub.note(new AsyncMessage(AsyncMessage.UpdateGCMSignTip,new string[]{tip}));
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 获取有效登录用户身份信息，如果登录状态无效则返回null
        /// </summary>
        /// <returns></returns>
        public AuthInfo getLoginAuthInfo()
        {
            if (gcmMHub != null)
            {
                gcmMHub.getSignedAuthInfo();
            }
            return null;
        }
    }
}
