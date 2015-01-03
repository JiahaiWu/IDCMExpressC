using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.Forms;
using IDCM.Service.Common;
using IDCM.Service.Common.Core.ServBuf;

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
            subManagers = new Dictionary<Type, ManagerI>(); //初始化子视图的用户交互界面管理器的实例对象存储池
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
            subManagers = null;
        }
        #endregion
        #region 实例对象保持部分
        //声明释放主窗口界面实例
        internal IDCMForm mainForm = null;
        //声明子视图的用户交互界面管理器的实例对象存储池
        internal Dictionary<Type, ManagerI> subManagers = null;
        #endregion

#region 接管视图组件的关键的事件处理区
        /// <summary>
        /// IDCMForm主界面第一次显示后，启动默认的数据页面展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDCMForm_Shown(object sender, EventArgs e)
        {
            //启动欢迎页面
            activeChildView(typeof(StartRetainer), true);
            //activeChildViewAwait(typeof(HomeViewManager), true);
            //activeChildView(typeof(AuthenticationRetainer), false);
        }
        /// <summary>
        /// 异步消息事务分发处理
        /// </summary>
        /// <param name="msg"></param>
        internal void dispatchMessage(AsyncMessage msg)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(msg !=null);
#endif
            if(msg.Equals(AsyncMessage.Prepared))
            {
            }
        }

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
        /// <summary>
        /// 获取子视图的用户交互界面管理器的实例对象
        /// 说明：
        /// 1.相对于主框架界面来说，子视图的用户交互界面管理器具有实例保持性，子视图初始化方法需支持重复调用请求。
        /// 2.子视图的用户交互界面管理器默认要求继承ManagerA（或RetainerA）抽象类，实现了顶层ManagerI接口类ManagerI中方法。
        /// </summary>
        /// <param name="manager">子视图的用户交互界面管理器对象类型</param>
        /// <returns>实现了ManagerI接口的子视图的用户交互界面管理器对象实例</returns>
        internal ManagerI getManager(Type manager)
        {
            ManagerI obj = null;
            subManagers.TryGetValue(manager, out obj);
            if (obj == null || obj.isDisposed())
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(manager is ManagerI);
#endif
                obj = Activator.CreateInstance(manager) as ManagerI;
                subManagers[manager] = obj;
                if (obj.initView(false))
                {
                    obj.setMdiParent(this.mainForm);
                }
            }
            return obj;
        }
        /// <summary>
        /// 激活直属视图实例，及其必要的窗口显示操作。
        /// 说明：
        /// 1.该方法封装getManager(Type manger)方法之上，根据目标子视图的用户交互界面管理器对象类型获取对象实例
        /// 2.该方法尽可能激活子视图界面的前端显示的有效调用，但不保证资源释放期调用时的有效性。
        /// 注意：
        /// 1.当激活前端显示的参数有效，既有的最大化显示的前端视图会因此调用还原默认窗口表现。
        /// </summary>
        /// <param name="manager">子视图的用户交互界面管理器对象类型</param>
        /// <param name="activeFront">是否激活前端显示</param>
        /// <returns>激活子视图界面的前端显示调用成功与否</returns>
        public bool activeChildView(Type manager, bool activeFront = false)
        {
            if (typeof(ManagerI).IsAssignableFrom(manager))
            {
                ManagerI view = getManager(manager);
                if (activeFront)
                {
                    if (manager.IsSubclassOf(typeof(RetainerA)))
                    {
                        view.initView(true);
                    }
                    else
                    {
                        foreach (ManagerI ma in subManagers.Values)
                        {
                            ma.setMaxToNormal();
                        }
                        view.setToMaxmize(activeFront);
                    }
                }
                return view != null;
            }
            return false;
        }
        /// <summary>
        /// 显示等待提示页面，并隐式地激活直属视图实例及其必要的窗口显示操作。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="activeFront"></param>
        /// <returns></returns>
        public void activeChildViewAwait(Type manager, bool activeFront = false)
        {
            Form startForm = new StartForm();
            startForm.Show();
            startForm.Update();
            bool res = activeChildView(manager, activeFront);
            startForm.Close();
            startForm.Dispose();
        }
        /// <summary>
        /// 关闭当前工作空间，仅保留主框架窗口
        /// </summary>
        /// <returns></returns>
        public bool closeWorkSpaceHolder()
        {
            foreach (ManagerI ma in subManagers.Values)
            {
                ma.dispose();
            }
            subManagers.Clear();
            DataSourceHolder.close();
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
        public void showDBDataSearch()
        {
            HomeViewManager hvManager = (HomeViewManager)getManager(typeof(HomeViewManager));
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    hvManager.showDBDataSearch();
                }
            }
        }
        public void frontDataSearch()
        {
            HomeViewManager hvManager = (HomeViewManager)getManager(typeof(HomeViewManager));
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    hvManager.frontDataSearch();
                }
            }
        }
        public void frontSearchNext()
        {
            HomeViewManager hvManager = (HomeViewManager)getManager(typeof(HomeViewManager));
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    hvManager.frontSearchNext();
                }
            }
        }
        public void frontSearchPrev()
        {
            HomeViewManager hvManager = (HomeViewManager)getManager(typeof(HomeViewManager));
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    hvManager.frontSearchPrev();
                }
            }
        }
    }
}
