using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.Utils
{
    public class DGVAsyncUtil
    {
        public static void syncAddCol(DataGridView dgv,DataGridViewColumn dgvCol)
        {
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (dgvCol != null)
                    dgv.Columns.Add(dgvCol);
            }));
        }
        /// <summary>
        /// 为指定的DataGridView实现异步的数据行插入操作
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cellvals"></param>
        public static void syncAddRow(DataGridView dgv, string[] cellvals)
        {
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (cellvals != null)
                    dgv.Rows.Add(cellvals);
                else
                    dgv.Rows.Add();
            }));
        }
        /// <summary>
        /// 为指定的DataGridView指定行索引数处实现异步的数据行插入操作
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cellvals"></param>
        /// <param name="rowIdx"></param>
        public static void syncAddRow(DataGridView dgv, string[] cellvals,int rowIdx)
        {
            DataGridViewRow dgvr = new DataGridViewRow();
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (cellvals != null)
                {
                    dgvr.CreateCells(dgv, cellvals);
                }
                else
                {
                    dgvr.CreateCells(dgv);
                }
                dgv.Rows.InsertRange(rowIdx, dgvr);
            }));
        }
        /// <summary>
        /// 为指定的DataGridView实现异步的移除数据行操作
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cellvals"></param>
        public static void syncRemoveRow(DataGridView dgv, DataGridViewRow dgvRow)
        {
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                dgv.Rows.Remove(dgvRow);
            }));
        }
        public static void syncRemoveAllRow(DataGridView dgv)
        {
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                dgv.Rows.Clear();
            }));
        }
        public static void syncClearAll(DataGridView dgv)
        {
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                dgv.Rows.Clear();
                dgv.Columns.Clear();
            }));
        }

        /// <summary>
        /// 为指定的DataGridViewCell实现异步的错误提示更新操作
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cellvals"></param>
        public static void syncErrorText(DataGridView dgv, DataGridViewCell dgvCell, string errorText)
        {
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                dgvCell.ErrorText = errorText;
            }));
        }
        /// <summary>
        /// 为指定的DataGridViewCell实现异步的赋值更新操作
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cellvals"></param>
        public static void syncValue(DataGridView dgv, DataGridViewCell dgvCell, string value)
        {
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                dgvCell.Value = value;
            }));
        }
        /// <summary>
        /// 为指定的DataGridView取消编辑状态
        /// </summary>
        /// <param name="dgv"></param>
        public static void syncCancelEdit(DataGridView dgv)
        {
            ControlAsyncUtil.SyncInvoke(dgv, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                dgv.CancelEdit();
            }));
        }
    }
}
