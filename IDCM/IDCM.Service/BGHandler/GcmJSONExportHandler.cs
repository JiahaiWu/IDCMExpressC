using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.DataTransfer;
using IDCM.Service.UIM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.BGHandler
{
    public class GCMJSONExportHandler : AbsHandler
    {
        public GCMJSONExportHandler(string xpath, bool exportDetail, DataGridViewSelectedRowCollection selectedRows, GCMSiteMHub gcmSiteHolder = null)
        {
            this.xpath = xpath;
            this.exportDetail = exportDetail;            
            this.gcmSiteHolder = gcmSiteHolder;
            this.selectedRows = selectedRows;
        }
        public GCMJSONExportHandler(string xpath, bool exportDetail, DataTable strainViewList, GCMSiteMHub gcmSiteHolder = null)
        {
            this.xpath = xpath;
            this.exportDetail = exportDetail;
            this.strainList = strainViewList;
            this.gcmSiteHolder = gcmSiteHolder;
        }


        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// 说明：
        /// 1：doWork有两种情况
        ///     A：用户有选中行，以选中行为准，导出选中行
        ///     B：用户没有选中行，以全部记录为准，导出全部记录
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res = false;
            DWorkMHub.note(AsyncMessage.StartBackProgress);

            if (strainList == null && selectedRows == null)
                return new object[] { false };

            GCMJSONExporter exporter = new GCMJSONExporter();
            if (strainList != null)
                res = exporter.exportJSONList(xpath, strainList, exportDetail, gcmSiteHolder);
            else
                res = exporter.exportJSONList(xpath, selectedRows, exportDetail,gcmSiteHolder);
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
        private DataTable strainList;
        private string xpath;
        private bool exportDetail;
        private GCMSiteMHub gcmSiteHolder;
        private DataGridViewSelectedRowCollection selectedRows;        
    }
}
