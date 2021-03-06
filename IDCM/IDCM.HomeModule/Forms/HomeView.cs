﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using IDCM.Service.Utils;
using IDCM.HomeModule.Modules;

namespace IDCM.HomeModule.Forms
{
    public partial class HomeView : Form
    {
        private DataGridViewSelectedRowCollection dgvsRowCollection = null;//用来保存选中的行
        private HomeViewManager manager = null;
        internal void setManager(HomeViewManager manager)
        {
            this.manager = manager;
        }
        /// <summary>
        /// 客户端默认首页面视图实例初始化
        /// </summary>
        public HomeView()
        {
            InitializeComponent();
            this.splitContainer_middle.Panel1Collapsed = true;
            dataGridView_items.AllowDrop = true;
            dataGridView_items.AllowUserToAddRows = false;
            dataGridView_items.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView_items.EditMode = DataGridViewEditMode.EditOnKeystroke;
            ////dataGridView_items.TopLeftHeaderCell = new DataGridViewTopLeftHeaderCell();
            ////dataGridView_items.AdjustedTopLeftHeaderBorderStyle = DataGridViewTopLeftHeaderCell.MeasureTextPreferredSize();
            
            treeView_base.ImageList = imageList_lib;
            treeView_library.ImageList = imageList_lib;
        }
        /// <summary>
        /// 客户端默认首页面视图实例化后，加载数据过程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeView_Load(object sender, EventArgs e)
        {
            //Thread.CurrentThread.Name = "IDCM_HomeView" + HandleToken.nextTempID();
        }

        private void HomeView_Shown(object sender, EventArgs e)
        {
            activeDataView(true);
        }
        /// <summary>
        /// activeHomeView
        /// </summary>
        /// <param name="refresh"></param>
        public void activeDataView(bool refresh=true)
        {
            //加载默认的数据报表展示
            manager.loadTreeSet();
            manager.loadDataSetView(treeView_base.Nodes[0]);
            manager.updateLibRecCount();
            //resize for data view
            dataGridView_items.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //dataGridView_items.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None);
            dataGridView_items.AllowUserToResizeColumns = true;
        }

        /// <summary>
        /// 左键或右键的Base分组事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_base_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                treeView_base.SelectedNode = e.Node;
                manager.noteCurSelectedNode(e.Node);
                //左键(激活节点选定事件，并将触发必要的右侧数据表单的更新显示)
                if (e.Button == MouseButtons.Left)
                {
                }
                //右键（不会激活节点选定事件，事件触发节点信息通过匿名委托方法绑定到弹出菜单项的事件方法中去）
                if (e.Button == MouseButtons.Right)
                {
                    foreach (ToolStripItem tsItem in contextMenuStrip_base.Items)
                    {
                        if (tsItem is ToolStripSeparator)
                            continue;
                        ControlUtil.ClearEvent(tsItem, "Click");
                        tsItem.Click += delegate(object tsender, EventArgs te) { toolStripMenuItem_base_Click(tsender, te, e.Node); };
                    }
                    contextMenuStrip_base.Show(treeView_base, e.X, e.Y);
                }
            }
        }
        /// <summary>
        /// 左键或右键单击的自定义分组事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_library_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                treeView_library.SelectedNode = e.Node;
                manager.noteCurSelectedNode(e.Node);
                //左键
                if (e.Button == MouseButtons.Left)
                {
                }
                //右键
                if (e.Button == MouseButtons.Right)
                {
                    LocalLibBuilder.filterContextMenuItems(contextMenuStrip_lib, e.Node);
                    foreach (ToolStripItem tsItem in contextMenuStrip_lib.Items)
                    {
                        if (tsItem is ToolStripSeparator)
                            continue;
                        ControlUtil.ClearEvent(tsItem, "Click");
                        tsItem.Click += delegate(object tsender, EventArgs te) { toolStripMenuItem_lib_Click(tsender, te, e.Node); };
                    }
                    contextMenuStrip_lib.Show(treeView_library, e.X, e.Y);
                }
            }
        }
        protected void toolStripMenuItem_base_Click(object sender, EventArgs e, TreeNode node)
        {
            string name=((ToolStripItem)sender).Name;
            switch (name)
            {
                case "clear_toolStripMenuItem":
                    clear_toolStripMenuItem_Click(sender, e, node);
                    break;
                default:
                    break;
            }
        }
        protected void toolStripMenuItem_lib_Click(object sender, EventArgs e,TreeNode node)
        {
            string name = ((ToolStripItem)sender).Name;
            switch (name)
            {
                case "CreateGroupSet":
                    CreateGroupSet_Click(sender, e, node);
                    break;
                case "CreateGroup":
                    CreateGroup_Click(sender, e, node);
                    break;
                case "CreateSmartGroup":
                    CreateSmartGroup_Click(sender, e, node);
                    break;
                case "RenameGroup":
                    RenameGroup_Click(sender, e, node);
                    break;
                case "RenameGroupSet":
                    RenameGroupSet_Click(sender, e, node);
                    break;
                case "DeleteGroup":
                    DeleteGroup_Click(sender, e, node);
                    break;
                case "DeleteGroupSet":
                    DeleteGroupSet_Click(sender, e, node);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 新建分组节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateGroupSet_Click(object sender, EventArgs e, TreeNode node)
        {
            if (node != null)
            {
                manager.addGroupSet(node);
            }
        }

        private void RenameGroupSet_Click(object sender, EventArgs e, TreeNode node)
        {
            if (node != null)
            {
                treeView_library.LabelEdit = true;
                node.BeginEdit();
            }
        }

        private void DeleteGroupSet_Click(object sender, EventArgs e, TreeNode node)
        {
            if (node != null)
            {
                DialogResult res = MessageBox.Show("Are you sure to delete the selected Node named " + node.Text, "Confirm Delete", MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    manager.deleteNode(node);
                }
            }
        }

        private void CreateGroup_Click(object sender, EventArgs e, TreeNode node)
        {
            if (node != null)
            {
                manager.addGroup(node);
            }
        }

        private void RenameGroup_Click(object sender, EventArgs e, TreeNode node)
        {
            if (node != null)
            {
                treeView_library.LabelEdit = true;
                node.BeginEdit();
            }
        }

        private void DeleteGroup_Click(object sender, EventArgs e, TreeNode node)
        {
            if (node != null)
            {
                DialogResult res = MessageBox.Show("Are you sure to delete the selected Node named " + node.Text, "Confirm Delete", MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    manager.deleteNode(node);
                }
            }
        }
        private void CreateSmartGroup_Click(object sender, EventArgs e, TreeNode node)
        {
            if (node != null)
            {
                MessageBox.Show("UnImplicated");
            }
        }
        private void clear_toolStripMenuItem_Click(object sender, EventArgs e, TreeNode node)
        {
            if (node != null)
            {
                DialogResult res = MessageBox.Show("Are you sure to Empty Data", "Confirm Empty", MessageBoxButtons.OKCancel);
                if (res == DialogResult.OK)
                {
                    manager.trashDataSet(node);
                }
            }
        }
        private void treeView_library_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            manager.renameNode(e.Node, e.Label);
            e.Node.EndEdit(false);
            treeView_library.LabelEdit = false;
            manager.updateLibRecCount(e.Node);
        }
        private void treeView_library_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Equals(manager.SelectedNode_Current))
            {
                e.Graphics.FillRectangle(Brushes.LightYellow, e.Bounds.Left, e.Bounds.Top, e.Node.TreeView.Width - e.Bounds.Left-2, e.Bounds.Height);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Transparent, e.Bounds.Left, e.Bounds.Top, e.Node.TreeView.Width - e.Bounds.Left-2, e.Bounds.Height);
            }
            //由系統繪制
            e.Graphics.DrawString(e.Node.Text, e.Node.TreeView.Font, Brushes.Black, e.Bounds.Left, e.Bounds.Top);
            ////e.DrawDefault = true;
            if (e.Node.Tag != null)
            {
                string newMail = string.Format(" ({0})", e.Node.Tag.ToString());
                e.Graphics.DrawString(newMail, e.Node.TreeView.Font, Brushes.Blue, e.Bounds.Right, e.Bounds.Top);
            }
            //e.Node.Expand();
        }

        private void treeView_base_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Equals(manager.SelectedNode_Current))
            {
                e.Graphics.FillRectangle(Brushes.LightYellow, e.Bounds.Left, e.Bounds.Top, e.Node.TreeView.Width - e.Bounds.Left-2, e.Bounds.Height);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Transparent, e.Bounds.Left, e.Bounds.Top, e.Node.TreeView.Width - e.Bounds.Left-2, e.Bounds.Height);
            }
            //由系統繪制
            e.Graphics.DrawString(e.Node.Text, e.Node.TreeView.Font, Brushes.Black, e.Bounds.Left, e.Bounds.Top);
            ////e.DrawDefault = true;
            if (e.Node.Tag != null)
            {
                string newMail = string.Format(" ({0})", e.Node.Tag.ToString());
                e.Graphics.DrawString(newMail, e.Node.TreeView.Font, Brushes.Blue, e.Bounds.Right, e.Bounds.Top);
            }
            //e.Node.Expand();
        }
        
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_add_Click(object sender, EventArgs e)
        {
            if (dataGridView_items.Rows.Count > 0 && dataGridView_items.Rows[dataGridView_items.Rows.Count - 1].IsNewRow)
                return;
            manager.addNewRecord();
            manager.updateLibRecCount();
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_del_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_items.SelectedRows)
            {
                DataGridViewCell idCell = row.Cells[CTDRecordDAM.CTD_RID];
                if (idCell != null)
                {
                    long uid = Convert.ToInt64(idCell.FormattedValue.ToString());
                    {
                        int ic =-1;
                        if (manager.CURRENT_LID != LibraryNodeDAM.REC_TRASH && manager.CURRENT_LID !=LibraryNodeDAM.REC_TEMP)
                        {
                            ic=CTDRecordDAM.updateCTCRecordLid(LibraryNodeDAM.REC_TRASH,LibraryNodeDAM.REC_UNFILED, uid);
                        }
                        else
                        {
                            ic=CTDRecordDAM.deleteRec(uid);
                        }
                        if (ic > 0)
                            dataGridView_items.Rows.Remove(row);
                    }
                }
            }
            manager.updateLibRecCount();
        }
        /// <summary>
        /// 单元格的值改变后，执行更新或插入操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_items_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
                DataGridViewCell idCell = dataGridView_items.Rows[e.RowIndex].Cells[CTDRecordDAM.CTD_RID];
                DataGridViewCell cell = dataGridView_items.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell != null && idCell!=null)
                {
                    string cellVal = cell.FormattedValue.ToString();
                    string attrName = dataGridView_items.Columns[e.ColumnIndex].Name;
                    if (idCell.Value != null)
                    {
                        string uid = idCell.FormattedValue.ToString();
                        CTDRecordDAM.updateAttrVal(uid, cellVal, attrName);
                    }
                    else
                    {
                        Console.WriteLine("Error!!!");
                    }
                }
            }
        }

        /// <summary>
        /// 拖拽事件运行时的鼠标状态切换方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_items_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        /// <summary>
        /// 文件拖拽后事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_items_DragDrop(object sender, DragEventArgs e)
        {
            String[] recvs = (String[])e.Data.GetData(DataFormats.FileDrop, false);
            for (int i = 0; i < recvs.Length; i++)
            {
                if (recvs[i].Trim() != "")
                {
                    String fpath = recvs[i].Trim();
                    bool exists = System.IO.File.Exists(fpath);
                    if (exists == true)
                    {
                        try
                        {
                            manager.importData(fpath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("数据导入失败。");
                            log.Info("数据导入失败，错误信息：",ex);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 变更选取行记录事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_items_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView_items.Rows.Count>0)
            {
                DataGridViewRow dgvr = dataGridView_items.CurrentRow;
                string rid = dgvr.Cells[CTDRecordDAM.CTD_RID].FormattedValue.ToString();
                if (rid.Length > 0 && !rid.Equals(manager.CURRENT_RID.ToString()))
                {
                    manager.selectViewRecord(dgvr);
                }
            }
        }
        private void toolStripTextBox_quickSearch_TextChanged(object sender, EventArgs e)
        {

        }
        

        private void dataGridView_items_MouseDown(object sender, MouseEventArgs e)
        {
            //捕获鼠标点击区域的信息
            DataGridView.HitTestInfo hitTestInfo = this.dataGridView_items.HitTest(e.X, e.Y);
            if (hitTestInfo.RowIndex > -1)
            {
                if (this.dataGridView_items.SelectedRows.Count > 0)
                {
                    dgvsRowCollection = this.dataGridView_items.SelectedRows;
                }
            }
            else
                dgvsRowCollection = null;
        }

        private void dataGridView_items_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (dgvsRowCollection != null)
                {
                    DragDropEffects effect = this.dataGridView_items.DoDragDrop(dgvsRowCollection, DragDropEffects.Link);
                    if (effect == DragDropEffects.Move)
                    {
                        Console.WriteLine("将dgvsRowCollection重新置空");
                        //将dgvsRowCollection重新置空
                        dgvsRowCollection = null;
                    }
                }
            }
        }

        private void treeView_library_DragOver(object sender, DragEventArgs e)
        {
            //获得鼠标的坐标  
            Point point = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            // 按鼠标所指示的位置选择节点  
            ((TreeView)sender).SelectedNode = ((TreeView)sender).GetNodeAt(point);

            if (e.Data.GetDataPresent(typeof(DataGridViewSelectedRowCollection)))
            {
                e.Effect = DragDropEffects.Link;  //这个值会返回给DoDragDrop方法
                ///////////////////////////////////////
                // 拖放的目标节点
                TreeNode EnterNode = null;
                // 根据坐标点取得处于坐标点位置的节点
                EnterNode = ((TreeView)sender).GetNodeAt(Cursor.Position.X, Cursor.Position.Y);
                if (EnterNode != null && EnterNode.Parent != null)
                {
                    e.Effect = DragDropEffects.Move;  //这个值会返回给DoDragDrop方法
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void treeView_library_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void treeView_library_DragDrop(object sender, DragEventArgs e)
        {
            //获得释放鼠标位置的坐标  
            Point point = treeView_library.PointToClient(new Point(e.X, e.Y));
            //获得在鼠标释放处的节点  
            TreeNode targetNode = treeView_library.GetNodeAt(point);
            if (targetNode != null && targetNode.Parent != null)
            {
                if (e.Data.GetDataPresent(typeof(DataGridViewSelectedRowCollection)))
                {
                    DataGridViewSelectedRowCollection rowCollection = e.Data.GetData(typeof(DataGridViewSelectedRowCollection)) as DataGridViewSelectedRowCollection;
                    if (rowCollection != null)
                    {
                        foreach (DataGridViewRow row in rowCollection)
                        {
                            int lid = Convert.ToInt32(targetNode.Name);
                            int plid = Convert.ToInt32(targetNode.Parent.Name);
                            long rid = Convert.ToInt64(row.Cells[CTDRecordDAM.CTD_RID].Value.ToString());
                            CTDRecordDAM.updateCTCRecordLid(lid, plid, rid);
                        }
                        manager.updateLibRecCount();
                    }
                }
                manager.updateDataSet(targetNode);
            }
        }
        /// <summary>
        /// 设置Datagridview显示行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_items_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,e.RowBounds.Location.Y,dataGridView_items.RowHeadersWidth - 4,e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),dataGridView_items.RowHeadersDefaultCellStyle.Font,rectangle,
                dataGridView_items.RowHeadersDefaultCellStyle.ForeColor,TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            dataGridView_items.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }

        private void treeView_library_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            manager.noteCurSelectedNode(e.Node);
        }

        private void treeView_base_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            manager.noteCurSelectedNode(e.Node);
        }
        public TreeView getBaseTree()
        {
            return this.treeView_base;
        }
        public TreeView getLibTree()
        {
            return this.treeView_library;
        }
        public DataGridView getItemGridView()
        {
            return this.dataGridView_items;
        }
        public TabControl getAttachTabControl()
        {
            return this.tabControl_attach;
        }
        public ToolStripProgressBar getProgressBar()
        {
            return toolStripProgressBar_bottom;
        }
        private void treeView_library_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }
        private void treeView_base_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void toolStripButton_import_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel文件,mdi缓存文件,DI打包文件(*.xls,*.xlsx)|*.xls;*.xlsx;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fpath = ofd.FileName;
                bool exists = System.IO.File.Exists(fpath);
                if (exists == true)
                {
                    try
                    {
                        manager.importData(fpath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据导入失败。");
                        log.Info("数据导入失败，错误信息：",ex);
                    }
                }
            }
        }

        private void toolStripButton_export_Click(object sender, EventArgs e)
        {
            ExportTypeDlg exportDlg = new ExportTypeDlg();
            if(exportDlg.ShowDialog()==DialogResult.OK)
            {
                try
                {
                    manager.exportData(ExportTypeDlg.LastOptionValue, ExportTypeDlg.LastFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据导出失败。");
                    log.Info("数据导出失败，错误信息：",ex);
                }
            }
        }
        /// <summary>
        /// For Quick search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_qsearch_Click(object sender, EventArgs e)
        {
            string findTerm = this.toolStripTextBox_quickSearch.Text.Trim();
            manager.quickSearch(findTerm);
        }
        private void toolStripTextBox_quickSearch_Enter(object sender, EventArgs e)
        {
            this.toolStripTextBox_quickSearch.Text = "";
            //this.toolStripTextBox_quickSearch.Owner.Update();
        }

        private void toolStripTextBox_quickSearch_Leave(object sender, EventArgs e)
        {
            if (this.toolStripTextBox_quickSearch.Text.Trim().Length < 1)
                this.toolStripTextBox_quickSearch.Text = "Quick Search";
        }

        private void toolStripTextBox_quickSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string findTerm = this.toolStripTextBox_quickSearch.Text.Trim();
                manager.quickSearch(findTerm);
            }
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            if (this.textBox_search1.Text.Trim().Length > 0)
            {
                manager.doDBDataSearch();
            }
        }
        public TableLayoutPanel getDBSearchPanel()
        {
            return this.tableLayoutPanel_search;
        }
        public SplitContainer getSearchSpliter()
        {
            return splitContainer_middle;
        }
        /// <summary>
        /// 验证登录状态，并打开GCM视图页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_gcm_Click(object sender, EventArgs e)
        {
            IDCMFormManger.getInstance().activeChildView(typeof(GCMViewManager), true);
        }
        /// <summary>
        /// 云备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_download_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 云恢复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_upload_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 本地数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_refresh_Click(object sender, EventArgs e)
        {

        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

    }
}
