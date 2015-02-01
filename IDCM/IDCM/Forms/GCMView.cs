using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.ViewManager;
using IDCM.Service.Utils;
using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.UIM;

namespace IDCM.Forms
{
    public partial class GCMView : Form
    {
        public GCMView()
        {
            InitializeComponent();
            this.splitContainer_left.Panel1Collapsed = true;
            dataGridView_items.AllowDrop = true;
            dataGridView_items.AllowUserToAddRows = false;
            dataGridView_items.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView_items.EditMode = DataGridViewEditMode.EditOnKeystroke;
        }

        private GCMViewManager manager=null;

        public ToolStripProgressBar getProgressBar()
        {
            return this.toolStripProgressBar_request;
        }
        public DataGridView getItemGridView()
        {
            return this.dataGridView_items;
        }
        public SplitContainer getRightSpliterContainer()
        {
            return this.splitContainer_right;
        }
        public SplitContainer getMainSpliterContainer()
        {
            return this.splitContainer_main;
        }
        public SplitContainer getLeftSpliterContainer()
        {
            return this.splitContainer_left;
        }
        public TreeView getRecordTree()
        {
            return this.treeView_record;
        }
        public ListView getRecordList()
        {
            return this.listView_record;
        }
        public void setManager(GCMViewManager manager)
        {
            this.manager=manager;
        }
        public TableLayoutPanel getSearchPanel()
        {
            return this.tableLayoutPanel_search;
        }
        public SplitContainer getSearchSpliter()
        {
            return this.splitContainer_left;
        }
        /////////////////////////////////////
        private void GCMView_Shown(object sender, EventArgs e)
        {
            BackProgressIndicator.addIndicatorBar(this.getProgressBar());
        }

        private void toolStripButton_local_Click(object sender, EventArgs e)
        {
            manager.activeGCMView();
        }

        private void toolStripButton_down_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton_refresh_Click(object sender, EventArgs e)
        {
            manager.gcmDataRefresh();
        }

        private void toolStripButton_search_Click(object sender, EventArgs e)
        {
            string findTerm = this.toolStripTextBox_search.Text.Trim();
            manager.quickSearch(findTerm);
        }

        private void toolStripButton_help_Click(object sender, EventArgs e)
        {
            manager.requestHelp();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {

        }

        private void btn_options_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 设置Datagridview显示行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_items_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, this.dataGridView_items.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), this.dataGridView_items.RowHeadersDefaultCellStyle.Font, rectangle,
                this.dataGridView_items.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            this.dataGridView_items.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ExportTypeDlg exportDlg = new ExportTypeDlg();
            exportDlg.setCheckBoxVisible(true);
            if (exportDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    manager.exportData(ExportTypeDlg.LastOptionValue, ExportTypeDlg.LastFilePath,ExportTypeDlg.ExportStainTree);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据导出失败。",ex.Message);
                    log.Info("数据导出失败，错误信息：", ex);
                }
            }
        }

        

        private void copyCtrlCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.CopyClipboard();
            DataObject d = dataGridView_items.GetClipboardContent();
            Clipboard.SetDataObject(d);
        }

        private void pasteCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.PasteClipboard();
        }

        /******************************************************************
         * 键盘事件处理方法
         * @auther JiahaiWu 2014-03-17
         ******************************************************************/
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                //case Keys.Control | Keys.F://打开查找菜单
                //    manager.showDBDataSearch();
                //    break;
                case Keys.Control | Keys.Shift | Keys.F://打开前端记录查找菜单
                    manager.frontDataSearch();
                    break;
                case Keys.Control | Keys.Shift | Keys.N:
                    manager.frontSearchNext();
                    break;
                case Keys.Control | Keys.Shift | Keys.P:
                    manager.frontSearchPrev();
                    break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        /// <summary>
        /// 绑定剪贴板复制粘贴的快捷键处理Ctrl+C Ctrl+V Shift+Delete 及 Shift+Insert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_items_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.KeyCode == Keys.C) || (e.Shift && e.KeyCode == Keys.Delete))
            {
                manager.CopyClipboard();
            }
            if ((e.Control && e.KeyCode == Keys.V) || (e.Shift && e.KeyCode == Keys.Insert))
            {
                manager.PasteClipboard();
            }
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        private void GCMView_FormClosed(object sender, FormClosedEventArgs e)
        {
            BackProgressIndicator.removeIndicatorBar(this.getProgressBar());
        }
    }
}
