using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.AppContext;
using IDCM.Service;
using IDCM.Service.POO;
using IDCM.Forms;
using System.Windows.Forms;

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
            startInfo = IDCMEnvironment.getLastStartInfo();
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
            startInfo = null;
        }
        
        /// <summary>
        /// 对象实例化初始化方法
        /// </summary>
        /// <returns></returns>
        public override bool initView(bool activeShow = true)
        {
            if (activeShow)
            {
                StartView startView = new StartView();
                startView.setReferStartInfo(ref startInfo);
                DialogResult res = startView.ShowDialog();
                if(res.Equals(DialogResult.OK))
                {
                    if(DataSourceHolder.chooseWorkspace(startInfo.Location,startInfo.LoginName))
                    {
                        DataSourceHolder.GCMLogin(startInfo.LoginName, startInfo.GCMPassword);
                        noteStartInfo(startInfo.Location,startInfo.asDefaultWorkspace,startInfo.LoginName,startInfo.rememberPassword?startInfo.GCMPassword:null);
                        startView.Dispose();
                        return true;
                    }
                }
                startView.Dispose();
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
