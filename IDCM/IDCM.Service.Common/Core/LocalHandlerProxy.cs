using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using IDCM.Data.Base;

namespace IDCM.Service.Common.Core
{
    /// <summary>
    /// 后台处理方法的任务包装代理实现
    /// </summary>
    class LocalHandlerProxy
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="handler"></param>
        public LocalHandlerProxy(AbsHandler _handler, Queue<AbsHandler> cascadeHandlers = null)
        {
            this.handler = _handler;
            this.cascadeHandlers = cascadeHandlers;
            doWorkTime = DateTime.Now;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            doWorkTime = DateTime.Now;
            Thread.CurrentThread.Name = handler.GetType().Name;
            ////////////////////////
            List<Object> args = new List<Object>();
            if (e.Argument is Object[])
            {
                args.AddRange((Object[])e.Argument);
            }
            else
                args.Add(e.Argument);
            BackgroundWorker worker = (BackgroundWorker)sender;
            worker.ReportProgress(0);
            e.Result = handler.doWork(worker, e.Cancel, args);
            worker.ReportProgress(100);
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Object> args = new List<Object>();
            if (e.Result is Object[])
            {
                args.AddRange((Object[])e.Result);
            }
            else
                args.Add(e.Result);
            BackgroundWorker worker = (BackgroundWorker)sender;
            //////////////////////////////////////////
            handler.complete(worker, e.Cancelled, e.Error, args);
            //////////////////////////////
            BGWorkerInvoker.removeWorker(worker);
        }
        /// <summary>
        /// 后台任务执行结束后的串联执行任务队列的代理实现代码段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void worker_cascadeProcess(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
                return;
            //插队串联部分
            if (handler.cascadeHandlers() != null)
            {
                Queue<AbsHandler> nextHandlers = this.cascadeHandlers;
                if (nextHandlers == null)
                    this.cascadeHandlers = handler.cascadeHandlers();
                if (nextHandlers != null)
                {
                    foreach (AbsHandler hand in nextHandlers)
                    {
                        cascadeHandlers.Enqueue(hand);
                    }
                }
            }
            //队头出列，提交执行任务池
            if (this.cascadeHandlers != null && this.cascadeHandlers.Count > 0)
            {
                AbsHandler nextHandler = cascadeHandlers.Dequeue();
                List<Object> args = new List<Object>();
                if (e.Result is Object[])
                {
                    args.AddRange((Object[])e.Result);
                }
                else
                    args.Add(e.Result);
                BGWorkerInvoker.pushHandler(nextHandler, args, cascadeHandlers);
            }
        }
        /// <summary>
        /// 后台任务执行过程中的状态反馈代码段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            List<Object> args = new List<Object>();
            if (e.UserState is Object[])
            {
                args.AddRange((Object[])e.UserState);
            }
            else
                args.Add(e.UserState);
            BackgroundWorker worker = (BackgroundWorker)sender;
            handler.progressChanged(worker, e.ProgressPercentage, args);
        }

        /// <summary>
        /// 获取任务代理包装的元处理器实例对象
        /// </summary>
        /// <returns></returns>
        internal AbsHandler getHandler()
        {
            return handler;
        }
        /// <summary>
        /// 获取任务代理包装的元处理器实例对象名称
        /// </summary>
        /// <returns></returns>
        public string getProxyName()
        {
            return handler.GetType().Name;
        }
        /// <summary>
        /// 返回运行累计时长
        /// 说明：
        /// 1.以毫秒为单位
        /// 2.计算数值不排除异步运行完成后的存活时间
        /// </summary>
        /// <returns></returns>
        public long getRunningTime()
        {
            TimeSpan span = DateTime.Now - doWorkTime;
            return span.Milliseconds;
        }
        /// <summary>
        /// 任务代理包装的元处理器实例对象
        /// </summary>
        private AbsHandler handler;
        /// <summary>
        /// 等待执行的串联执行任务队列
        /// </summary>
        private Queue<AbsHandler> cascadeHandlers = null;
        /// <summary>
        /// 用于标记任务执行的起始时间
        /// </summary>
        private DateTime doWorkTime;
    }
}
