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
    public class GCMTextExportHandler : AbsHandler
    {
        public GCMTextExportHandler(string xpath, bool exportDetail, DataGridViewSelectedRowCollection selectedRows, string spliter, GCMSiteMHub gcmSiteHolder = null)
        {
            this.xpath = xpath;
            this.exportDetail = exportDetail;
            this.selectedRows = selectedRows;
            this.spliter = spliter;
            this.gcmSiteHolder = gcmSiteHolder;
            
        }
        public GCMTextExportHandler(string xpath, bool exportDetail, DataTable strainViewList,string spliter, GCMSiteMHub gcmSiteHolder = null)
        {
            this.xpath = xpath;
            this.exportDetail = exportDetail;
            this.strainList = strainViewList;
            this.spliter = spliter;
            this.gcmSiteHolder = gcmSiteHolder;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res = false;
            DWorkMHub.note(AsyncMessage.StartBackProgress);
            if (strainList == null && selectedRows == null) return new object[] { false };

            GCMTextExporter exporter = new GCMTextExporter();
            if (strainList != null)
            {
                res = exporter.exportText(xpath, strainList, exportDetail, spliter, gcmSiteHolder);
            }
            else
            {
                res = exporter.exportText(xpath, selectedRows, exportDetail, spliter,gcmSiteHolder);
            }
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
                MessageBox.Show("Export success. @filepath=" + xpath);
            }
        }
        private string xpath;
        private DataTable strainList;
        private DataGridViewSelectedRowCollection selectedRows;
        private bool exportDetail;
        private string spliter;
        GCMSiteMHub gcmSiteHolder;
    }
}
