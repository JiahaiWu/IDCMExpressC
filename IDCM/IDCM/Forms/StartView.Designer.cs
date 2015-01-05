namespace IDCM.Forms
{
    partial class StartView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartView));
            this.pictureBox_start = new System.Windows.Forms.PictureBox();
            this.panel_start = new System.Windows.Forms.Panel();
            this.checkBox_defaultWS = new System.Windows.Forms.CheckBox();
            this.checkBox_remember = new System.Windows.Forms.CheckBox();
            this.button_confirm = new System.Windows.Forms.Button();
            this.textBox_pwd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_loginName = new System.Windows.Forms.TextBox();
            this.label_user = new System.Windows.Forms.Label();
            this.textBox_datasource = new System.Windows.Forms.TextBox();
            this.label_dataSource = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_download = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_start)).BeginInit();
            this.panel_start.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_start
            // 
            this.pictureBox_start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_start.Image = global::IDCM.Properties.Resources.initView;
            this.pictureBox_start.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_start.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox_start.Name = "pictureBox_start";
            this.pictureBox_start.Size = new System.Drawing.Size(630, 440);
            this.pictureBox_start.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_start.TabIndex = 0;
            this.pictureBox_start.TabStop = false;
            // 
            // panel_start
            // 
            this.panel_start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_start.BackColor = System.Drawing.Color.Transparent;
            this.panel_start.Controls.Add(this.button_download);
            this.panel_start.Controls.Add(this.button_cancel);
            this.panel_start.Controls.Add(this.checkBox_defaultWS);
            this.panel_start.Controls.Add(this.checkBox_remember);
            this.panel_start.Controls.Add(this.button_confirm);
            this.panel_start.Controls.Add(this.textBox_pwd);
            this.panel_start.Controls.Add(this.label1);
            this.panel_start.Controls.Add(this.textBox_loginName);
            this.panel_start.Controls.Add(this.label_user);
            this.panel_start.Controls.Add(this.textBox_datasource);
            this.panel_start.Controls.Add(this.label_dataSource);
            this.panel_start.Location = new System.Drawing.Point(12, 279);
            this.panel_start.Name = "panel_start";
            this.panel_start.Size = new System.Drawing.Size(606, 100);
            this.panel_start.TabIndex = 1;
            // 
            // checkBox_defaultWS
            // 
            this.checkBox_defaultWS.AutoSize = true;
            this.checkBox_defaultWS.Location = new System.Drawing.Point(393, 14);
            this.checkBox_defaultWS.Name = "checkBox_defaultWS";
            this.checkBox_defaultWS.Size = new System.Drawing.Size(84, 16);
            this.checkBox_defaultWS.TabIndex = 8;
            this.checkBox_defaultWS.Text = "As Default";
            this.checkBox_defaultWS.UseVisualStyleBackColor = true;
            // 
            // checkBox_remember
            // 
            this.checkBox_remember.AutoSize = true;
            this.checkBox_remember.Checked = true;
            this.checkBox_remember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_remember.Location = new System.Drawing.Point(293, 72);
            this.checkBox_remember.Name = "checkBox_remember";
            this.checkBox_remember.Size = new System.Drawing.Size(126, 16);
            this.checkBox_remember.TabIndex = 7;
            this.checkBox_remember.Text = "Remember Password";
            this.checkBox_remember.UseVisualStyleBackColor = true;
            // 
            // button_confirm
            // 
            this.button_confirm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_confirm.Location = new System.Drawing.Point(509, 69);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 21);
            this.button_confirm.TabIndex = 6;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // textBox_pwd
            // 
            this.textBox_pwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_pwd.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBox_pwd.Location = new System.Drawing.Point(110, 69);
            this.textBox_pwd.Name = "textBox_pwd";
            this.textBox_pwd.ShortcutsEnabled = false;
            this.textBox_pwd.Size = new System.Drawing.Size(176, 21);
            this.textBox_pwd.TabIndex = 5;
            this.textBox_pwd.Tag = "Optional Password For GCM View ";
            this.textBox_pwd.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "GCM Password:";
            // 
            // textBox_loginName
            // 
            this.textBox_loginName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_loginName.Location = new System.Drawing.Point(110, 40);
            this.textBox_loginName.Name = "textBox_loginName";
            this.textBox_loginName.Size = new System.Drawing.Size(177, 21);
            this.textBox_loginName.TabIndex = 3;
            // 
            // label_user
            // 
            this.label_user.AutoSize = true;
            this.label_user.Location = new System.Drawing.Point(42, 44);
            this.label_user.Name = "label_user";
            this.label_user.Size = new System.Drawing.Size(65, 12);
            this.label_user.TabIndex = 2;
            this.label_user.Text = "LoginName:";
            // 
            // textBox_datasource
            // 
            this.textBox_datasource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_datasource.Location = new System.Drawing.Point(111, 11);
            this.textBox_datasource.Name = "textBox_datasource";
            this.textBox_datasource.Size = new System.Drawing.Size(270, 21);
            this.textBox_datasource.TabIndex = 1;
            // 
            // label_dataSource
            // 
            this.label_dataSource.AutoSize = true;
            this.label_dataSource.Location = new System.Drawing.Point(36, 16);
            this.label_dataSource.Name = "label_dataSource";
            this.label_dataSource.Size = new System.Drawing.Size(71, 12);
            this.label_dataSource.TabIndex = 0;
            this.label_dataSource.Text = "DataSource:";
            // 
            // button_cancel
            // 
            this.button_cancel.BackColor = System.Drawing.Color.Transparent;
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_cancel.Location = new System.Drawing.Point(509, 10);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 9;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = false;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_download
            // 
            this.button_download.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_download.Location = new System.Drawing.Point(509, 40);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(75, 23);
            this.button_download.TabIndex = 10;
            this.button_download.Text = "Download";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // StartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 440);
            this.Controls.Add(this.panel_start);
            this.Controls.Add(this.pictureBox_start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StartView";
            this.Shown += new System.EventHandler(this.StartView_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_start)).EndInit();
            this.panel_start.ResumeLayout(false);
            this.panel_start.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_start;
        private System.Windows.Forms.Panel panel_start;
        private System.Windows.Forms.Label label_dataSource;
        private System.Windows.Forms.Label label_user;
        private System.Windows.Forms.TextBox textBox_datasource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_loginName;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.TextBox textBox_pwd;
        private System.Windows.Forms.CheckBox checkBox_remember;
        private System.Windows.Forms.CheckBox checkBox_defaultWS;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_download;
    }
}