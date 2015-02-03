using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using IDCM.Data.Base;
using IDCM.Service.Utils;
using IDCM.Service.DataTransfer;
using IDCM.Service.Common;
using System.Windows.Forms;

namespace IDCM.Service.BGHandler
{
    public class XMLExportHandler:AbsHandler
    {
        public XMLExportHandler(DataSourceMHub datasource, string fpath, string cmdstr, int tcount)
        {
            this.textPath = System.IO.Path.GetFullPath(fpath);
            this.cmdstr = cmdstr;
            this.tcount = tcount;
            this.datasource = datasource;
        }

        public XMLExportHandler(DataSourceMHub datasource, string fpath, DataGridViewSelectedRowCollection selectedRows)
        {
            this.datasource = datasource;
            this.textPath = System.IO.Path.GetFullPath(fpath); ;
            this.selectedRows = selectedRows;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res=false;
            DWorkMHub.note(AsyncMessage.StartBackProgress);
            XMLExporter exporter = new XMLExporter();
            if (selectedRows != null)
                res = exporter.exportXML(datasource, textPath, selectedRows);
            else
                res = exporter.exportXML(datasource, textPath, cmdstr, tcount);
            return new object[] { res};
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override void complete(BackgroundWorker worker, bool canceled, Exception error, List<Object> args)
        {
            DWorkMHub.note(AsyncMessage.EndBackProgress);
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

        private string textPath = null;
        private string cmdstr=null;
        private int tcount = 0;
        private DataSourceMHub datasource=null;
        private DataGridViewSelectedRowCollection selectedRows;
    }
}
