using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.Forms;
using IDCM.Service.Common;
using IDCM.Service.Common.Core;
using IDCM.Service;
using IDCM.Data.Base;
using IDCM.Core;

/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.ViewManager
{
    /// <summary>
    /// IDCM用户交互界面主控管理器的实现类
    /// 说明：
    /// 1. 用户交互界面主控管理器持有一个或多个Form/View/Dialog的界面句柄资源，同时包含相应的界面组件域的实际托管类实现对象实例。
    /// 对于用户交互呈现资源包括Form/View/Dialog三类表现形式，View嵌套在Form中作为子视图，Dialog作为动态创建的子例程。
    /// 2.在界面布局中包含若干动态展示性部件，其具体动态交互事务实现由对应的界面组件域的实际托管类维护，在用户界面主控管理器中侧重集中式的事务控制中转过程。
    /// 3.
    /// @author JiahaiWu  2014-10-30
    /// </summary>
    class IDCMFormManger
    {
        #region 构造&析构
        /// <summary>
        /// 用户交互界面主控管理器构造方法
        /// </summary>
        public IDCMFormManger()
        {
            mainForm = new IDCMForm(); //创建主框架界面实例对象
            DWorkMHub.note(mainForm);
            mainForm.Shown += IDCMForm_Shown;
            mainForm.setManager(this); //通知界面实例对象绑定事件控制中转例程
        }
        /// <summary>
        /// 用于共享成员实例获取当前用户交互界面主控管理器
        /// 注意：
        /// 该静态方法获取实例对象可能为空，调用方法需负责检测获取实例失败的特例情形。
        /// </summary>
        /// <returns></returns>
        public static IDCMFormManger getInstance()
        {
            return IDCMAppContext.MainManger;
        }
        /// <summary>
        /// 用户交互界面主控管理器析构方法
        /// </summary>
        ~IDCMFormManger()
        {
            Dispose();
        }
        /// <summary>
        /// 立即释放当前主控管理器的实例资源
        /// 注意：
        /// 1.调用该方法意味着当前实例不再可用，目标状态即是将被自动销毁。
        /// 在后台任务轮询监视器的心跳检测事件处理中会根据此状态自动释放对当前实例句柄的引用。
        /// 2.该方法的实现务必保证冗余调用许可性，容错性，资源释放状态判定务必保持同步可用。
        /// </summary>
        public void Dispose()
        {
            if (mainForm != null && !mainForm.IsDisposed) //释放主窗口界面实例资源
            {
                mainForm.Close();
                mainForm.Dispose();
                mainForm = null;
            }
            ViewManagerHolder.Dispose();
        }
        #endregion
        #region 实例对象保持部分
        //声明释放主窗口界面实例
        internal IDCMForm mainForm = null;
        #endregion

        /// <summary>
        /// 主窗体初始化方法，用于激活新（或旧）实例的界面资源及其动态动态显示
        /// 注意：
        /// 1.该方法的实现务必保证冗余调用许可性，容错性，资源释放状态判定务必保持同步可用。
        /// </summary>
        /// <param name="activeShow">激活界面用户可见性（可选）</param>
        /// <returns>初始化成功与否状态</returns>
        public bool initForm(bool activeShow = true)
        {
            mainForm.WindowState = FormWindowState.Maximized;
            if (activeShow)
            {
                mainForm.Show();
            }
            else
                mainForm.Hide();
            return true;
        }

#region 接管视图组件的关键的事件处理区
        /// <summary>
        /// IDCMForm主界面第一次显示后，打开默认的登录启动页面展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDCMForm_Shown(object sender, EventArgs e)
        {
            //启动欢迎页面
            startWorkSpace();
        }

        /// <summary>
        /// 再次打开默认的登录启动页面展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnRetryQuickStartConnect(object sender, IDCMAsyncEventArgs e)
        {
            ViewManagerHolder.activeChildView(typeof(HomeViewManager), true);
        }
        /// <summary>
        /// 数据源预处理流程完成事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnDataPrepared(object sender, IDCMAsyncEventArgs e)
        {
            Form waitingForm = new WaitingForm();
            waitingForm.MdiParent = mainForm;
            waitingForm.Show();
            waitingForm.BringToFront();
            waitingForm.Update();
            ViewManagerHolder.activeChildView(typeof(HomeViewManager), false);
            ViewManagerHolder.activeChildView(typeof(HomeViewManager), false);
            DWorkMHub.note(AsyncMessage.RequestHomeView);
            waitingForm.Close();
            waitingForm.Dispose();
        }
        internal void OnActiveHomeView(object sender, IDCMAsyncEventArgs e)
        {
            ViewManagerHolder.activeChildView(typeof(HomeViewManager), true);
        }
        internal void OnActiveGCMView(object sender, IDCMAsyncEventArgs e)
        {
            ViewManagerHolder.activeChildView(typeof(GCMViewManager), true);
        }
        internal void OnRetryDataPrepare(object sender, IDCMAsyncEventArgs e)
        {
            //Unimplement!
        }
#endregion
        internal ManagerI getHomeViewManager()
        {
            return ViewManagerHolder.getManager(typeof(HomeViewManager));
        }
        internal ManagerI getGCMViewManager()
        {
            return ViewManagerHolder.getManager(typeof(GCMViewManager));
        }
        ///////////////////////////////////////////////////////////////
        ///// <summary>
        ///// 主窗体初始化方法，用于激活新（或旧）实例的界面资源及其动态动态显示
        ///// 注意：
        ///// 1.该方法的实现务必保证冗余调用许可性，容错性，资源释放状态判定务必保持同步可用。
        ///// </summary>
        ///// <param name="activeShow">激活界面用户可见性（可选）</param>
        ///// <returns>初始化成功与否状态</returns>
        //public bool initForm(bool activeShow = true)
        //{
        //    mainForm.WindowState = FormWindowState.Maximized;
        //    if (activeShow)
        //    {
        //        mainForm.Show();
        //    }
        //    else
        //        mainForm.Hide();
        //    return true;
        //}
        
        ///// <summary>
        ///// 显示等待提示页面，并隐式地激活直属视图实例及其必要的窗口显示操作。
        ///// </summary>
        ///// <param name="manager"></param>
        ///// <param name="activeFront"></param>
        ///// <returns></returns>
        //public void activeChildViewAwait(Type manager, bool activeFront = false)
        //{
        //    Form startForm = new StartForm();
        //    startForm.Show();
        //    startForm.Update();
        //    bool res = ViewManagerHolder.activeChildView(manager, activeFront);
        //    startForm.Close();
        //    startForm.Dispose();
        //}
        /////////////////////////////////////////////////////////////////
        //@Deprecated

        /// <summary>
        /// 启动当前工作空间
        /// </summary>
        /// <returns></returns>
        public bool startWorkSpace()
        {
            if (DataSourceHolder.InWorking)
            {
                DialogResult res = MessageBox.Show("A workspace is in working, you need close it first.", "Close Workspace Notice", MessageBoxButtons.OK);
                return false;
            }
            ManagerI view = ViewManagerHolder.getManager(typeof(StartRetainer));
            return view.initView(true);
        }
        /// <summary>
        /// 关闭当前工作空间，仅保留主框架窗口
        /// </summary>
        /// <returns></returns>
        public bool closeWorkSpace()
        {
            if (DataSourceHolder.InWorking)
            {
                ViewManagerHolder.Dispose();
                return DataSourceHolder.close();
            }
            return true;
        }
        /// <summary>
        /// 更新用户身份认证状态
        /// </summary>
        /// <param name="uname"></param>
        public void updateUserStatus(string uname = null)
        {
            string tip = "Off Line";
            if (uname != null)
                tip = "On Line: " + uname;
            mainForm.setLoginTip(tip);
        }
        public bool activeTemplateView()
        {
            ManagerI view = ViewManagerHolder.getManager(typeof(LibFieldManager));
            return view.initView(true);
        }
        public bool activeAuthView()
        {
            ManagerI view = ViewManagerHolder.getManager(typeof(AuthenticationRetainer));
            return view.initView(true);
        }
        public bool activeBackTaskInfoView()
        {
            ////////////////////////////////////////////////////////
            //ManagerI view = ViewManagerHolder.getManager(typeof(StackInfoManager));
            //return view.initView(true);
            ////////////////////////////////////
            //UnImplement!
            return false;
        }
        
        public void showDBDataSearch()
        {
            ManagerI mi = ViewManagerHolder.getManager(typeof(HomeViewManager));
            if (mi != null)
            {
                HomeViewManager hvManager = (HomeViewManager)mi;
                if (hvManager != null)
                {
                    if (hvManager.isActive())
                    {
                        hvManager.showDBDataSearch();
                    }
                }
            }
            /////////////////////////////////////////////////////
            //else
            //{
            //    mi = ViewManagerHolder.getManager(typeof(GCMViewManager));
            //    if (mi != null)
            //    {
            //        GCMViewManager gcmvManager = (GCMViewManager)mi;
            //        if (gcmvManager != null)
            //        {
            //            if (gcmvManager.isActive())
            //            {
            //                gcmvManager.showDBDataSearch();
            //            }
            //        }
            //    }
            //}
            ///////////////////////////////////////////////////
        }
        public void frontDataSearch()
        {
            ManagerI mi = ViewManagerHolder.getManager(typeof(HomeViewManager));
            if (mi != null)
            {
                HomeViewManager hvManager = (HomeViewManager)mi;
                if (hvManager != null)
                {
                    if (hvManager.isActive())
                    {
                        hvManager.frontDataSearch();
                    }
                }
            }
            else
            {
                mi = ViewManagerHolder.getManager(typeof(GCMViewManager));
                if (mi != null)
                {
                    GCMViewManager gcmvManager = (GCMViewManager)mi;
                    if (gcmvManager != null)
                    {
                        if (gcmvManager.isActive())
                        {
                            /////////////////////////////
                            //gcmvManager.frontDataSearch();
                        }
                    }
                }
            }
        }
        public void frontSearchNext()
        {
            ManagerI mi = ViewManagerHolder.getManager(typeof(HomeViewManager));
            if (mi != null)
            {
                HomeViewManager hvManager = (HomeViewManager)mi;
                if (hvManager != null)
                {
                    if (hvManager.isActive())
                    {
                        hvManager.frontSearchNext();
                    }
                }
            }
            else
            {
                mi = ViewManagerHolder.getManager(typeof(GCMViewManager));
                if (mi != null)
                {
                    GCMViewManager gcmvManager = (GCMViewManager)mi;
                    if (gcmvManager != null)
                    {
                        if (gcmvManager.isActive())
                        {
                            //////////////////////////////
                            //gcmvManager.frontSearchNext();
                        }
                    }
                }
            }
        }
        public void frontSearchPrev()
        {
            ManagerI mi = ViewManagerHolder.getManager(typeof(HomeViewManager));
            if (mi != null)
            {
                HomeViewManager hvManager = (HomeViewManager)mi;
                if (hvManager != null)
                {
                    if (hvManager.isActive())
                    {
                        hvManager.frontSearchPrev();
                    }
                }
            }
            else
            {
                mi = ViewManagerHolder.getManager(typeof(GCMViewManager));
                if (mi != null)
                {
                    GCMViewManager gcmvManager = (GCMViewManager)mi;
                    if (gcmvManager != null)
                    {
                        if (gcmvManager.isActive())
                        {
                            //////////////////////////////
                            //gcmvManager.frontSearchPrev();
                        }
                    }
                }
            }
        }
    }
}
