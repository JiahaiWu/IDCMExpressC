using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Concurrent;
using IDCM.Service;

namespace IDCM.ViewManager
{
    internal class ViewManagerHolder
    {
        /// <summary>
        /// 获取子视图的用户交互界面管理器的实例对象
        /// 说明：
        /// 1.相对于主框架界面来说，子视图的用户交互界面管理器具有实例保持性，子视图初始化方法需支持重复调用请求。
        /// 2.子视图的用户交互界面管理器默认要求继承ManagerA（或RetainerA）抽象类，实现了顶层ManagerI接口类ManagerI中方法。
        /// </summary>
        /// <param name="manager">子视图的用户交互界面管理器对象类型</param>
        /// <returns>实现了ManagerI接口的子视图的用户交互界面管理器对象实例</returns>
        internal static ManagerI getManager(Type manager)
        {
            ManagerI obj = null;
            subManagers.TryGetValue(manager, out obj);
            if (obj == null || obj.isDisposed())
            {
#if DEBUG
                /////////////////////////////////////////////////////////////////////////////
                //当manager实现的是ManagerI的话断言失败
                //System.Diagnostics.Debug.Assert(manager.IsSubclassOf(typeof(ManagerA)));
                /////////////////////////////////////////////////////////////////////////////
                System.Diagnostics.Debug.Assert(manager.IsSubclassOf(typeof(ManagerA)) || manager.GetInterface("ManagerI")!=null);
#endif
                obj = Activator.CreateInstance(manager) as ManagerI;
                subManagers[manager] = obj;
                if (obj.initView(false) && IDCMFormManger.getInstance()!=null)
                {
                    obj.setMdiParent(IDCMFormManger.getInstance().MainForm);
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
        public static bool activeChildView(Type manager, bool activeFront = false)
        {
            if (typeof(ManagerI).IsAssignableFrom(manager))
            {
                ManagerI view = getManager(manager);
                if (activeFront)
                {
                    foreach (ManagerI ma in subManagers.Values)
                    {
                        ma.setMaxToNormal();
                    }
                    view.setToMaxmize(activeFront);
                }
                return view != null;
            }
            return false;
        }
        /// <summary>
        /// 释放缓存资源
        /// </summary>
        internal static void Dispose()
        {
            ConcurrentDictionary<Type, ManagerI> bakSubManagers = subManagers;
            subManagers = new ConcurrentDictionary<Type, ManagerI>();
            foreach (ManagerI ma in bakSubManagers.Values)
            {
                ma.dispose();
            }
            bakSubManagers.Clear();
        }
        //声明子视图的用户交互界面管理器的实例对象存储池
        private static ConcurrentDictionary<Type, ManagerI> subManagers = new ConcurrentDictionary<Type, ManagerI>();
    }
}
