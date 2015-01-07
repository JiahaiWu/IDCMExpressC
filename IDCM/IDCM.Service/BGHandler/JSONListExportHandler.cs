using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using IDCM.ServiceBL.CmdChannel;
using IDCM.ServiceBL.DataTransfer;

namespace IDCM.ServiceBL.Handle
{
    class JSONListExportHandler:AbsHandler
    {
        public JSONListExportHandler(string fpath,  string cmdstr,int tcount)
        {
            this.xlsPath = System.IO.Path.GetFullPath(fpath);
            this.cmdstr = cmdstr;
            this.tcount = tcount;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res=false;
            JSONListExporter exporter = new JSONListExporter();
            res = exporter.exportJSONList(xlsPath, cmdstr,tcount);
            return new object[] { res};
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
                MessageBox.Show("ERROR::" + error.Message + "\n" + error.StackTrace);
                return;
            }
            else
            {
                MessageBox.Show("Export success. @filepath="+xlsPath);
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
            //if (progressPercentage > 0)
            //    reportProgress(worker, progressPercentage);
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
        private string xlsPath = null;
        private  string cmdstr;
        private int tcount;
    }
}
