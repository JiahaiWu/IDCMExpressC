using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Service.Common;
using IDCM.Data.Base;
using System.ComponentModel;

namespace IDCM.Service.BGHandler
{
    public class SignInHandler:AbsHandler
    {
        public SignInHandler(GCMSiteMHub gcmSite)
        {
            this.gcmSite = gcmSite;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res=gcmSite.connect();
            return new object[] { res };
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public void complete(BackgroundWorker worker, bool canceled, Exception error, List<Object> args)
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
        /// 后台任务执行过程中的状态反馈代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="progressPercentage"></param>
        /// <param name="args"></param>
        public void progressChanged(BackgroundWorker worker, int progressPercentage, List<Object> args)
        {
        }
        /// <summary>
        /// 后台任务执行结束后的串联执行任务队列。
        /// 单任务情形，返回结果为空即可。
        /// </summary>
        /// <returns></returns>
        public Queue<AbsHandler> cascadeHandlers()
        {
            return null;
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private GCMSiteMHub gcmSite = null;
    }
}
