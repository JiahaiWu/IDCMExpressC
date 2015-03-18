namespace IDCM.Forms
{
    partial class AboutDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDlg));
            this.button1 = new System.Windows.Forms.Button();
            this.version = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.WFCC = new System.Windows.Forms.Label();
            this.contact = new System.Windows.Forms.Label();
            this.address = new System.Windows.Forms.Label();
            this.email = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(100, 284);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // version
            // 
            this.version.AutoSize = true;
            this.version.Location = new System.Drawing.Point(39, 87);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(41, 12);
            this.version.TabIndex = 1;
            this.version.Text = "lable1";
            this.version.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(73, 148);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(131, 12);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.wfcc.info/";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::IDCM.Properties.Resources.gcm_logo;
            this.pictureBox1.Location = new System.Drawing.Point(100, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 63);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // WFCC
            // 
            this.WFCC.AutoSize = true;
            this.WFCC.Location = new System.Drawing.Point(32, 148);
            this.WFCC.Name = "WFCC";
            this.WFCC.Size = new System.Drawing.Size(35, 12);
            this.WFCC.TabIndex = 4;
            this.WFCC.Text = "WFCC:";
            // 
            // contact
            // 
            this.contact.AutoSize = true;
            this.contact.Location = new System.Drawing.Point(73, 179);
            this.contact.Name = "contact";
            this.contact.Size = new System.Drawing.Size(41, 12);
            this.contact.TabIndex = 5;
            this.contact.Text = "label1";
            this.contact.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // address
            // 
            this.address.AutoSize = true;
            this.address.Location = new System.Drawing.Point(42, 238);
            this.address.Name = "address";
            this.address.Size = new System.Drawing.Size(41, 12);
            this.address.TabIndex = 6;
            this.address.Text = "label1";
            this.address.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.Location = new System.Drawing.Point(63, 211);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(41, 12);
            this.email.TabIndex = 7;
            this.email.Text = "label1";
            this.email.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AboutDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 344);
            this.Controls.Add(this.email);
            this.Controls.Add(this.address);
            this.Controls.Add(this.contact);
            this.Controls.Add(this.WFCC);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.version);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutIDCM";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label version;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label WFCC;
        private System.Windows.Forms.Label contact;
        private System.Windows.Forms.Label address;
        private System.Windows.Forms.Label email;
    }
}