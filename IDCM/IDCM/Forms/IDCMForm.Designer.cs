namespace IDCM.Forms
{
    partial class IDCMForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDCMForm));
            this.MenuStrip_IDCM = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem_file = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_cfg = new System.Windows.Forms.ToolStripMenuItem();
            this.templatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.authToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_tool = new System.Windows.Forms.ToolStripMenuItem();
            this.localSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontPageSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_window = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showConsoleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.showBackTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_help = new System.Windows.Forms.ToolStripMenuItem();
            this.webSupportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutIDCMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutIDCMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripTextBox_user = new System.Windows.Forms.ToolStripTextBox();
            this.MenuStrip_IDCM.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip_IDCM
            // 
            this.MenuStrip_IDCM.BackColor = System.Drawing.Color.LightBlue;
            this.MenuStrip_IDCM.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MenuStrip_IDCM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_file,
            this.ToolStripMenuItem_cfg,
            this.ToolStripMenuItem_tool,
            this.ToolStripMenuItem_window,
            this.ToolStripMenuItem_help,
            this.ToolStripTextBox_user});
            this.MenuStrip_IDCM.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip_IDCM.Name = "MenuStrip_IDCM";
            this.MenuStrip_IDCM.Size = new System.Drawing.Size(1008, 27);
            this.MenuStrip_IDCM.TabIndex = 1;
            this.MenuStrip_IDCM.Text = "MenuStrip_IDCM";
            this.MenuStrip_IDCM.ItemAdded += new System.Windows.Forms.ToolStripItemEventHandler(this.MenuStrip_IDCM_ItemAdded);
            // 
            // ToolStripMenuItem_file
            // 
            this.ToolStripMenuItem_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.ToolStripMenuItem_file.Name = "ToolStripMenuItem_file";
            this.ToolStripMenuItem_file.Size = new System.Drawing.Size(57, 23);
            this.ToolStripMenuItem_file.Text = "File (F)";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New...";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_cfg
            // 
            this.ToolStripMenuItem_cfg.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.templatesToolStripMenuItem,
            this.authToolStripMenuItem});
            this.ToolStripMenuItem_cfg.Name = "ToolStripMenuItem_cfg";
            this.ToolStripMenuItem_cfg.Size = new System.Drawing.Size(119, 23);
            this.ToolStripMenuItem_cfg.Text = "Configuration (C)";
            this.ToolStripMenuItem_cfg.DropDownOpening += new System.EventHandler(this.ToolStripMenuItem_cfg_DropDownOpening);
            // 
            // templatesToolStripMenuItem
            // 
            this.templatesToolStripMenuItem.Name = "templatesToolStripMenuItem";
            this.templatesToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.templatesToolStripMenuItem.Text = "Templates";
            this.templatesToolStripMenuItem.Click += new System.EventHandler(this.templatesToolStripMenuItem_Click);
            // 
            // authToolStripMenuItem
            // 
            this.authToolStripMenuItem.Name = "authToolStripMenuItem";
            this.authToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.authToolStripMenuItem.Text = "Authentication";
            this.authToolStripMenuItem.Click += new System.EventHandler(this.authToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_tool
            // 
            this.ToolStripMenuItem_tool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localSearchToolStripMenuItem,
            this.onlineSearchToolStripMenuItem,
            this.frontPageSearchToolStripMenuItem});
            this.ToolStripMenuItem_tool.Name = "ToolStripMenuItem_tool";
            this.ToolStripMenuItem_tool.Size = new System.Drawing.Size(71, 23);
            this.ToolStripMenuItem_tool.Text = "Tools (T)";
            this.ToolStripMenuItem_tool.DropDownOpening += new System.EventHandler(this.ToolStripMenuItem_tool_DropDownOpening);
            // 
            // localSearchToolStripMenuItem
            // 
            this.localSearchToolStripMenuItem.Name = "localSearchToolStripMenuItem";
            this.localSearchToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.localSearchToolStripMenuItem.Text = "Local Search";
            this.localSearchToolStripMenuItem.Click += new System.EventHandler(this.localSearchToolStripMenuItem_Click);
            // 
            // onlineSearchToolStripMenuItem
            // 
            this.onlineSearchToolStripMenuItem.Name = "onlineSearchToolStripMenuItem";
            this.onlineSearchToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.onlineSearchToolStripMenuItem.Text = "Online Search";
            this.onlineSearchToolStripMenuItem.Click += new System.EventHandler(this.onlineSearchToolStripMenuItem_Click);
            // 
            // frontPageSearchToolStripMenuItem
            // 
            this.frontPageSearchToolStripMenuItem.Name = "frontPageSearchToolStripMenuItem";
            this.frontPageSearchToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.frontPageSearchToolStripMenuItem.Text = "Front Page Search";
            this.frontPageSearchToolStripMenuItem.Click += new System.EventHandler(this.frontPageSearchToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_window
            // 
            this.ToolStripMenuItem_window.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeAllToolStripMenuItem,
            this.showConsoleToolStripMenuItem1,
            this.showBackTaskToolStripMenuItem});
            this.ToolStripMenuItem_window.Name = "ToolStripMenuItem_window";
            this.ToolStripMenuItem_window.Size = new System.Drawing.Size(91, 23);
            this.ToolStripMenuItem_window.Text = "Window (W)";
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.closeAllToolStripMenuItem.Text = "Close All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // showConsoleToolStripMenuItem1
            // 
            this.showConsoleToolStripMenuItem1.Name = "showConsoleToolStripMenuItem1";
            this.showConsoleToolStripMenuItem1.Size = new System.Drawing.Size(176, 22);
            this.showConsoleToolStripMenuItem1.Text = "Show Console";
            // 
            // showBackTaskToolStripMenuItem
            // 
            this.showBackTaskToolStripMenuItem.Name = "showBackTaskToolStripMenuItem";
            this.showBackTaskToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.showBackTaskToolStripMenuItem.Text = "Show Back Tasks";
            this.showBackTaskToolStripMenuItem.Click += new System.EventHandler(this.showBackTaskToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_help
            // 
            this.ToolStripMenuItem_help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.webSupportToolStripMenuItem,
            this.aboutIDCMToolStripMenuItem,
            this.aboutIDCMToolStripMenuItem1});
            this.ToolStripMenuItem_help.Name = "ToolStripMenuItem_help";
            this.ToolStripMenuItem_help.Size = new System.Drawing.Size(68, 23);
            this.ToolStripMenuItem_help.Text = "Help (H)";
            // 
            // webSupportToolStripMenuItem
            // 
            this.webSupportToolStripMenuItem.Name = "webSupportToolStripMenuItem";
            this.webSupportToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.webSupportToolStripMenuItem.Text = "Web Support";
            // 
            // aboutIDCMToolStripMenuItem
            // 
            this.aboutIDCMToolStripMenuItem.Name = "aboutIDCMToolStripMenuItem";
            this.aboutIDCMToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.aboutIDCMToolStripMenuItem.Text = "Show Getting Start Dialog";
            // 
            // aboutIDCMToolStripMenuItem1
            // 
            this.aboutIDCMToolStripMenuItem1.Name = "aboutIDCMToolStripMenuItem1";
            this.aboutIDCMToolStripMenuItem1.Size = new System.Drawing.Size(226, 22);
            this.aboutIDCMToolStripMenuItem1.Text = "About IDCM";
            this.aboutIDCMToolStripMenuItem1.Click += new System.EventHandler(this.aboutIDCMToolStripMenuItem1_Click);
            // 
            // ToolStripTextBox_user
            // 
            this.ToolStripTextBox_user.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolStripTextBox_user.BackColor = System.Drawing.Color.LightBlue;
            this.ToolStripTextBox_user.ForeColor = System.Drawing.Color.RoyalBlue;
            this.ToolStripTextBox_user.Name = "ToolStripTextBox_user";
            this.ToolStripTextBox_user.Padding = new System.Windows.Forms.Padding(0, 0, 30, 0);
            this.ToolStripTextBox_user.ReadOnly = true;
            this.ToolStripTextBox_user.ShortcutsEnabled = false;
            this.ToolStripTextBox_user.Size = new System.Drawing.Size(160, 23);
            this.ToolStripTextBox_user.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IDCMForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 612);
            this.Controls.Add(this.MenuStrip_IDCM);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MenuStrip_IDCM;
            this.Name = "IDCMForm";
            this.Text = "IDCM";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.IDCMForm_FormClosed);
            this.Load += new System.EventHandler(this.IDCMForm_Load);
            this.MenuStrip_IDCM.ResumeLayout(false);
            this.MenuStrip_IDCM.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip_IDCM;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_file;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_tool;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onlineSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_help;
        private System.Windows.Forms.ToolStripMenuItem webSupportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutIDCMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutIDCMToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_cfg;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_window;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showConsoleToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem templatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem authToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frontPageSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBackTaskToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox ToolStripTextBox_user;
    }
}

