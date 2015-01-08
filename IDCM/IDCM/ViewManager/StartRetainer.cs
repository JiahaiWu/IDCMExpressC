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

namespace IDCM.ViewManager
{
    /// <summary>
    /// 初始化载入信息管理
    /// 说明：
    /// 1.每次初始化显示欢迎登陆页面，主要包含数据文件地址、访问用户及其密码三部分。
    /// </summary>
    class StartRetainer:ManagerA
    {
         
        #region 构造&析构
        public StartRetainer()
        {
            startInfo = IDCMEnvironment.getLastStartInfo();
            startView = new StartView();
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
                startView.FormClosed += OnStartViewClosed;
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
            if (res.Equals(CloseReason.UserClosing) || res.Equals(CloseReason.FormOwnerClosing))
            {
                if (startInfo.Location != null && startInfo.LoginName != null)
                {
                    Form waitingForm = new WaitingForm();
                    waitingForm.Show();
                    waitingForm.Update();
                    waitingForm.UseWaitCursor = true;
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
