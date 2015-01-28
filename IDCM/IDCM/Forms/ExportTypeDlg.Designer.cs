namespace IDCM.Forms
{
    partial class ExportTypeDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportTypeDlg));
            this.radioButton_excel = new System.Windows.Forms.RadioButton();
            this.radioButton_json = new System.Windows.Forms.RadioButton();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_confirm = new System.Windows.Forms.Button();
            this.groupBox_filetype = new System.Windows.Forms.GroupBox();
            this.radioButton_tsv = new System.Windows.Forms.RadioButton();
            this.radioButton_csv = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.radioButton_xml = new System.Windows.Forms.RadioButton();
            this.groupBox_filetype.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton_excel
            // 
            this.radioButton_excel.AutoSize = true;
            this.radioButton_excel.Checked = true;
            this.radioButton_excel.Location = new System.Drawing.Point(22, 26);
            this.radioButton_excel.Name = "radioButton_excel";
            this.radioButton_excel.Size = new System.Drawing.Size(101, 16);
            this.radioButton_excel.TabIndex = 0;
            this.radioButton_excel.TabStop = true;
            this.radioButton_excel.Text = "Excel (.xlsx)";
            this.radioButton_excel.UseVisualStyleBackColor = true;
            // 
            // radioButton_json
            // 
            this.radioButton_json.AutoSize = true;
            this.radioButton_json.Location = new System.Drawing.Point(129, 26);
            this.radioButton_json.Name = "radioButton_json";
            this.radioButton_json.Size = new System.Drawing.Size(119, 16);
            this.radioButton_json.TabIndex = 1;
            this.radioButton_json.Text = "JSON List (.jso)";
            this.radioButton_json.UseVisualStyleBackColor = true;
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(79, 146);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(274, 146);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 3;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // groupBox_filetype
            // 
            this.groupBox_filetype.Controls.Add(this.radioButton_xml);
            this.groupBox_filetype.Controls.Add(this.radioButton_tsv);
            this.groupBox_filetype.Controls.Add(this.radioButton_csv);
            this.groupBox_filetype.Controls.Add(this.radioButton_json);
            this.groupBox_filetype.Controls.Add(this.radioButton_excel);
            this.groupBox_filetype.Location = new System.Drawing.Point(12, 3);
            this.groupBox_filetype.Name = "groupBox_filetype";
            this.groupBox_filetype.Size = new System.Drawing.Size(463, 93);
            this.groupBox_filetype.TabIndex = 4;
            this.groupBox_filetype.TabStop = false;
            // 
            // radioButton_tsv
            // 
            this.radioButton_tsv.AutoSize = true;
            this.radioButton_tsv.Location = new System.Drawing.Point(355, 26);
            this.radioButton_tsv.Name = "radioButton_tsv";
            this.radioButton_tsv.Size = new System.Drawing.Size(83, 16);
            this.radioButton_tsv.TabIndex = 3;
            this.radioButton_tsv.TabStop = true;
            this.radioButton_tsv.Text = "TSV (.tsv)";
            this.radioButton_tsv.UseVisualStyleBackColor = true;
            // 
            // radioButton_csv
            // 
            this.radioButton_csv.AutoSize = true;
            this.radioButton_csv.Location = new System.Drawing.Point(254, 26);
            this.radioButton_csv.Name = "radioButton_csv";
            this.radioButton_csv.Size = new System.Drawing.Size(83, 16);
            this.radioButton_csv.TabIndex = 2;
            this.radioButton_csv.TabStop = true;
            this.radioButton_csv.Text = "CSV (.csv)";
            this.radioButton_csv.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "SavePath:";
            // 
            // textBox_path
            // 
            this.textBox_path.Location = new System.Drawing.Point(79, 102);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.Size = new System.Drawing.Size(371, 21);
            this.textBox_path.TabIndex = 6;
            this.textBox_path.Text = "C:\\idcm_export";
            this.textBox_path.DoubleClick += new System.EventHandler(this.textBox_path_DoubleClick);
            // 
            // radioButton_xml
            // 
            this.radioButton_xml.AutoSize = true;
            this.radioButton_xml.Location = new System.Drawing.Point(22, 61);
            this.radioButton_xml.Name = "radioButton_xml";
            this.radioButton_xml.Size = new System.Drawing.Size(83, 16);
            this.radioButton_xml.TabIndex = 4;
            this.radioButton_xml.TabStop = true;
            this.radioButton_xml.Text = "XML (.xml)";
            this.radioButton_xml.UseVisualStyleBackColor = true;
            // 
            // ExportTypeDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 196);
            this.ControlBox = false;
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox_filetype);
            this.Controls.Add(this.button_confirm);
            this.Controls.Add(this.button_cancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportTypeDlg";
            this.ShowInTaskbar = false;
            this.Text = "Select File Type For Export";
            this.groupBox_filetype.ResumeLayout(false);
            this.groupBox_filetype.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_excel;
        private System.Windows.Forms.RadioButton radioButton_json;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.GroupBox groupBox_filetype;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.RadioButton radioButton_tsv;
        private System.Windows.Forms.RadioButton radioButton_csv;
        private System.Windows.Forms.RadioButton radioButton_xml;
    }
}