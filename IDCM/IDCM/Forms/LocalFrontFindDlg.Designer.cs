namespace IDCM.Forms
{
    partial class LocalFrontFindDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocalFrontFindDlg));
            this.label_search = new System.Windows.Forms.Label();
            this.comboBox_find = new System.Windows.Forms.ComboBox();
            this.button_searchDown = new System.Windows.Forms.Button();
            this.button_findRev = new System.Windows.Forms.Button();
            this.checkBox_matchCase = new System.Windows.Forms.CheckBox();
            this.checkBox_matchAll = new System.Windows.Forms.CheckBox();
            this.button_reset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_search
            // 
            this.label_search.AutoSize = true;
            this.label_search.Location = new System.Drawing.Point(22, 31);
            this.label_search.Name = "label_search";
            this.label_search.Size = new System.Drawing.Size(47, 12);
            this.label_search.TabIndex = 0;
            this.label_search.Text = "Search:";
            // 
            // comboBox_find
            // 
            this.comboBox_find.FormattingEnabled = true;
            this.comboBox_find.Location = new System.Drawing.Point(69, 28);
            this.comboBox_find.Name = "comboBox_find";
            this.comboBox_find.Size = new System.Drawing.Size(246, 20);
            this.comboBox_find.TabIndex = 1;
            this.comboBox_find.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_find_KeyDown);
            // 
            // button_searchDown
            // 
            this.button_searchDown.Location = new System.Drawing.Point(328, 26);
            this.button_searchDown.Name = "button_searchDown";
            this.button_searchDown.Size = new System.Drawing.Size(75, 23);
            this.button_searchDown.TabIndex = 2;
            this.button_searchDown.Text = "Find Down";
            this.button_searchDown.UseVisualStyleBackColor = true;
            this.button_searchDown.Click += new System.EventHandler(this.button_searchDown_Click);
            // 
            // button_findRev
            // 
            this.button_findRev.Location = new System.Drawing.Point(329, 63);
            this.button_findRev.Name = "button_findRev";
            this.button_findRev.Size = new System.Drawing.Size(75, 23);
            this.button_findRev.TabIndex = 3;
            this.button_findRev.Text = "Find Up";
            this.button_findRev.UseVisualStyleBackColor = true;
            this.button_findRev.Click += new System.EventHandler(this.button_findRev_Click);
            // 
            // checkBox_matchCase
            // 
            this.checkBox_matchCase.AutoSize = true;
            this.checkBox_matchCase.Location = new System.Drawing.Point(27, 66);
            this.checkBox_matchCase.Name = "checkBox_matchCase";
            this.checkBox_matchCase.Size = new System.Drawing.Size(84, 16);
            this.checkBox_matchCase.TabIndex = 4;
            this.checkBox_matchCase.Text = "Match Case";
            this.checkBox_matchCase.UseVisualStyleBackColor = true;
            // 
            // checkBox_matchAll
            // 
            this.checkBox_matchAll.AutoSize = true;
            this.checkBox_matchAll.Location = new System.Drawing.Point(124, 67);
            this.checkBox_matchAll.Name = "checkBox_matchAll";
            this.checkBox_matchAll.Size = new System.Drawing.Size(90, 16);
            this.checkBox_matchAll.TabIndex = 5;
            this.checkBox_matchAll.Text = "Match Fully";
            this.checkBox_matchAll.UseVisualStyleBackColor = true;
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(240, 63);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 23);
            this.button_reset.TabIndex = 6;
            this.button_reset.Text = "Reset";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // FrontFindDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 114);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.checkBox_matchAll);
            this.Controls.Add(this.checkBox_matchCase);
            this.Controls.Add(this.button_findRev);
            this.Controls.Add(this.button_searchDown);
            this.Controls.Add(this.comboBox_find);
            this.Controls.Add(this.label_search);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrontFindDlg";
            this.Text = "Front Find";
            this.Load += new System.EventHandler(this.FrontFindDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_search;
        private System.Windows.Forms.ComboBox comboBox_find;
        private System.Windows.Forms.Button button_searchDown;
        private System.Windows.Forms.Button button_findRev;
        private System.Windows.Forms.CheckBox checkBox_matchCase;
        private System.Windows.Forms.CheckBox checkBox_matchAll;
        private System.Windows.Forms.Button button_reset;
    }
}