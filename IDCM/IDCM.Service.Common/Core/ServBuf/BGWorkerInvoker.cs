using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IDCM.Service.Common.Core.ServBuf
{
    /// <summary>
    /// 本地后台运行服务池，为后台执行线程
    /// </summary>
    class BGWorkerInvoker
    {
        /// <summary>
        /// 加载后台执行任务袋，并立即提交异步执行操作
        /// </summary>
        /// <param name="handler"></param>
        public static void pushHandler(AbsHandler handler, Object args = null, Queue<AbsHandler> cascadeHandlers = null)
        {
            if (handler == null)
                return;
            LocalHandlerProxy proxy = new LocalHandlerProxy(handler, cascadeHandlers);
            BackgroundWorker bgworker = new BackgroundWorker();
            bgworker.WorkerReportsProgress = true;
            bgworker.WorkerSupportsCancellation = true;
            //bgworker.DoWork += proxy.worker_DoWork;
            //bgworker.ProgressChanged += proxy.worker_ProgressChanged;
            //bgworker.RunWorkerCompleted += proxy.worker_RunWorkerCompleted;
            bgworker.DoWork += new DoWorkEventHandler(proxy.worker_DoWork);
            bgworker.ProgressChanged += new ProgressChangedEventHandler(proxy.worker_ProgressChanged);
            bgworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(proxy.worker_RunWorkerCompleted);
            bgworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(proxy.worker_cascadeProcess);
            LongTermHandleNoter.note(bgworker, proxy);
            if (args == null)
                bgworker.RunWorkerAsync();
            else
                bgworker.RunWorkerAsync(args);
        }
        /// <summary>
        /// 移除特定的BackgroundWorker实例
        /// </summary>
        /// <param name="worker"></param>
        public static void removeWorker(BackgroundWorker worker)
        {
            if (worker == null)
                return;
            LongTermHandleNoter.removeBackgroundworker(worker);
        }
        /// <summary>
        /// 按照特征类型请求中止后台任务执行请求操作
        /// </summary>
        /// <param name="handlerType"></param>
        public static void abortHandlerByType(Type handlerType)
        {
            KeyValuePair<BackgroundWorker, LocalHandlerProxy>[] worker = LongTermHandleNoter.getActiveWorkers();
            foreach (KeyValuePair<BackgroundWorker, LocalHandlerProxy> kvpair in worker)
            {
                if (kvpair.Value.getHandler().GetType().Equals(handlerType))
                {
                    BackgroundWorker targetWorker = kvpair.Key;
                    if (targetWorker!= null) 
                    {
                        if(targetWorker.WorkerSupportsCancellation)
                        {
                            if(!targetWorker.CancellationPending)
                                targetWorker.CancelAsync();
                            LongTermHandleNoter.removeBackgroundworker(targetWorker);
                        }
                    }
                }
            }
        }
    }
}
