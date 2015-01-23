using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Test
{
    /// <summary>
    /// 测试数据定制表单显示风格的原型实现
    /// 说明：
    /// 1.设置一个checkbox列，用于标记记录行状态
    /// 2.设置右键菜单支持剪贴板复制粘贴的功能
    /// </summary>
    public partial class DGVSelectAndPaste : Form
    {
        int TotalCheckBoxes = 0;
        int TotalCheckedCheckBoxes = 0;
        CheckBox HeaderCheckBox = null;
        bool IsHeaderCheckBoxClicked = false;

        private BindingSource oBS = new BindingSource();


        public DGVSelectAndPaste()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载数据源，绑定事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            AddHeaderCheckBox();

            HeaderCheckBox.KeyUp += new KeyEventHandler(HeaderCheckBox_KeyUp);
            HeaderCheckBox.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);
            dgData.CellValueChanged += new DataGridViewCellEventHandler(dgvSelectAll_CellValueChanged);
            dgData.CurrentCellDirtyStateChanged += new EventHandler(dgvSelectAll_CurrentCellDirtyStateChanged);
            dgData.CellPainting += new DataGridViewCellPaintingEventHandler(dgvSelectAll_CellPainting);

            BindGridView();

            dgData.ContextMenuStrip = this.contextMenuStrip1;
            btnGetData.Enabled = false;
        }

        /// <summary>
        /// Add the checkBox into the datagridview
        /// </summary>
        private void AddHeaderCheckBox()
        {
            HeaderCheckBox = new CheckBox();

            HeaderCheckBox.Size = new Size(15, 15);

            //Add the CheckBox into the DataGridView
            this.dgData.Controls.Add(HeaderCheckBox);
        }
        /// <summary>
        /// 获取数据源，添加默认的数据记录
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataSource()
        {
            DataTable dTable = new DataTable();

            DataRow dRow = null;
            DateTime dTime;
            Random rnd = new Random();

            dTable.Columns.Add("IsChecked", System.Type.GetType("System.Boolean"));
            dTable.Columns.Add("RandomNo");
            dTable.Columns.Add("Date");
            dTable.Columns.Add("Time");
            dTable.Columns.Add("Annoation");

            for (int n = 0; n < 10; ++n)
            {
                dRow = dTable.NewRow();
                dTime = DateTime.Now;

                dRow["IsChecked"] = "false";
                dRow["RandomNo"] = rnd.NextDouble();
                dRow["Date"] = dTime.ToString("MM/dd/yyyy");
                dRow["Time"] = dTime.ToString("hh:mm:ss tt");

                dTable.Rows.Add(dRow);
                dTable.AcceptChanges();
            }

            return dTable;
        }
        /// <summary>
        /// 绑定数据源操作
        /// </summary>
        private void BindGridView()
        {
            oBS.DataSource = GetDataSource();
            dgData.DataSource = oBS;

            TotalCheckBoxes = dgData.RowCount;
            TotalCheckedCheckBoxes = 0;

            dgData.Columns[0].HeaderText = "";

            //readonly the columns that cannot be edited
            dgData.Columns[1].ReadOnly = true;
            dgData.Columns[2].ReadOnly = true;
            dgData.Columns[3].ReadOnly = true;
        }
        /// <summary>
        /// 获取记录改变消息事件，按单元格识别判断如何处理事件响应
        /// 说明：
        /// 1.如果单击Checkbox那么设定Checkbox选中操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSelectAll_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0 && !IsHeaderCheckBoxClicked)
                RowCheckBoxClick((DataGridViewCheckBoxCell)dgData[e.ColumnIndex, e.RowIndex]);
        }
        /// <summary>
        /// 选中一行
        /// </summary>
        /// <param name="RCheckBox"></param>
        private void RowCheckBoxClick(DataGridViewCheckBoxCell RCheckBox)
        {
            if (RCheckBox != null)
            {
                //Modifiy Counter;            
                if ((bool)RCheckBox.Value && TotalCheckedCheckBoxes < TotalCheckBoxes)
                    TotalCheckedCheckBoxes++;
                else if (TotalCheckedCheckBoxes > 0)
                    TotalCheckedCheckBoxes--;

                //Change state of the header CheckBox.
                if (TotalCheckedCheckBoxes < TotalCheckBoxes)
                    HeaderCheckBox.Checked = false;
                else if (TotalCheckedCheckBoxes == TotalCheckBoxes)
                    HeaderCheckBox.Checked = true;
            }
        }
        /// <summary>
        /// 当单元格状态改变时，事件触发机制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSelectAll_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgData.CurrentCell is DataGridViewCheckBoxCell)
                dgData.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
        /// <summary>
        /// 当Checkbox被点击时的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            HeaderCheckBoxClick((CheckBox)sender);
        }
        /// <summary>
        /// 获取空格键输入的键盘弹起事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderCheckBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                HeaderCheckBoxClick((CheckBox)sender);
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="HCheckBox"></param>
        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            IsHeaderCheckBoxClicked = true;

            foreach (DataGridViewRow Row in dgData.Rows)
                ((DataGridViewCheckBoxCell)Row.Cells["IsChecked"]).Value = HCheckBox.Checked;

            dgData.RefreshEdit();

            TotalCheckedCheckBoxes = HCheckBox.Checked ? TotalCheckBoxes : 0;

            IsHeaderCheckBoxClicked = false;
        }
        /// <summary>
        /// 当单元格需要绘制时，捕获事件重绘Checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSelectAll_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
                ResetHeaderCheckBoxLocation(e.ColumnIndex, e.RowIndex);
        }
        /// <summary>
        /// 重置Checkbox绘制图层的显示位置
        /// </summary>
        /// <param name="ColumnIndex"></param>
        /// <param name="RowIndex"></param>
        private void ResetHeaderCheckBoxLocation(int ColumnIndex, int RowIndex)
        {
            //Get the column header cell bounds
            Rectangle oRectangle = this.dgData.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);

            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBox.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBox.Height) / 2 + 1;

            //Change the location of the CheckBox to make it stay on the header
            HeaderCheckBox.Location = oPoint;
        }
        /// <summary>
        /// 保存按钮触发事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable oTable = (DataTable)oBS.DataSource;
            DataView oDV = new DataView(oTable, "", "", DataViewRowState.ModifiedCurrent);
            foreach (DataRowView oRow in oDV)
            {
                //write each row to the database as all of them have changed
            }
        }
        /// <summary>
        /// 关闭按钮事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 右键菜单中复制请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyClipboard();
        }

        private void CopyClipboard()
        {
            DataObject d = dgData.GetClipboardContent();
            Clipboard.SetDataObject(d);
        }

        private void pasteCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteClipboard();
        }

        /// <summary>
        /// This will be moved to the util class so it can service any paste into a DGV
        /// </summary>
        private void PasteClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int iFail = 0, iRow = dgData.CurrentCell.RowIndex;
                int iCol = dgData.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                foreach (string line in lines)
                {
                    if (iRow < dgData.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this.dgData.ColumnCount)
                            {
                                oCell = dgData[iCol + i, iRow];
                                if (!oCell.ReadOnly)
                                {
                                    if (oCell.Value == null || oCell.Value.ToString() != sCells[i])
                                    {
                                        oCell.Value = Convert.ChangeType(sCells[i], oCell.ValueType);
                                        oCell.Style.BackColor = Color.Tomato;
                                    }
                                    else
                                        iFail++;//only traps a fail if the data has changed and you are pasting into a read only cell
                                }
                            }
                            else
                            { break; }
                        }
                        iRow++;
                    }
                    else
                    { break; }
                    if (iFail > 0)
                        MessageBox.Show(string.Format("{0} updates failed due to read only column setting", iFail));
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }
        /// <summary>
        /// 绑定剪贴板复制粘贴的快捷键处理Ctrl+C Ctrl+V Shift+Delete 及 Shift+Insert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgData_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.KeyCode == Keys.C) || (e.Shift && e.KeyCode == Keys.Delete))
            {
                CopyClipboard();
            }
            if ((e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert))
            {
                PasteClipboard();
            }

        }
    }
}
