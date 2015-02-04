using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.DataTransfer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.BGHandler
{
    public class GCMExcelExportHandler : AbsHandler
    {
        public GCMExcelExportHandler(string fpath, bool exportDetail, DataGridViewSelectedRowCollection selectedRows, GCMSiteMHub gcmSiteHolder)
        {
            this.fpath = fpath;
            this.exportDetail = exportDetail;
            this.selectedRows = selectedRows;
            this.gcmSiteHolder = gcmSiteHolder;
        }

        public GCMExcelExportHandler(string fpath, bool exportDetail, DataTable strainViewList, GCMSiteMHub gcmSiteHolder)
        {
            this.fpath = fpath;
            this.exportDetail = exportDetail;
            this.strainViewList = strainViewList;
            this.gcmSiteHolder = gcmSiteHolder;
        }

        public override object doWork(BackgroundWorker worker, bool cancel, List<object> args)
        {
            bool res = false;
            DWorkMHub.note(AsyncMessage.StartBackProgress);

            if (strainViewList == null && selectedRows == null) 
                return new object[] { false };

            GCMExcelExporter exporter = new GCMExcelExporter();
            if (strainViewList != null)
                res = exporter.exportExcel(fpath, strainViewList, exportDetail, gcmSiteHolder);
            else
                res = exporter.exportExcel(fpath, selectedRows, exportDetail, gcmSiteHolder);
            return new object[] { res };
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
                MessageBox.Show("Export success. @filepath=" + fpath);
            }
        }
        private bool exportDetail;
        private string fpath;
        private DataTable strainViewList;
        private DataGridViewSelectedRowCollection selectedRows;
        private GCMSiteMHub gcmSiteHolder;       
    }
}
