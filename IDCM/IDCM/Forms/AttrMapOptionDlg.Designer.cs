namespace IDCM.Forms
{
    partial class AttrMapOptionDlg
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttrMapOptionDlg));
            this.dataGridView_map = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_confirm = new System.Windows.Forms.Button();
            this.radioButton_custom = new System.Windows.Forms.RadioButton();
            this.radioButton_exact = new System.Windows.Forms.RadioButton();
            this.radioButton_similarity = new System.Windows.Forms.RadioButton();
            this.button_cancel = new System.Windows.Forms.Button();
            this.contextMenuStrip_destList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripComboBox_dest = new System.Windows.Forms.ToolStripComboBox();
            this.Column_src = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_alter = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column_tag = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column_dest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_UnBound = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_map)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip_destList.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_map
            // 
            this.dataGridView_map.AllowUserToAddRows = false;
            this.dataGridView_map.AllowUserToDeleteRows = false;
            this.dataGridView_map.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_map.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_src,
            this.Column_alter,
            this.Column_tag,
            this.Column_dest,
            this.Column_UnBound});
            this.dataGridView_map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_map.Location = new System.Drawing.Point(3, 83);
            this.dataGridView_map.Name = "dataGridView_map";
            this.dataGridView_map.ReadOnly = true;
            this.dataGridView_map.RowTemplate.Height = 23;
            this.dataGridView_map.Size = new System.Drawing.Size(710, 429);
            this.dataGridView_map.TabIndex = 0;
            this.dataGridView_map.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_map_CellMouseClick);
            this.dataGridView_map.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_map_RowPostPaint);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView_map, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(716, 515);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(710, 74);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_confirm);
            this.groupBox1.Controls.Add(this.radioButton_custom);
            this.groupBox1.Controls.Add(this.radioButton_exact);
            this.groupBox1.Controls.Add(this.radioButton_similarity);
            this.groupBox1.Controls.Add(this.button_cancel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 74);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // button_confirm
            // 
            this.button_confirm.Location = new System.Drawing.Point(581, 30);
            this.button_confirm.Name = "button_confirm";
            this.button_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_confirm.TabIndex = 4;
            this.button_confirm.Text = "Confirm";
            this.button_confirm.UseVisualStyleBackColor = true;
            this.button_confirm.Click += new System.EventHandler(this.button_confirm_Click);
            // 
            // radioButton_custom
            // 
            this.radioButton_custom.AutoSize = true;
            this.radioButton_custom.Enabled = false;
            this.radioButton_custom.Location = new System.Drawing.Point(269, 33);
            this.radioButton_custom.Name = "radioButton_custom";
            this.radioButton_custom.Size = new System.Drawing.Size(107, 16);
            this.radioButton_custom.TabIndex = 3;
            this.radioButton_custom.Text = "Custom Mapping";
            this.radioButton_custom.UseVisualStyleBackColor = true;
            // 
            // radioButton_exact
            // 
            this.radioButton_exact.AutoSize = true;
            this.radioButton_exact.Location = new System.Drawing.Point(163, 33);
            this.radioButton_exact.Name = "radioButton_exact";
            this.radioButton_exact.Size = new System.Drawing.Size(89, 16);
            this.radioButton_exact.TabIndex = 2;
            this.radioButton_exact.Text = "Exact Match";
            this.radioButton_exact.UseVisualStyleBackColor = true;
            this.radioButton_exact.CheckedChanged += new System.EventHandler(this.radioButton_exact_CheckedChanged);
            // 
            // radioButton_similarity
            // 
            this.radioButton_similarity.AutoSize = true;
            this.radioButton_similarity.Location = new System.Drawing.Point(27, 33);
            this.radioButton_similarity.Name = "radioButton_similarity";
            this.radioButton_similarity.Size = new System.Drawing.Size(119, 16);
            this.radioButton_similarity.TabIndex = 1;
            this.radioButton_similarity.Text = "Similarity Match";
            this.radioButton_similarity.UseVisualStyleBackColor = true;
            this.radioButton_similarity.CheckedChanged += new System.EventHandler(this.radioButton_similarity_CheckedChanged);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(471, 30);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 0;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // contextMenuStrip_destList
            // 
            this.contextMenuStrip_destList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox_dest});
            this.contextMenuStrip_destList.Name = "contextMenuStrip_destList";
            this.contextMenuStrip_destList.Size = new System.Drawing.Size(182, 51);
            // 
            // toolStripComboBox_dest
            // 
            this.toolStripComboBox_dest.BackColor = System.Drawing.Color.LightSteelBlue;
            this.toolStripComboBox_dest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox_dest.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripComboBox_dest.Name = "toolStripComboBox_dest";
            this.toolStripComboBox_dest.Size = new System.Drawing.Size(121, 25);
            // 
            // Column_src
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_src.DefaultCellStyle = dataGridViewCellStyle15;
            this.Column_src.HeaderText = "From Attr";
            this.Column_src.Name = "Column_src";
            this.Column_src.ReadOnly = true;
            this.Column_src.Width = 150;
            // 
            // Column_alter
            // 
            this.Column_alter.HeaderText = "Config";
            this.Column_alter.Name = "Column_alter";
            this.Column_alter.ReadOnly = true;
            this.Column_alter.Text = "...";
            this.Column_alter.ToolTipText = "...";
            this.Column_alter.UseColumnTextForButtonValue = true;
            this.Column_alter.Width = 60;
            // 
            // Column_tag
            // 
            this.Column_tag.HeaderText = "To";
            this.Column_tag.Image = global::IDCM.Properties.Resources.rightArrow;
            this.Column_tag.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Column_tag.Name = "Column_tag";
            this.Column_tag.ReadOnly = true;
            // 
            // Column_dest
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_dest.DefaultCellStyle = dataGridViewCellStyle16;
            this.Column_dest.HeaderText = "Dest Attr";
            this.Column_dest.Name = "Column_dest";
            this.Column_dest.ReadOnly = true;
            this.Column_dest.Width = 150;
            // 
            // Column_UnBound
            // 
            this.Column_UnBound.HeaderText = "Unbound";
            this.Column_UnBound.Image = global::IDCM.Properties.Resources.del_note;
            this.Column_UnBound.Name = "Column_UnBound";
            this.Column_UnBound.ReadOnly = true;
            // 
            // AttrMapOptionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 515);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AttrMapOptionDlg";
            this.Text = "AttrMappingOptionDlg";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_map)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip_destList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_map;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_confirm;
        private System.Windows.Forms.RadioButton radioButton_custom;
        private System.Windows.Forms.RadioButton radioButton_exact;
        private System.Windows.Forms.RadioButton radioButton_similarity;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_destList;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox_dest;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_src;
        private System.Windows.Forms.DataGridViewButtonColumn Column_alter;
        private System.Windows.Forms.DataGridViewImageColumn Column_tag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_dest;
        private System.Windows.Forms.DataGridViewImageColumn Column_UnBound;

    }
}