using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.AppContext;
using IDCM.Service;
using IDCM.Service.POO;
using IDCM.Forms;
using System.Windows.Forms;
using IDCM.Service.Common.Core;
using IDCM.Service.Common;
using System.Configuration;
using IDCM.Data.Base;
using IDCM.Core;

namespace IDCM.ViewManager
{
    /// <summary>
    /// 初始化载入信息管理
    /// 说明：
    /// 1.每次初始化显示欢迎登陆页面，主要包含数据文件地址、访问用户及其密码三部分。
    /// 2.默认情况下每次启动都将显示欢迎页面，当有用户设置了默认的工作空间生效后，则会在启动过程中快速跳过用户输入的环节。
    /// 3.用户登录名称建议使用与GCM上注册通过的账号，以便可以有效实现数据同步功能。使用无效用户名，将仅能实现本地数据库访问管理。
    /// 4.这里的登录用户名将作为本地数据库访问的校验码，数据库实例成功加载要求正确的输入登录用户名（初始数据库指定用户名例外）。
    /// 5.这里的登录密码对应的是GCM上注册账号的密码，该密码不在本地数据库中发挥作用，主要用于GCM网络数据资源访问的登录验证的条件。
    /// 6.在输入登录密码的时候，登录用户名不可为空，否则登录用户名和登录密码均可以空值设定。
    /// 7.GCM注册账号的有效性验证并不会阻止用户进入本地数据库管理界面，但正确输入可以及时打开GCM网络数据资源同步服务。
    /// </summary>
    class StartRetainer:ManagerA
    {
         
        #region 构造&析构
        public StartRetainer()
        {
            startInfo = IDCMEnvironment.getLastStartInfo();
            startView = new StartView();
            startView.FormClosed += OnStartViewClosed;
            startView.OnRequestHelp += OnStartViewRequestHelp;
        }

        public static StartRetainer getInstance()
        {
            ManagerI am = ViewManagerHolder.getManager(typeof(StartRetainer));
            return am == null ? null : (am as StartRetainer);
        }
        ~StartRetainer()
        {
            dispose();
        }
        #endregion
        #region 实例对象保持部分

        private StartInfo startInfo =null;
        private StartView startView = null;
        #endregion
        #region 接口实例化部分
        public override void dispose()
        {
            base.dispose();
            startInfo = null;
        }
        public override void setMdiParent(Form pForm)
        {
            if(!isDisposed())
                startView.MdiParent = pForm;
        }
        public override void setMaxToNormal()
        {
        }
        public override void setToMaxmize(bool activeFront = false)
        {
        }
        /// <summary>
        /// 对象实例化，显示用户界面方法
        /// </summary>
        /// <param name="activeShow"></param>
        /// <returns></returns>
        public override bool initView(bool activeShow = true)
        {
            if (activeShow)
            {
                startView.setReferStartInfo(ref startInfo);
                startView.Show();
            }
            return true;
        }
        /// <summary>
        /// 当用户操作确认后窗口关闭，触发必要的数据加载流程和显示Loding进程状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnStartViewClosed(object sender, FormClosedEventArgs e)
        {
            CloseReason res = e.CloseReason;
            DialogResult dres = startView.DialogResult;
            if (res.Equals(CloseReason.UserClosing) || dres.Equals(DialogResult.OK))
            {
                if (startInfo.Location != null && startInfo.LoginName != null)
                {
                    Form waitingForm = new WaitingForm();
                    waitingForm.MdiParent = startView.MdiParent;
                    waitingForm.Show();
                    waitingForm.BringToFront();
                    waitingForm.Update();
                    if (DataSourceHolder.connectWorkspace(startInfo.Location, startInfo.LoginName))
                    {
                        if (startInfo.GCMPassword != null)
                        {
                            DataSourceHolder.connectGCM(startInfo.LoginName, startInfo.GCMPassword);
                        }
                        IDCMEnvironment.noteStartInfo(startInfo.Location, startInfo.asDefaultWorkspace, startInfo.LoginName, startInfo.rememberPassword ? startInfo.GCMPassword : null);
                        DataSourceHolder.prepareInstance();
                        DWorkMHub.note(AsyncMessage.DataPrepared);
                    }
                    else
                    {
                        MessageBox.Show("DataSource Open Failed.");  //MessageBox.Show("DataSource Open Failed. You can input again by 'Open' menu item in 'File' menu item.");
                        DWorkMHub.note(AsyncMessage.RetryQuickStartConnect);
                    }
                    waitingForm.Close();
                    waitingForm.Dispose();
                }
            }
            //else if(dres.Equals(DialogResult.Cancel))
            //{
            //    DialogResult cres= MessageBox.Show("Close IDCM Application?", MessageBoxButtons.YesNo);
            //    if(cres.Equals(DialogResult.Yes))
            //        DWorkMHub.note(AsyncMessage.RequestCloseIDCMForm);
            //}
        }
        public void OnStartViewRequestHelp(object sender, HelpEventArgs e)
        {
            string helpBase = ConfigurationManager.AppSettings.Get(SysConstants.HelpBase);
            //浏览器跳转到帮助说明文档
            //有待补充
        }

        public override bool isDisposed()
        {
            if (_isDisposed == false)
            {
                _isDisposed = (startView == null || startView.Disposing || startView.IsDisposed);
            }
            return _isDisposed;
        }
        #endregion
        
    }
}
