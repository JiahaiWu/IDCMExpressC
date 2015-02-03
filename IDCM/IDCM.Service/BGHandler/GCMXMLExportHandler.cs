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
    public class GCMXMLExportHandler : AbsHandler
    {
        public GCMXMLExportHandler(string xpath, bool exportDetail, DataTable strainViewList, GCMSiteMHub gcmSiteHolder = null)
        {
            this.xpath = xpath;
            this.exportDetail = exportDetail;
            this.strainViewList = strainViewList;
            this.gcmSiteHolder = gcmSiteHolder;
        }

        public GCMXMLExportHandler(string xpath, bool exportDetail, DataGridViewSelectedRowCollection selectedRows, GCMSiteMHub gcmSiteHolder =null)
        {
            this.xpath = xpath;
            this.exportDetail = exportDetail;
            this.selectedRows = selectedRows;
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
            if (strainViewList == null) 
                return new object[] { false };

            GCMXMLExporter exporter = new GCMXMLExporter();
            if (strainViewList != null)
                exporter.exportXML(xpath, strainViewList, exportDetail, gcmSiteHolder);
            else
                exporter.exportXML(xpath, selectedRows, exportDetail, gcmSiteHolder);
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
        private bool exportDetail;
        private DataTable strainViewList;
        private GCMSiteMHub gcmSiteHolder;
        private DataGridViewSelectedRowCollection selectedRows;
    }
}
