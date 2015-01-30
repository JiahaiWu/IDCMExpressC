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
    public class GCMExcelExportHandler : AbsHandler
    {
        public GCMExcelExportHandler(string path, DataGridView dgv, bool exportStrainTree)
        {
            this.path = path;
            this.dgv = dgv;
            this.exportStrainTree = exportStrainTree;
        }

        public override object doWork(System.ComponentModel.BackgroundWorker worker, bool cancel, List<object> args)
        {
            bool res = false;
            DWorkMHub.note(AsyncMessage.StartBackProgress);
            GCMExcelExporter exporter = new GCMExcelExporter();
            res = exporter.exportExcel(path, dgv);
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
                MessageBox.Show("Export success. @filepath=" + path);
            }
        }

        private string path;
        private DataGridView dgv;
        private bool exportStrainTree;
    }
}
