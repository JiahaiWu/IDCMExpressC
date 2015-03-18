namespace IDCM.Forms
{
    partial class LoginStatusDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginStatusDlg));
            this.linkLabel_uname = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label_timeTag = new System.Windows.Forms.Label();
            this.button_singout = new System.Windows.Forms.Button();
            this.label_noteSite = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkLabel_uname
            // 
            this.linkLabel_uname.AutoSize = true;
            this.linkLabel_uname.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel_uname.Location = new System.Drawing.Point(60, 66);
            this.linkLabel_uname.Name = "linkLabel_uname";
            this.linkLabel_uname.Size = new System.Drawing.Size(253, 29);
            this.linkLabel_uname.TabIndex = 0;
            this.linkLabel_uname.TabStop = true;
            this.linkLabel_uname.Text = "XXXXXXXXXXXXXXXX";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(55, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Last Sign In Time:";
            // 
            // label_timeTag
            // 
            this.label_timeTag.AutoSize = true;
            this.label_timeTag.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_timeTag.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label_timeTag.Location = new System.Drawing.Point(213, 175);
            this.label_timeTag.Name = "label_timeTag";
            this.label_timeTag.Size = new System.Drawing.Size(72, 16);
            this.label_timeTag.TabIndex = 2;
            this.label_timeTag.Text = "00:00:00";
            // 
            // button_singout
            // 
            this.button_singout.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_singout.Location = new System.Drawing.Point(176, 218);
            this.button_singout.Name = "button_singout";
            this.button_singout.Size = new System.Drawing.Size(82, 32);
            this.button_singout.TabIndex = 3;
            this.button_singout.Text = "Sign out";
            this.button_singout.UseVisualStyleBackColor = true;
            this.button_singout.Click += new System.EventHandler(this.button_singout_Click);
            // 
            // label_noteSite
            // 
            this.label_noteSite.AutoSize = true;
            this.label_noteSite.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_noteSite.Location = new System.Drawing.Point(55, 142);
            this.label_noteSite.Name = "label_noteSite";
            this.label_noteSite.Size = new System.Drawing.Size(88, 16);
            this.label_noteSite.TabIndex = 4;
            this.label_noteSite.Text = "Sign Site:";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel2.Location = new System.Drawing.Point(138, 142);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(176, 16);
            this.linkLabel2.TabIndex = 5;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "http://gcm.wfcc.info/";
            // 
            // ok
            // 
            this.ok.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ok.Location = new System.Drawing.Point(279, 218);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(82, 32);
            this.ok.TabIndex = 6;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // LoginStatusDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 278);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.label_noteSite);
            this.Controls.Add(this.button_singout);
            this.Controls.Add(this.label_timeTag);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel_uname);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginStatusDlg";
            this.Text = "Login Status";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel_uname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_timeTag;
        private System.Windows.Forms.Button button_singout;
        private System.Windows.Forms.Label label_noteSite;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Button ok;
    }
}