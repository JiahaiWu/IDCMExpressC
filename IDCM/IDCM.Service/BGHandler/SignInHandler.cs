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
        private GCMSiteMHub gcmSite = null;
    }
}
