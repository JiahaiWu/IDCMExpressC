using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using IDCM.Data.Base;
using IDCM.Service.Utils;
using IDCM.Service.DataTransfer;
using IDCM.Service.Common;

namespace IDCM.Service.BGHandler
{
    public class JSONListExportHandler:AbsHandler
    {
        public JSONListExportHandler(DataSourceMHub datasource, string fpath, string cmdstr, int tcount)
        {
            this.xlsPath = System.IO.Path.GetFullPath(fpath);
            this.cmdstr = cmdstr;
            this.tcount = tcount;
            this.datasource = datasource;
        }

        public JSONListExportHandler(DataSourceMHub datasource, string fpath, DataGridViewSelectedRowCollection selectedRows)
        {
            // TODO: Complete member initialization
            this.datasource = datasource;
            this.xlsPath = fpath;
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
            JSONListExporter exporter = new JSONListExporter();
            if (selectedRows != null && selectedRows.Count > 0)
                res = exporter.exportJSONList(datasource, xlsPath, selectedRows);
            else
                res = exporter.exportJSONList(datasource, xlsPath, cmdstr, tcount);
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
                MessageBox.Show("Export success. @filepath="+xlsPath);
            }
        }
        private string xlsPath = null;
        private  string cmdstr;
        private int tcount;
        private DataSourceMHub datasource = null;
        private DataGridViewSelectedRowCollection selectedRows;
    }
}
