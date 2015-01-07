using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using IDCM.Data.Base;

namespace IDCM.ServiceBL.Handle
{
    class ExcelExportHandler:AbsHandler
    {
        public ExcelExportHandler(string fpath,  string cmdstr,int tcount)
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
            ExcelExporter exporter = new ExcelExporter();
            res = exporter.exportExcel(xlsPath,cmdstr,tcount);
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

        private string xlsPath = null;
        private  string cmdstr;
        private int tcount;
    }
}
