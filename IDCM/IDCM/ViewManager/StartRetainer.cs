using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.AppContext;
using IDCM.Service;
using IDCM.Service.POO;
using IDCM.Forms;
using System.Windows.Forms;
using IDCM.Service.Common.Core.ServBuf;
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
        }
        public override void setMaxToNormal()
        {
        }
        public override void setToMaxmize(bool activeFront = false)
        {
        }
        /// <summary>
        /// 对象实例化初始化方法
        /// </summary>
        /// <returns></returns>
        public override bool initView(bool activeShow = true)
        {
            if (activeShow)
            {
                startView.setReferStartInfo(ref startInfo);
                DialogResult res = startView.ShowDialog();
                if(res.Equals(DialogResult.OK))
                {
                    Form waitingForm = new WaitingForm();
                    waitingForm.Show();
                    waitingForm.Update();
                    if(DataSourceHolder.connectWorkspace(startInfo.Location,startInfo.LoginName))
                    {
                        if (startInfo.GCMPassword != null)
                        {
                            DataSourceHolder.connectGCM(startInfo.LoginName, startInfo.GCMPassword);
                        }
                        IDCMEnvironment.noteStartInfo(startInfo.Location, startInfo.asDefaultWorkspace, startInfo.LoginName, startInfo.rememberPassword ? startInfo.GCMPassword : null);
                        DataSourceHolder.prepareInstance();
                        DWorkMHub.note(new AsyncMessage(MsgType.DataPrepared, "DataSource Prepared."));
                    }
                    waitingForm.Close();
                    waitingForm.Dispose();
                }
                startView.Close();
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
