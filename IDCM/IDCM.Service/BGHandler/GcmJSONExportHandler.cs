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
        public GCMJSONExportHandler(string xpath,bool exportDetail,GCMSiteMHub gcmSiteHolder,DataGridViewSelectedRowCollection selectedRows)
        {
            this.xpath = xpath;
            this.exportDetail = exportDetail;            
            this.gcmSiteHolder = gcmSiteHolder;
            this.selectedRows = selectedRows;
        }
        public GCMJSONExportHandler(string xpath, bool exportDetail,DataTable strainViewList)
        {
            this.xpath = xpath;
            this.exportDetail = exportDetail;
            this.strainList = strainViewList;
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
           
            GCMJSONExporter exporter = new GCMJSONExporter();
            if (exportDetail)
            {
                res = exporter.exportJSONList(xpath, strainList);
            }
            else {
                res = exporter.exportJSONList(xpath,selectedRows, gcmSiteHolder);
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
        private DataTable strainList;
        private string xpath;
        private bool exportDetail;
        private GCMSiteMHub gcmSiteHolder;
        private DataGridViewSelectedRowCollection selectedRows;        
    }
}
