using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using IDCM.ViewManager;
using IDCM.Service;
using IDCM.Service.Utils;

namespace IDCM.Forms
{
    public partial class IDCMForm : Form
    {
        private IDCMFormManger manager = null;
        internal void setManager(IDCMFormManger manager)
        {
            this.manager = manager;
        }

        public IDCMForm()
        {
            InitializeComponent();
            
        }

        private void IDCMForm_Load(object sender, EventArgs e)
        {
            //Thread.CurrentThread.Name = "IDCMForm" + HandleToken.nextTempID();
        }

        private void MenuStrip_IDCM_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            ///////////////////////////////////////////////////////////
            //if (e.Item.Text.Length == 0 || e.Item.Text == "还原(&R)" || e.Item.Text == "最小化(&N)")
            //{
            //    e.Item.Visible = false;
            //}
            ////////////////////////////////////////////////////////////
            //@Deprecated 该方法可以解决图标更新显示的问题，但会导致另外的显示时间处理的问题，有待测试改进。
        }
        /// <summary>
        // 主窗口关闭事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDCMForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            manager.closeWorkSpace();
        }
        /// <summary>
        /// 打开或新建一个本地数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.reopenWorkSpace(false);
        }
        public void setLoginTip(string tip=null)
        {
            ToolStripItemAsyncUtil.SyncSetText(this.ToolStripTextBox_user, tip);
        }
        /// <summary>
        /// 打开或新建一个本地数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.reopenWorkSpace(true);
        }
        /// <summary>
        /// 关闭一个本地数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.closeWorkSpace();
        }
        /// <summary>
        /// 更新字段模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void templatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.activeTemplateView();
        }
        
        /// <summary>
        /// 身份认证信息配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void authToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.activeAuthView();
        }

        private void showBackTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskInfoDlg taskInfoDlg = new TaskInfoDlg();
            taskInfoDlg.Show();
        }
        /******************************************************************
         * 键盘事件处理方法
         * @auther JiahaiWu 2014-03-17
         ******************************************************************/
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Alt | Keys.F://打开File菜单项
                    this.ToolStripMenuItem_file.ShowDropDown();
                    break;
                case Keys.Alt | Keys.C://打开Configuration菜单项
                    this.ToolStripMenuItem_cfg.ShowDropDown();
                    break;
                case Keys.Alt | Keys.T://打开Tools菜单项
                    this.ToolStripMenuItem_tool.ShowDropDown();
                    break;
                case Keys.Alt | Keys.W://打开Window菜单项
                    this.ToolStripMenuItem_window.ShowDropDown();
                    break;
                case Keys.Alt | Keys.H://打开Help菜单项
                    this.ToolStripMenuItem_help.ShowDropDown();
                    break;
                case Keys.Control | Keys.F://打开查找菜单
                    manager.showDBDataSearch();
                    break;
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
        /// 根据HomeView和GCMView的打开状态控制菜单栏Tools选项下的菜单的可用状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_tool_DropDownOpening(object sender, EventArgs e)
        {
            ManagerI hvManager = manager.getHomeViewManager();
            ManagerI gcmManager = manager.getGCMViewManager();
            localSearchToolStripMenuItem.Enabled = false;
            frontPageSearchToolStripMenuItem.Enabled = false;
            onlineSearchToolStripMenuItem.Enabled = false;
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    localSearchToolStripMenuItem.Enabled = true;
                    frontPageSearchToolStripMenuItem.Enabled = true;
                }
            }
            if (gcmManager != null)
            {
                if (gcmManager.isActive())
                {
                    onlineSearchToolStripMenuItem.Enabled = true;
                    frontPageSearchToolStripMenuItem.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 根据HomeView打开状态控制菜单栏Configuration选项下的菜单的可用状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_cfg_DropDownOpening(object sender, EventArgs e)
        {
            ManagerI hvManager = manager.getHomeViewManager();
            templatesToolStripMenuItem.Enabled = false;
            authToolStripMenuItem.Enabled = true;
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    templatesToolStripMenuItem.Enabled = true;
                }
                else
                {
                    templatesToolStripMenuItem.Enabled = false;
                }
            }
        }

        private void localSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.showDBDataSearch();
        }

        private void onlineSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.showDBDataSearch();
        }

        private void frontPageSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.frontDataSearch();
        }
        private void aboutIDCMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutDlg aboutDlg = new AboutDlg();
            aboutDlg.ShowDialog();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.closeWorkSpace();
        }


    }
}
