using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using IDCM.ViewManager;
using IDCM.Service.Common;
using IDCM.Service.Common.Core;

/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.AppContext
{
    /// <summary>
    /// The class that handles the creation of the application windows
    /// 本类作为IDCM的窗口任务、后台任务、线程任务的创建管理类。
    /// 本实例只允许一次有效实例化，启动初始化窗口线程和心跳检测任务。
    /// 心跳检测任务会定期地判断当前进程是否活动任务为空，准备退出。
    /// @author JiahaiWu 2014-10
    /// 基础初始化操作包含三个部分：
    /// 1.工作空间独享验证checkWorkSpaceSingleton()
    /// 2.基本数据库初始化验证 startForm()
    /// 3.载入IDCM主窗口对象实例
    /// </summary>
    class IDCMAppContext : ApplicationContext
    {
        /// <summary>
        /// 初始化应用程序上下文环境，并触发程序主界面启动的事件过程。
        /// 说明：
        /// 1. 同一个工作空间下本实例进程要求排他性，如果已存在其他进程占用则执行退出过程。
        /// 2. IDCMFormManager为用户交互界面主控管理器实现，该实例对象保持唯一性；
        /// 3. 通过后台任务轮询监视器monitor来识别当前应用是否需要自动销毁进程资源，
        /// 对于长驻线程任务实现应当注册到LongTermHandleNoter实例中，以标志当前线程任务具有事务保护性。
        /// 由此实现的程序完全退出机制不受单一用户界面的关闭影响，强化事务完整性和数据操作完整性。
        /// 在诸项事务队列完成之后，才能有效退出。
        /// 4. 任务轮询监视器monitor的监听时间默认为2s。
        /// @author JiahaiWu 2014-12
        /// </summary>
        public IDCMAppContext()
        {
            if (!hasInited)
            {
                hasInited = true;
                // Handle the ApplicationExit event to know when the application is exiting.
                Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
                // Create main application form and active the initForm method
                servInvoker = new AsyncServInvoker();
                mainManger = new IDCMFormManger();
                mainManger.initForm(true);
                //bind Async Service to AsyncServInvoker
                servInvoker.OnDataPrepared += mainManger.OnDataPrepared;
                servInvoker.OnRetryQuickStartConnect += mainManger.OnRetryQuickStartConnect;
                //Run HandleInstanceMonitor
                handleMonitor.Interval = 1000;
                handleMonitor.Tick += OnHMHeartBreak;
                handleMonitor.Start();
                //Run MessageInstanceMonitor
                messageMonitor.Interval = 100;
                messageMonitor.Tick += OnMMHeartBreak;
                messageMonitor.Start();
            }
        }

        /// <summary>
        /// 应用程序常规退出触发事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnApplicationExit(object sender, EventArgs e)
        {
            log.Info("On IDCM application exiting.");
        }

        /// <summary>
        /// 后台任务轮询监视器的心跳检测事件处理
        /// 说明：
        /// 当长驻线程任务实例悉数完成或中断时，满足IDCM应用进程退出的条件时立即销毁进程资源操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHMHeartBreak(object sender, EventArgs e)
        {
#if DEBUG
            //Console.WriteLine("* Heart Break For checkForIdle()");
#endif
            if (DWorkMHub.checkForIdle())
            {
                handleMonitor.Stop();
                messageMonitor.Stop();
                ExitThread();
                this.Dispose();
            }
        }
        /// <summary>
        /// 异步消息轮询监视器的心跳检测事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMMHeartBreak(object sender, EventArgs e)
        {
#if DEBUG
            //Console.WriteLine("* Heart Break For checkAnsycMessage()");
#endif
            AsyncMessage[] msgs=DWorkMHub.getAsyncMessage();
            foreach(AsyncMessage msg in msgs)
            {
                servInvoker.dispatchMessage(msg);
            }
        }
        /// <summary>
        /// 用于共享成员实例获取当前用户交互界面主控管理器
        /// 注意：
        /// 该静态方法获取实例对象可能为空，调用方法需负责检测获取实例失败的特例情形。
        /// </summary>
        internal static IDCMFormManger MainManger
        {
            get
            {
                if (hasInited)
                    return IDCMAppContext.mainManger;
                else
                    return null;
            }
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// HandleInstanceMonitor
        /// </summary>
        private static System.Windows.Forms.Timer handleMonitor = new System.Windows.Forms.Timer();
        /// <summary>
        /// MessageInstanceMonitor
        /// </summary>
        private static System.Windows.Forms.Timer messageMonitor = new System.Windows.Forms.Timer();
        private static volatile bool hasInited = false;
        private static IDCMFormManger mainManger = null;
        private static AsyncServInvoker servInvoker = null;
    }
}
