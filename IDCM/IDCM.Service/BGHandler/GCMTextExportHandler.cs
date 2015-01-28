using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.DataTransfer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.BGHandler
{
    public class GCMTextExportHandler : AbsHandler
    {
        public GCMTextExportHandler(string xpath,DataGridView dgv,string spliter = "")
        {
            this.xpath = xpath;
            this.dgv = dgv;
            this.spliter = spliter;
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
            GCMTextExporter exporter = new GCMTextExporter();
            res = exporter.exportText(xpath, dgv, spliter);
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
        private string spliter;
        private DataGridView dgv;
    }
}
