using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using System.ComponentModel;
using IDCM.Service.Common;
using System.Windows.Forms;
using IDCM.Service.Utils;

namespace IDCM.Service.BGHandler
{
    public class UpdateLocalLinkTagHandler : AbsHandler
    {
        private GCMSiteMHub gcmHolder;
        private DataGridView dataGridView;
        private string keyName;

        public UpdateLocalLinkTagHandler(GCMSiteMHub gcmHolder, DataGridView dataGridView, string keyName)
        {
            this.gcmHolder = gcmHolder;
            this.dataGridView = dataGridView;
            this.keyName = keyName;
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
            Dictionary<string, int> loadedNoter =gcmHolder.getLoadedNoter();
            foreach (DataGridViewRow dgvr in dataGridView.Rows)
            {
                if (dgvr.IsNewRow || dgvr.Index < 0)
                    continue;
                DataGridViewCell dgvc =dgvr.Cells[keyName];
                if (dgvc != null && loadedNoter.ContainsKey(DGVUtil.getCellValue(dgvc)))
                {
                    dgvr.Cells[0].Value = true;
                }
                else
                {
                    dgvr.Cells[0].Value = false;
                }
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
                log.Error(error);
                return;
            }
        }
    }
}
