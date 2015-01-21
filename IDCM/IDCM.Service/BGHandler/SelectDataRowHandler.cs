using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.Utils;
using IDCM.Data.Base.Utils;

namespace IDCM.Service.BGHandler
{
    public class SelectDataRowHandler:AbsHandler
    {
        public SelectDataRowHandler(DataSourceMHub datasource, DataGridView itemDGV, TabControl attachTC)
        {
            this.itemDGV = itemDGV;
            this.attachTC = attachTC;
            this.datasource = datasource;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res = false;
            List<string> viewAttrs = LocalRecordMHub.getViewAttrs(datasource);
            if (itemDGV.Rows.Count > 0)
                selectViewRecord(itemDGV.Rows[0], viewAttrs);
            else
                showReferences(viewAttrs, null);
            return new object[] { res};
        }
        /// <summary>
        /// 根据指定的数据行更新显示附加的属性信息
        /// </summary>
        /// <param name="viewAttrs"></param>
        /// <param name="dr"></param>
        /// dr为null则等效于清空表单操作
        protected void showReferences(List<string> viewAttrs, DataRow dr = null)
        {
            TabPage tabPage = attachTC.TabPages["references"];
            if (dr != null)
            {
                ControlAsyncUtil.SyncSetText(tabPage.Controls["ctd_rid_Label"], dr[CTDRecordA.CTD_RID].ToString());
                //(tabPage.Controls["ctd_rid_Label"] as Label).Text = dr[CTDRecordDA.CTD_RID].ToString();
            }
            foreach (Control ctl in tabPage.Controls)
            {
                if (ctl is Panel)
                {
                    int idx = Convert.ToInt32(ctl.Name.Substring("referPanel_".Length));
                    string attr = CVNameConverter.toViewName(viewAttrs[idx]);
                    Control ictl = ctl.Controls[attr];
                    if (ictl != null)
                    {
                        if (ictl is TextBox)
                        {
                            //(ictl as TextBox).Text = dr == null ? "" : dr[attr].ToString();
                            ControlAsyncUtil.SyncSetText(ictl, dr == null ? "" : dr[attr].ToString());
                        }
                        else if (ictl is ComboBox)
                        {
                            //(ictl as ComboBox).FormatString = dr == null ? "" : dr[attr].ToString();
                            ControlAsyncUtil.SyncSetText(ictl, dr == null ? "" : dr[attr].ToString());
                        }
                        else if (ictl is DateTimePicker)
                        {
                            (ictl as DateTimePicker).CustomFormat = dr == null ? "" : dr[attr].ToString();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 根据指定的索引位序更新显示附加的属性信息
        /// </summary>
        /// <param name="dgvr"></param>
        /// <param name="tc"></param>
        protected void selectViewRecord(DataGridViewRow dgvr, List<string> viewAttrs)
        {
            int rIdx = dgvr.DataGridView.Columns[CTDRecordA.CTD_RID.ToString()].Index;
            if (dgvr.Cells.Count > rIdx && dgvr.Cells[rIdx].FormattedValue.ToString().Length>0)
            {
                long rid= Convert.ToInt64(dgvr.Cells[rIdx].FormattedValue.ToString());
                ///////((HomeViewManager)HomeViewManager.getInstance()).CURRENT_RID = rid;
                DataTable table = LocalRecordMHub.queryCTDRecord(datasource, null, Convert.ToString(rid));
                if (table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    showReferences(viewAttrs, dr);
                }
            }
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override void complete(BackgroundWorker worker, bool canceled, Exception error, List<Object> args)
        {
            if (canceled)
                return;
            if (error != null)
            {
                MessageBox.Show("ERROR::" + error.Message + "\n" + error.StackTrace);
                return;
            }
        }

        private DataGridView itemDGV;
        private DataSourceMHub datasource;
        private TabControl attachTC;
    }
}
