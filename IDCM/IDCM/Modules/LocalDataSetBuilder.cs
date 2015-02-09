using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Data.Base.Utils;
using IDCM.Service.Utils;
using IDCM.Core;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using IDCM.Forms;
using System.Configuration;
using System.Xml;
using IDCM.Service;

namespace IDCM.Modules
{
    class LocalDataSetBuilder
    {
        #region 构造&析构
        /// <summary>
        /// 初始化个人资源目录树
        /// </summary>
        /// <param name="libTree"></param>
        public LocalDataSetBuilder(DataGridView dgv_items, TabControl tc_attach)
        {
            this.itemDGV = dgv_items;
            this.attachTC = tc_attach;
            ///////////////////////////////
            //AddHeaderCheckBox();
            ////////////////////////////
            //本地数据提交优先考虑只读软连接模式，全选操作模式暂行屏蔽，有待增设状态后再利用。
            /////////////////////////
            //CustomRowSelectionStyle();
            ////////////////////////////
            //该方法在单元格选择情况下有缺陷，暂行屏蔽，有待改进。
        }
        ~LocalDataSetBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            itemDGV = null;
            attachTC = null;
        }
        #endregion
        #region 实例对象保持部分
        private long CUR_LID = CatalogNode.REC_ALL;

        public long CURRENT_LID
        {
            get { return CUR_LID; }
        }
        private long CUR_PLID = CatalogNode.REC_ALL;

        public long CURRENT_PLID
        {
            get { return CUR_PLID; }
        }
        
        private long CUR_RID = -1L;

        public long CURRENT_RID
        {
            get { return CUR_RID; }
            set { CUR_RID=value; }
        }
        /// <summary>
        /// 当前表单显示的查询条件缓存
        /// </summary>
        private volatile string queryCondtion = null;

        public string QueryCondtion
        {
            get { return queryCondtion; }
        }

        private volatile int TotalCheckedCheckBoxes = 0;
        private volatile CheckBox HeaderCheckBox = null;
        private volatile bool IsHeaderCheckBoxClicked = false;
        private volatile int oldRowSelectionIndex = 0;
        #endregion


        #region 自定义itemDGV支持多行选取的代码实现
        /// <summary>
        /// Add the checkBox into the datagridview
        /// </summary>
        private void AddHeaderCheckBox()
        {
            HeaderCheckBox = new CheckBox();
            HeaderCheckBox.Size = new Size(15, 15);

            //Add the CheckBox into the DataGridView
            this.itemDGV.Controls.Add(HeaderCheckBox);
            HeaderCheckBox.KeyUp += new KeyEventHandler(HeaderCheckBox_KeyUp);
            HeaderCheckBox.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);
            itemDGV.CellValueChanged += new DataGridViewCellEventHandler(dgvSelectAll_CellValueChanged);
            itemDGV.CurrentCellDirtyStateChanged += new EventHandler(dgvSelectAll_CurrentCellDirtyStateChanged);
            itemDGV.CellPainting += new DataGridViewCellPaintingEventHandler(dgvSelectAll_CellPainting);
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
                RowCheckBoxClick((DataGridViewCheckBoxCell)itemDGV[e.ColumnIndex, e.RowIndex]);
        }
        /// <summary>
        /// 当单元格状态改变时，事件触发机制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvSelectAll_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (itemDGV.CurrentCell!=null && itemDGV.CurrentCell.OwningColumn.Name.Equals(CTDRecordA.CTD_RID))
                itemDGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
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
            Rectangle oRectangle = this.itemDGV.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);

            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBox.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBox.Height) / 2 + 1;

            //Change the location of the CheckBox to make it stay on the header
            HeaderCheckBox.Location = oPoint;
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="HCheckBox"></param>
        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            IsHeaderCheckBoxClicked = true;

            foreach (DataGridViewRow Row in itemDGV.Rows)
                ((DataGridViewCheckBoxCell)Row.Cells[0]).Value = HCheckBox.Checked;
            itemDGV.RefreshEdit();
            int TotalCheckBoxes = itemDGV.Rows.Count;
            TotalCheckedCheckBoxes = HCheckBox.Checked ? TotalCheckBoxes : 0;
            IsHeaderCheckBoxClicked = false;
        }
        /// <summary>
        /// 选中一行
        /// </summary>
        /// <param name="RCheckBox"></param>
        private void RowCheckBoxClick(DataGridViewCheckBoxCell RCheckBox)
        {
            int TotalCheckBoxes = itemDGV.Rows.Count;
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
#endregion 

        #region 自定义itemDGV行选中的外观样式
        private void CustomRowSelectionStyle()
        {
            this.itemDGV.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            // Attach handlers to DataGridView events.
            this.itemDGV.ColumnWidthChanged += new
                DataGridViewColumnEventHandler(itemDGV_ColumnWidthChanged);
            this.itemDGV.RowPrePaint += new
                DataGridViewRowPrePaintEventHandler(itemDGV_RowPrePaint);
            this.itemDGV.RowPostPaint += new
                DataGridViewRowPostPaintEventHandler(itemDGV_RowPostPaint);
            this.itemDGV.CurrentCellChanged += new
                EventHandler(itemDGV_CurrentCellChanged);
            this.itemDGV.RowHeightChanged += new
                DataGridViewRowEventHandler(itemDGV_RowHeightChanged);
        }
        // Forces the control to repaint itself when the user 
        // manually changes the width of a column.
        void itemDGV_ColumnWidthChanged(object sender,
            DataGridViewColumnEventArgs e)
        {
            this.itemDGV.Invalidate();
        }

        // Forces the row to repaint itself when the user changes the 
        // current cell. This is necessary to refresh the focus rectangle.
        void itemDGV_CurrentCellChanged(object sender, EventArgs e)
        {
            if (oldRowSelectionIndex != -1)
            {
                this.itemDGV.InvalidateRow(oldRowSelectionIndex);
            }
            oldRowSelectionIndex = this.itemDGV.CurrentCellAddress.Y;
        }

        // Paints the custom selection background for selected rows.
        void itemDGV_RowPrePaint(object sender,DataGridViewRowPrePaintEventArgs e)
        {
            // Do not automatically paint the focus rectangle.
            e.PaintParts &= ~DataGridViewPaintParts.Focus;

            // Determine whether the cell should be painted
            // with the custom selection background.
            if ((e.State & DataGridViewElementStates.Selected) ==
                        DataGridViewElementStates.Selected)
            {
                // Calculate the bounds of the row.
                Rectangle rowBounds = new Rectangle(
                    this.itemDGV.RowHeadersWidth, e.RowBounds.Top,
                    this.itemDGV.Columns.GetColumnsWidth(
                        DataGridViewElementStates.Visible) -
                    this.itemDGV.HorizontalScrollingOffset + 1,
                    e.RowBounds.Height);

                // Paint the custom selection background.
                using (Brush backbrush =
                    new System.Drawing.Drawing2D.LinearGradientBrush(rowBounds,
                        this.itemDGV.DefaultCellStyle.SelectionBackColor,
                        Color.LightSkyBlue,
                        System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(backbrush, rowBounds);
                }
            }
        }

        // Paints the content that spans multiple columns and the focus rectangle.
        void itemDGV_RowPostPaint(object sender,
            DataGridViewRowPostPaintEventArgs e)
        {
            // Calculate the bounds of the row.
            Rectangle rowBounds = new Rectangle(
                this.itemDGV.RowHeadersWidth, e.RowBounds.Top,
                this.itemDGV.Columns.GetColumnsWidth(
                    DataGridViewElementStates.Visible) -
                this.itemDGV.HorizontalScrollingOffset + 1,
                e.RowBounds.Height);

            SolidBrush forebrush = null;
            try
            {
                // Determine the foreground color.
                if ((e.State & DataGridViewElementStates.Selected) ==
                    DataGridViewElementStates.Selected)
                {
                    forebrush = new SolidBrush(e.InheritedRowStyle.SelectionForeColor);
                }
                else
                {
                    forebrush = new SolidBrush(e.InheritedRowStyle.ForeColor);
                }

                DataGridViewRow dgvr = this.itemDGV.Rows.SharedRow(e.RowIndex);
                if (dgvr != null && dgvr.Index > 0)
                {
                    // Get the content that spans multiple columns.
                    object recipe =
                        this.itemDGV.Rows.SharedRow(e.RowIndex).Cells[2].Value;

                    if (recipe != null)
                    {
                        String text = recipe.ToString();

                        // Calculate the bounds for the content that spans multiple 
                        // columns, adjusting for the horizontal scrolling position 
                        // and the current row height, and displaying only whole
                        // lines of text.
                        Rectangle textArea = rowBounds;
                        textArea.X -= this.itemDGV.HorizontalScrollingOffset;
                        textArea.Width += this.itemDGV.HorizontalScrollingOffset;
                        textArea.Y += rowBounds.Height - e.InheritedRowStyle.Padding.Bottom;
                        textArea.Height -= rowBounds.Height -
                            e.InheritedRowStyle.Padding.Bottom;
                        textArea.Height = (textArea.Height / e.InheritedRowStyle.Font.Height) *
                            e.InheritedRowStyle.Font.Height;

                        // Calculate the portion of the text area that needs painting.
                        RectangleF clip = textArea;
                        clip.Width -= this.itemDGV.RowHeadersWidth + 1 - clip.X;
                        clip.X = this.itemDGV.RowHeadersWidth + 1;
                        RectangleF oldClip = e.Graphics.ClipBounds;
                        e.Graphics.SetClip(clip);

                        // Draw the content that spans multiple columns.
                        e.Graphics.DrawString(
                            text, e.InheritedRowStyle.Font, forebrush, textArea);

                        e.Graphics.SetClip(oldClip);
                    }
                }
            }
            finally
            {
                forebrush.Dispose();
            }

            if (this.itemDGV.CurrentCellAddress.Y == e.RowIndex)
            {
                // Paint the focus rectangle.
                e.DrawFocus(rowBounds, true);
            }
        }

        // Adjusts the padding when the user changes the row height so that 
        // the normal cell content is fully displayed and any extra
        // height is used for the content that spans multiple columns.
        void itemDGV_RowHeightChanged(object sender,
            DataGridViewRowEventArgs e)
        {
            // Calculate the new height of the normal cell content.
            Int32 preferredNormalContentHeight =
                e.Row.GetPreferredHeight(e.Row.Index,
                DataGridViewAutoSizeRowMode.AllCellsExceptHeader, true) -
                e.Row.DefaultCellStyle.Padding.Bottom;

            // Specify a new padding.
            Padding newPadding = e.Row.DefaultCellStyle.Padding;
            newPadding.Bottom = e.Row.Height - preferredNormalContentHeight;
            e.Row.DefaultCellStyle.Padding = newPadding;
        }
        #endregion


        /// <summary>
        /// 从剪贴板粘贴文本型数据记录到目标区域
        /// This will be moved to the util class so it can service any paste into a DGV
        /// </summary>
        internal void PasteClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int iFail = 0, iRow = itemDGV.CurrentCell.RowIndex;
                int iCol = itemDGV.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                foreach (string line in lines)
                {
                    if (iRow < itemDGV.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this.itemDGV.ColumnCount)
                            {
                                oCell = itemDGV[iCol + i, iRow];
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
        /// 根据指定的索引位序更新显示附加的属性信息
        /// </summary>
        /// <param name="dgvr"></param>
        /// <param name="tc"></param>
        public void selectViewRecord(DataGridViewRow dgvr)
        {
            int rIdx = dgvr.DataGridView.Columns[CTDRecordA.CTD_RID.ToString()].Index;
            if (dgvr.Cells.Count > rIdx)
            {
                CUR_RID = Convert.ToInt64(dgvr.Cells[rIdx].FormattedValue.ToString());
                DataTable table = LocalRecordMHub.queryCTDRecord(DataSourceHolder.DataSource, CUR_RID.ToString());
                if (table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    List<string> viewAttrs = LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource);
                    showReferences(viewAttrs, dr);
                }
            }
        }

        /// <summary>
        /// 根据指定的数据集合加载数据报表显示，指定的字段映射为空时则使用默认的字段映射显示规则。
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="dataset"></param>
        /// <param name="colMap"></param>
        public void loadDataSetView()
        {
            List<string> viewAttrs = LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource);//获取所有属性名称集合
            lock (LocalDataGridView_Lock)
            {
                if (itemDGV.ColumnCount > 0)
                {
                    DGVAsyncUtil.syncRemoveAllRow(itemDGV);
                    resetReferences(viewAttrs);
                }
                else
                {
                    //行列表头显示
                    loadDGVColumns(viewAttrs);
                    loadReferences(viewAttrs);
                }
            }
        }
        /// <summary>
        /// 标记目标数据报表关联文档目录键值
        /// </summary>
        public void noteDataSetLib(TreeNode filterNode)
        {
            long lid = Convert.ToInt64(filterNode.Name);

            if (filterNode.Level > 0)
            {
                CUR_LID = lid;
                CUR_PLID = Convert.ToInt64(filterNode.Parent.Name);
            }
            else
            {
                CUR_LID = lid;
                CUR_PLID = lid;
            }
        }
        /// <summary>
        /// 删除数据归档目标的数据记录，置入未分类目录
        /// </summary>
        /// <param name="filteNode"></param>
        /// <param name="lid"></param>
        public void trashDataSet(TreeNode filteNode, int newlid = CatalogNode.REC_TRASH)
        {
            List<string> viewAttrs = LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource);
            long lid = Convert.ToInt64(filteNode.Name);
            lock (LocalDataGridView_Lock)
            {
                if (filteNode.Level > 0)
                {
                    CUR_LID = lid;
                    CUR_PLID = Convert.ToInt64(filteNode.Parent.Name);
                }
                else
                {
                    CUR_LID = lid;
                    CUR_PLID = lid;
                }
                itemDGV.Rows.Clear();
                resetReferences(viewAttrs);
                string filterLids = lid.ToString();
                if (lid > 0)
                {
                    long[] lids = LocalRecordMHub.extractToLids(DataSourceHolder.DataSource,lid);
                    if (lids != null)
                    {
                        filterLids = "";
                        foreach (long _lid in lids)
                        {
                            filterLids += "," + _lid;
                        }
                        filterLids = filterLids.Substring(1);
                    }
                }
                //数据归档更新
                LocalRecordMHub.updateCTCRecordLid(DataSourceHolder.DataSource, newlid, CatalogNode.REC_ALL, filterLids);
            }
        }
        /// <summary>
        /// 删除数据归档目标的数据记录，置入未分类目录
        /// </summary>
        /// <param name="filteNode"></param>
        public void dropDataSet(TreeNode filteNode)
        {
            List<string> viewAttrs = LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource);
            long lid = Convert.ToInt64(filteNode.Name);
            lock (LocalDataGridView_Lock)
            {
                if (filteNode.Level > 0)
                {
                    CUR_LID = lid;
                    CUR_PLID = Convert.ToInt64(filteNode.Parent.Name);
                }
                else
                {
                    CUR_LID = lid;
                    CUR_PLID = lid;
                }
                itemDGV.Rows.Clear();
                resetReferences(viewAttrs);
                string filterLids = lid.ToString();
                if (lid > 0)
                {
                    long[] lids = LocalRecordMHub.extractToLids(DataSourceHolder.DataSource,lid);
                    if (lids != null)
                    {
                        filterLids = "";
                        foreach (long _lid in lids)
                        {
                            filterLids += "," + _lid;
                        }
                        filterLids = filterLids.Substring(1);
                    }
                }
                //数据归档更新
                LocalRecordMHub.dropCTCRecordLid(DataSourceHolder.DataSource, filterLids);
            }
        }
        

        /// <summary>
        /// 加载数据表头展示
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="attrs"></param>
        private void loadDGVColumns(List<string> viewAttrs)
        {
            DGVAsyncUtil.syncClearAll(itemDGV);
            //默认前置的选择列
            DataGridViewCheckBoxColumn chxCol = new DataGridViewCheckBoxColumn();
            chxCol.ReadOnly = true;
            chxCol.Resizable = DataGridViewTriState.False;
            chxCol.FlatStyle = FlatStyle.Popup;
            chxCol.CellTemplate.Style.ForeColor = Color.LightGray;
            DGVAsyncUtil.syncAddCol(itemDGV,chxCol);
            //创建显性列属性
            foreach (string attr in viewAttrs)
            {
                int viewOrder = LocalRecordMHub.getViewOrder(DataSourceHolder.DataSource, attr);//返回属性显示位序
                log.Debug("##"+attr+"->"+viewOrder);
                if (viewOrder < CustomTColMap.MaxMainViewCount)
                {
                    CustomTColDef ctcd = LocalRecordMHub.getCustomTColDef(DataSourceHolder.DataSource,attr);
                    Type colType = RecordControlTypeConverter.getDGVColType(ctcd.AttrType);
                    DataGridViewColumn dgvCol = Activator.CreateInstance(colType) as DataGridViewColumn;
                    dgvCol.Name = ctcd.Attr;
                    dgvCol.HeaderText = CVNameConverter.toViewName(ctcd.Attr);
                    if(attr.Equals(CTDRecordA.CTD_RID)|| attr.Equals(CTDRecordA.CTD_PLID) || attr.Equals(CTDRecordA.CTD_LID)){
                        dgvCol.Visible = false;
                        dgvCol.Width = 0;
                    }
                    DGVAsyncUtil.syncAddCol(itemDGV, dgvCol);
                    if (viewOrder != dgvCol.Index)
                        LocalRecordMHub.updateViewOrder(DataSourceHolder.DataSource,attr, dgvCol.Index);
                }
            }
        }
        /// <summary>
        /// 加载附加注解字段名展示
        /// </summary> 
        /// <param name="dgv"></param>
        /// <param name="attrs"></param>
        private void loadReferences(List<string> viewAttrs)
        {
            //清理tabControl控件
            foreach (TabPage tp in attachTC.TabPages)
            {
                foreach (Control ictl in tp.Controls)
                {
                    ictl.Dispose();
                }
            }
            //重新build references Page
            TabPage tabPage = attachTC.TabPages["references"];
            int idx = 0;
            foreach (string attr in viewAttrs)
            {
                if (LocalRecordMHub.getViewOrder(DataSourceHolder.DataSource, attr) < CustomTColMap.MaxMainViewCount)
                {
                    ++idx;
                    continue;
                }
                CustomTColDef ctcd = LocalRecordMHub.getCustomTColDef(DataSourceHolder.DataSource, attr);
                Panel panel = new Panel();
                panel.Name = "referPanel_" + idx;
                panel.Dock = DockStyle.Top;
                panel.Margin = new Padding(0, 5, 0, 0);
                panel.BorderStyle = System.Windows.Forms.BorderStyle.None;
                panel.Height = 45;
                //panel.BorderStyle = BorderStyle.FixedSingle;
                TextBox label = new TextBox();
                label.ReadOnly = true;
                label.BorderStyle = System.Windows.Forms.BorderStyle.None;
                label.BackColor = Color.WhiteSmoke;
                label.Name = "referLabel_" + ctcd.Attr;
                string pattr =CVNameConverter.toViewName(ctcd.Attr);
                label.Text = pattr;
                label.Font = new Font(label.Font, label.Font.Style ^ FontStyle.Bold);
                label.Height = 14;
                label.Dock = DockStyle.Top;
                panel.Controls.Add(label);
                Type ctype = RecordControlTypeConverter.getControlType(ctcd.AttrType);
                Control control = Activator.CreateInstance(ctype) as Control;
                if (control is TextBox)
                {
                    (control as TextBox).BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                }
                control.Name = pattr;
                control.Dock = DockStyle.Bottom;
                control.Height = 20;
                control.Margin = new Padding(0, 1, 0, 0);
                control.TextChanged += Refers_record_TextChanged;
                panel.Controls.Add(control);
                tabPage.Controls.Add(panel);
                ++idx;
            }
            Label ridLabel = new Label();
            ridLabel.Name = "ctd_rid_Label";
            ridLabel.Visible = false;
            ridLabel.Height = 0;
            ridLabel.Dock = DockStyle.Top;
            ridLabel.Margin = new Padding(0, 0, 0, 0);
            tabPage.Controls.Add(ridLabel);
        }
        /// <summary>
        /// 根据指定的数据行置空所有附加的属性信息
        /// </summary>
        /// <param name="viewAttrs"></param>
        public void resetReferences(List<string> viewAttrs)
        {
            showReferences(viewAttrs, null);
        }
        /// <summary>
        /// 根据指定的数据行更新显示附加的属性信息
        /// </summary>
        /// <param name="viewAttrs"></param>
        /// <param name="dr"></param>
        /// dr为null则等效于清空表单操作
        public void showReferences(List<string> viewAttrs, DataRow dr = null)//参数是所有属性名称集合
        {
            TabPage tabPage = attachTC.TabPages["references"];
            if (dr != null)
            {
                (tabPage.Controls["ctd_rid_Label"] as Label).Text = dr[CTDRecordA.CTD_RID].ToString();
            }
            foreach (Control ctl in tabPage.Controls)//获取选项卡内所有集合
            {
                if (ctl is Panel)//如果空间是面板
                {
                    //获取，选择项卡中的集合名称从referPanel_开始处的索引
                    int idx = Convert.ToInt32(ctl.Name.Substring("referPanel_".Length));

                    //从viewAttrs[idx]第一位开始，viewAttrs[idx]-2个长度的字符串
                    string attr =CVNameConverter.toViewName(viewAttrs[idx]);//注意：
                    Control ictl = ctl.Controls[attr];
                    if (ictl != null)
                    {
                        if (ictl is TextBox)
                        {
                            (ictl as TextBox).Text = dr == null ? "" : dr[attr].ToString();
                        }
                        else if (ictl is ComboBox)
                        {
                            (ictl as ComboBox).FormatString = dr == null ? "" : dr[attr].ToString();
                        }
                        else if (ictl is DateTimePicker)
                        {
                            (ictl as DateTimePicker).CustomFormat = dr == null ? "" : dr[attr].ToString();
                        }
                    }
                }
            }
        }
        public DataGridViewCell quickSearch(string findTerm)
        {
            if (findTerm.Length > 0)
            {
                DataGridViewCell ncell = null;
                if (itemDGV.SelectedCells != null && itemDGV.SelectedCells.Count > 0)
                {
                    if(itemDGV.SelectedCells[0].Displayed)
                        ncell = itemDGV.SelectedCells[0];
                }
                while ((ncell = nextTextCell(ncell)) != null)
                {
                    string cellval = DGVUtil.getCellValue(ncell,"");
                    if (cellval.ToLower().Contains(findTerm.ToLower()))
                    {
                        return ncell;
                    }
                }
                if (ncell == null)
                {
                    MessageBox.Show("It's reached the end, and no subsequent matches.");
                }
            }
            return null;
        }
        public void doDBDataSearch(string whereCmd)
        {
            //unimplemented
        }
        /// <summary>
        /// 下一个单元格定位，如定位失败返回null
        /// </summary>
        /// <returns></returns>
        private DataGridViewCell nextTextCell(DataGridViewCell cell = null)
        {
            DataGridViewCell ncell = null;
            int rowCount = DGVUtil.getRowCount(itemDGV);
            int columnCount = DGVUtil.getTextColumnCount(itemDGV);
            int rowIndex = cell == null ? 0 : cell.RowIndex;
            int colIndex = cell == null ? 0 : cell.ColumnIndex + 1;
            if (colIndex >= columnCount)
            {
                rowIndex = rowIndex + 1;
                colIndex = 0;
            }
            if (colIndex < columnCount && rowIndex<rowCount)
            {
                DataGridViewRow dgvr = itemDGV.Rows[rowIndex];
                if (!dgvr.IsNewRow)
                {
                    ncell = dgvr.Cells[colIndex];
                    if (ncell.Visible && ncell is DataGridViewTextBoxCell)
                    {
                        return ncell;
                    }
                    else
                        return nextTextCell(ncell);
                }
            }
            return ncell;
        }
        /// <summary>
        /// 添加新纪录，自动生成默认的数据字段值
        /// </summary>
        /// <param name="dgv"></param>
        public void addNewRecord()
        {
            long nuid = LocalRecordMHub.addNewRecord(DataSourceHolder.DataSource,CUR_LID, CURRENT_PLID);
            if (nuid > 0)
            {
                int idx = itemDGV.Rows.Add();
                itemDGV.Rows[idx].Cells[CTDRecordA.CTD_RID].Value = nuid;
                itemDGV.Rows[idx].Cells[CTDRecordA.CTD_LID].Value = CUR_LID;
                itemDGV.Rows[idx].Cells[CTDRecordA.CTD_PLID].Value = CURRENT_PLID;
            }
        }
        
        /// <summary>
        /// 伴随用户修改操作同步更新记录属性信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Refers_record_TextChanged(object sender, EventArgs e)
        {
            if ((sender as Control).Focused)
            {
                string rid = null;
                string attrName = null;
                string cellVal = null;
                if (sender is TextBox)
                {
                    TextBox tb = (sender as TextBox);
                    rid = (tb.Parent.Parent.Controls["ctd_rid_Label"] as Label).Text;
                    attrName = tb.Name;
                    cellVal = tb.Text.Trim();
                }
                else if (sender is ComboBox)
                {
                    ComboBox tb = (sender as ComboBox);
                    rid = (tb.Parent.Parent.Controls["ctd_rid_Label"] as Label).Text;
                    attrName = tb.Name;
                    cellVal = tb.FormatString.Trim();
                }
                else if (sender is DateTimePicker)
                {
                    DateTimePicker tb = (sender as DateTimePicker);
                    rid = (tb.Parent.Parent.Controls["ctd_rid_Label"] as Label).Text;
                    attrName = tb.Name;
                    cellVal = tb.Text.Trim();
                }
                if (rid.Length > 0 && cellVal.Length > 0)
                {
                    LocalRecordMHub.updateAttrVal(DataSourceHolder.DataSource,rid, cellVal, "[" + attrName + "]");
                }
            }
        }
        /// <summary>
        /// 解析指定的Excel文档，验证数据转换的属性映射条件.
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns></returns>
        public bool checkForExcelImport(string fpath, ref Dictionary<string, string> dataMapping, Form pForm)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPath = System.IO.Path.GetFullPath(fpath);
            IWorkbook workbook = null;
            try
            {
                using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    workbook = WorkbookFactory.Create(fs);
                    ISheet dataSheet = workbook.GetSheet("Core Datasets");
                    if (dataSheet == null)
                        dataSheet = workbook.GetSheetAt(0);
                    return fetchSheetMappingInfo(dataSheet, ref dataMapping, pForm) && dataMapping.Count > 0;
                }
            }
            catch (Exception ex)
            {
                log.Info("ERROR: Excel文件导入失败！ ", ex);
                MessageBox.Show("ERROR: Excel文件导入失败！ " + ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }
        /// <summary>
        /// 解析指定的XML文档，验证数据转换的属性映射条件.
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns></returns>
        internal bool checkForXMLImport(string fpath, ref Dictionary<string, string> dataMapping, Form pForm)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPaht = System.IO.Path.GetFullPath(fpath);
            try
            {
                XmlDocument xDoc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                using(XmlReader xRead = XmlReader.Create(fullPaht))
                {
                    xDoc.Load(xRead);
                    return fetchXMLMappingInfo(xDoc, ref dataMapping, pForm) && dataMapping.Count > 0;
                }  
            }
            catch(Exception ex)
            {
                log.Info("ERROR: XML文件导入失败！ ", ex);
                MessageBox.Show("ERROR: XML文件导入失败！ " + ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }
        public bool fetchXMLMappingInfo(XmlDocument xDoc,ref Dictionary<string, string> dataMapping, Form pForm)
        {            
            XmlNodeList strainChildNodes = xDoc.DocumentElement.ChildNodes;
            //一直向下探索，直到某个节点下没有子节点，说明这个节点是个attrNode,
            //因为按正常的逻辑，属性节点应该是最小的节点单位了
            //attrNode的集合就是strainChildNodes  
            while (strainChildNodes.Count > 0)
            {
                XmlNode node = strainChildNodes[0];
                if (node.ChildNodes.Count <= 0)
                    break;
                strainChildNodes = node.ChildNodes;
            }

            //节点探测代码
            XmlNode strainNode = strainChildNodes[0].ParentNode;//获取第一个strainNode
            List<string> attrNameList = new List<string>(strainChildNodes.Count);
            int cursor = 0;
            int detectDepth = 5;
            while (!(strainNode == null))
            {
                if (cursor > detectDepth)
                    break;
                if (mergeAttrList(attrNameList, strainNode.ChildNodes))//如果这个节点下有新属性出现，使探测深度增加2倍
                    detectDepth = (int)(detectDepth * 1.5);
                strainNode = nextStrainNode(strainNode);
                cursor++;
            }

            ///////////////////////////////////////////////////////////////
            using (AttrMapOptionDlg amoDlg = new AttrMapOptionDlg())
            {
                amoDlg.BringToFront();
                amoDlg.setInitCols(attrNameList, LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource, false), ref dataMapping);
                amoDlg.ShowDialog();
                ///////////////////////////////////////////
                if (amoDlg.DialogResult == DialogResult.OK)
                    return true;
            }
            return false;
        }
        private XmlNode nextStrainNode(XmlNode strainNode)
        {
            return strainNode.NextSibling;
        }
        private bool mergeAttrList(List<string> attrNameList,XmlNodeList attrNodeList)
        {
            int startLeng = attrNameList.Count;
            foreach (XmlNode strainChildNode in attrNodeList)
            {
                if(!attrNameList.Contains(strainChildNode.Name))
                    attrNameList.Add(strainChildNode.Name);
            }
            int endLeng = attrNameList.Count;
            if (startLeng != endLeng)
                return true;
            return false;
        }
        
        /// <summary>
        /// 通过NPOI读取Excel文档，转换可识别内容至本地数据库中
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dgv"></param>
        private bool fetchSheetMappingInfo(ISheet sheet, ref Dictionary<string, string> dataMapping,Form pForm)
        {
            int skipIdx = 1;
            if (sheet == null || sheet.LastRowNum < skipIdx) //no data
                return false;
            /////////////////////////////////////////////////////////
            IRow titleRow = sheet.GetRow(skipIdx - 1);
            int columnSize = titleRow.LastCellNum;
            int rowSize = sheet.LastRowNum;
            List<string> xlscols = new List<string>(columnSize);
            for (int i = titleRow.FirstCellNum; i < columnSize; i++)
            {
                ICell titleCell = titleRow.GetCell(i);
                if (titleCell != null && titleCell.ToString().Length > 0)
                {
                    string cellData = titleCell.ToString();
                    xlscols.Add(CVNameConverter.toViewName(cellData.Trim().ToLower()));
                }
                else
                {
                    xlscols.Add(null);
                }
            }
            ///////////////////////////////////////////////////////////////
            using (AttrMapOptionDlg amoDlg = new AttrMapOptionDlg())
            {
                amoDlg.BringToFront();
                amoDlg.setInitCols(xlscols, LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource,false), ref dataMapping);
                amoDlg.ShowDialog();
                ///////////////////////////////////////////
                if (amoDlg.DialogResult == DialogResult.OK)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 解析本地数据属性和GCM发布数据的属性映射条件
        /// </summary>
        /// <param name="dataMapping"></param>
        /// <returns></returns>
        public bool checkForGCMImport(ref Dictionary<string, string> dataMapping)
        {
            List<string> gcmCols = GCMDataMHub.fetchPublishGCMFields();
            List<string> viewCols = LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource, false);
            if (gcmCols == null || gcmCols.Count < 1)
                return false;
            ///////////////////////////////////////////////////////////////
            using (AttrMapOptionDlg amoDlg = new AttrMapOptionDlg())
            {
                amoDlg.BringToFront();
                amoDlg.setInitCols(viewCols, gcmCols, ref dataMapping);
                amoDlg.ShowDialog();
                ///////////////////////////////////////////
                if (amoDlg.DialogResult == DialogResult.OK)
                    return true;
            }
            return false;
        }
        private DataGridView itemDGV;

        public DataGridView ItemDGV
        {
            get { return itemDGV; }
        }
        private TabControl attachTC;

        public TabControl AttachTC
        {
            get { return attachTC; }
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 本地表单数据视图控件的独占保持的共享锁对象
        /// </summary>
        public static object LocalDataGridView_Lock = new object();
    }
}
