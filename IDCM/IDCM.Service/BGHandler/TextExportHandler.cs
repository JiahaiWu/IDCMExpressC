using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using IDCM.Data.Base;
using IDCM.Service.Utils;

namespace IDCM.ServiceBL.Handle
{
    class TextExportHandler:AbsHandler
    {
        public TextExportHandler(string fpath, string cmdstr,int tcount,string spliter=" ")
        {
            this.textPath = System.IO.Path.GetFullPath(fpath);
            this.cmdstr = cmdstr;
            this.tcount = tcount;
            this.spliter = spliter;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res=false;
            TextExporter exporter = new TextExporter();
            res = exporter.exportText(textPath, cmdstr,tcount, spliter);
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
                MessageBox.Show("Export success. @filepath=" + textPath);
            }
        }

        private string spliter = null;
        private string textPath = null;
        private string cmdstr=null;
        private int tcount = 0;
    }
}
