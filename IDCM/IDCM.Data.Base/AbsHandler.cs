using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IDCM.Data.Base
{
    public abstract class AbsHandler
    {
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public abstract Object doWork(BackgroundWorker worker, bool cancel, List<Object> args);
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public virtual void complete(BackgroundWorker worker, bool canceled, Exception error, List<Object> args)
        {
            if (canceled)
                return;
            if (error != null)
            {
                log.Error(error);
                return;
            }
        }

        /// <summary>
        /// 后台任务执行过程中的状态反馈代码段。
        /// 如dowork的执行部分无状态反馈过程，本方法仅需空实现即可。
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="progressPercentage"></param>
        /// <param name="args"></param>
        public virtual void progressChanged(BackgroundWorker worker, int progressPercentage, List<Object> args)
        {
        }

        /// <summary>
        /// 后台任务执行结束后的串联执行任务队列。
        /// 单任务情形，返回结果为空即可。
        /// </summary>
        /// <returns></returns>
        public virtual Queue<AbsHandler> cascadeHandlers()
        {
            return nextHandlers;
        }
        /// <summary>
        /// 添加后台任务执行结束后的串联执行任务
        /// </summary>
        /// <param name="nextHandler"></param>
        protected virtual void addHandler(AbsHandler nextHandler)
        {
            if (nextHandler == null)
                return;
            if (nextHandlers == null)
                nextHandlers = new Queue<AbsHandler>(0);
            nextHandlers.Enqueue(nextHandler);
        }

        private Queue<AbsHandler> nextHandlers = null;
        protected static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
