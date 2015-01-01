namespace IDCM.Forms
{
    partial class LibFieldSettingView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibFieldSettingView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView_fields = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip_field = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_copy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_merge = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_up = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_down = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_del = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_check = new System.Windows.Forms.Button();
            this.button_submit = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.comboBox_templ = new System.Windows.Forms.ComboBox();
            this.label_template = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.attr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attrType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isUnique = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.defaultVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.restrict = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainField = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.comments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewImageColumn();
            this.Up = new System.Windows.Forms.DataGridViewImageColumn();
            this.Down = new System.Windows.Forms.DataGridViewImageColumn();
            this.Reuse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Overwrite = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_fields)).BeginInit();
            this.contextMenuStrip_field.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView_fields);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(6, 56);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(985, 441);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fields Configuration";
            // 
            // dataGridView_fields
            // 
            this.dataGridView_fields.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dataGridView_fields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_fields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.attr,
            this.attrType,
            this.isUnique,
            this.defaultVal,
            this.restrict,
            this.mainField,
            this.comments,
            this.Delete,
            this.Up,
            this.Down,
            this.Reuse,
            this.Overwrite});
            this.dataGridView_fields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_fields.Location = new System.Drawing.Point(3, 19);
            this.dataGridView_fields.Name = "dataGridView_fields";
            this.dataGridView_fields.RowTemplate.Height = 23;
            this.dataGridView_fields.Size = new System.Drawing.Size(979, 419);
            this.dataGridView_fields.TabIndex = 0;
            this.dataGridView_fields.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_fields_CellClick);
            this.dataGridView_fields.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_fields_CellMouseClick);
            this.dataGridView_fields.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_fields_CellValueChanged);
            this.dataGridView_fields.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_fields_RowPostPaint);
            // 
            // contextMenuStrip_field
            // 
            this.contextMenuStrip_field.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_copy,
            this.toolStripMenuItem_merge,
            this.toolStripMenuItem_up,
            this.toolStripMenuItem_down,
            this.toolStripMenuItem_del});
            this.contextMenuStrip_field.Name = "contextMenuStrip_field";
            this.contextMenuStrip_field.Size = new System.Drawing.Size(272, 114);
            // 
            // toolStripMenuItem_copy
            // 
            this.toolStripMenuItem_copy.Name = "toolStripMenuItem_copy";
            this.toolStripMenuItem_copy.Size = new System.Drawing.Size(271, 22);
            this.toolStripMenuItem_copy.Text = "Copy to Current Setting (Append)";
            // 
            // toolStripMenuItem_merge
            // 
            this.toolStripMenuItem_merge.Name = "toolStripMenuItem_merge";
            this.toolStripMenuItem_merge.Size = new System.Drawing.Size(271, 22);
            this.toolStripMenuItem_merge.Text = "Copy to Current Setting (Merge)";
            // 
            // toolStripMenuItem_up
            // 
            this.toolStripMenuItem_up.Name = "toolStripMenuItem_up";
            this.toolStripMenuItem_up.Size = new System.Drawing.Size(271, 22);
            this.toolStripMenuItem_up.Text = "Move Up";
            // 
            // toolStripMenuItem_down
            // 
            this.toolStripMenuItem_down.Name = "toolStripMenuItem_down";
            this.toolStripMenuItem_down.Size = new System.Drawing.Size(271, 22);
            this.toolStripMenuItem_down.Text = "Move Down";
            // 
            // toolStripMenuItem_del
            // 
            this.toolStripMenuItem_del.Name = "toolStripMenuItem_del";
            this.toolStripMenuItem_del.Size = new System.Drawing.Size(271, 22);
            this.toolStripMenuItem_del.Text = "Delete";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCyan;
            this.panel1.Controls.Add(this.button_check);
            this.panel1.Controls.Add(this.button_submit);
            this.panel1.Controls.Add(this.button_cancel);
            this.panel1.Controls.Add(this.comboBox_templ);
            this.panel1.Controls.Add(this.label_template);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(997, 50);
            this.panel1.TabIndex = 1;
            // 
            // button_check
            // 
            this.button_check.Location = new System.Drawing.Point(494, 13);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(75, 23);
            this.button_check.TabIndex = 4;
            this.button_check.Text = "Check";
            this.button_check.UseVisualStyleBackColor = true;
            this.button_check.Click += new System.EventHandler(this.button_check_Click);
            // 
            // button_submit
            // 
            this.button_submit.Location = new System.Drawing.Point(600, 13);
            this.button_submit.Name = "button_submit";
            this.button_submit.Size = new System.Drawing.Size(75, 23);
            this.button_submit.TabIndex = 3;
            this.button_submit.Text = "Submit";
            this.button_submit.UseVisualStyleBackColor = true;
            this.button_submit.Click += new System.EventHandler(this.button_submit_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(390, 13);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // comboBox_templ
            // 
            this.comboBox_templ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_templ.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_templ.FormattingEnabled = true;
            this.comboBox_templ.Location = new System.Drawing.Point(107, 15);
            this.comboBox_templ.Name = "comboBox_templ";
            this.comboBox_templ.Size = new System.Drawing.Size(250, 20);
            this.comboBox_templ.TabIndex = 1;
            this.comboBox_templ.SelectedIndexChanged += new System.EventHandler(this.comboBox_templ_SelectedIndexChanged);
            // 
            // label_template
            // 
            this.label_template.AutoSize = true;
            this.label_template.Location = new System.Drawing.Point(36, 18);
            this.label_template.Name = "label_template";
            this.label_template.Size = new System.Drawing.Size(65, 12);
            this.label_template.TabIndex = 0;
            this.label_template.Text = "Templates:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(997, 503);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "Delete";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.Width = 32;
            // 
            // dataGridViewImageColumn2
            // 
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle21.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle21.NullValue")));
            this.dataGridViewImageColumn2.DefaultCellStyle = dataGridViewCellStyle21;
            this.dataGridViewImageColumn2.HeaderText = "Up";
            this.dataGridViewImageColumn2.Image = global::IDCM.Properties.Resources.up;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn2.Width = 32;
            // 
            // dataGridViewImageColumn3
            // 
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle22.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle22.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle22.NullValue")));
            this.dataGridViewImageColumn3.DefaultCellStyle = dataGridViewCellStyle22;
            this.dataGridViewImageColumn3.HeaderText = "Down";
            this.dataGridViewImageColumn3.Image = global::IDCM.Properties.Resources.down;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn3.Width = 32;
            // 
            // attr
            // 
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.attr.DefaultCellStyle = dataGridViewCellStyle12;
            this.attr.HeaderText = "Name";
            this.attr.MaxInputLength = 128;
            this.attr.Name = "attr";
            this.attr.ToolTipText = "Field Name";
            // 
            // attrType
            // 
            this.attrType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.attrType.DefaultCellStyle = dataGridViewCellStyle13;
            this.attrType.HeaderText = "Type";
            this.attrType.Name = "attrType";
            this.attrType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.attrType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.attrType.ToolTipText = "Field Type";
            this.attrType.Width = 61;
            // 
            // isUnique
            // 
            this.isUnique.HeaderText = "Unique";
            this.isUnique.Name = "isUnique";
            this.isUnique.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isUnique.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isUnique.Width = 50;
            // 
            // defaultVal
            // 
            this.defaultVal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.defaultVal.DefaultCellStyle = dataGridViewCellStyle14;
            this.defaultVal.HeaderText = "Default Value";
            this.defaultVal.Name = "defaultVal";
            this.defaultVal.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.defaultVal.Width = 110;
            // 
            // restrict
            // 
            this.restrict.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.restrict.DefaultCellStyle = dataGridViewCellStyle15;
            this.restrict.HeaderText = "Restriction Expression ";
            this.restrict.Name = "restrict";
            this.restrict.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.restrict.Width = 150;
            // 
            // mainField
            // 
            this.mainField.HeaderText = "Main";
            this.mainField.Name = "mainField";
            this.mainField.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mainField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.mainField.Width = 50;
            // 
            // comments
            // 
            this.comments.HeaderText = "Comments";
            this.comments.Name = "comments";
            // 
            // Delete
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle16.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle16.NullValue")));
            dataGridViewCellStyle16.Padding = new System.Windows.Forms.Padding(1);
            this.Delete.DefaultCellStyle = dataGridViewCellStyle16;
            this.Delete.HeaderText = "Delete";
            this.Delete.Image = ((System.Drawing.Image)(resources.GetObject("Delete.Image")));
            this.Delete.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Delete.Width = 48;
            // 
            // Up
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle17.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle17.NullValue")));
            dataGridViewCellStyle17.Padding = new System.Windows.Forms.Padding(1);
            this.Up.DefaultCellStyle = dataGridViewCellStyle17;
            this.Up.HeaderText = "Up";
            this.Up.Image = global::IDCM.Properties.Resources.up;
            this.Up.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Up.Name = "Up";
            this.Up.ReadOnly = true;
            this.Up.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Up.Width = 48;
            // 
            // Down
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle18.NullValue")));
            dataGridViewCellStyle18.Padding = new System.Windows.Forms.Padding(1);
            this.Down.DefaultCellStyle = dataGridViewCellStyle18;
            this.Down.HeaderText = "Down";
            this.Down.Image = global::IDCM.Properties.Resources.down;
            this.Down.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Down.Name = "Down";
            this.Down.ReadOnly = true;
            this.Down.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Down.Width = 48;
            // 
            // Reuse
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.NullValue = false;
            dataGridViewCellStyle19.Padding = new System.Windows.Forms.Padding(1);
            this.Reuse.DefaultCellStyle = dataGridViewCellStyle19;
            this.Reuse.HeaderText = "Reuse";
            this.Reuse.Name = "Reuse";
            this.Reuse.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Reuse.Width = 80;
            // 
            // Overwrite
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.Padding = new System.Windows.Forms.Padding(1);
            this.Overwrite.DefaultCellStyle = dataGridViewCellStyle20;
            this.Overwrite.HeaderText = "Overwrite";
            this.Overwrite.Name = "Overwrite";
            this.Overwrite.Text = "Overwrite";
            this.Overwrite.UseColumnTextForButtonValue = true;
            this.Overwrite.Width = 80;
            // 
            // LibFieldSettingView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 503);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LibFieldSettingView";
            this.ShowInTaskbar = false;
            this.Text = "Library Fields Setting";
            this.Load += new System.EventHandler(this.LibFieldSettingView_Load);
            this.Shown += new System.EventHandler(this.LibFieldSettingDlg_Shown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_fields)).EndInit();
            this.contextMenuStrip_field.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_template;
        private System.Windows.Forms.ComboBox comboBox_templ;
        private System.Windows.Forms.Button button_submit;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.DataGridView dataGridView_fields;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_field;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_copy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_merge;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_up;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_down;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_del;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.Button button_check;
        private System.Windows.Forms.DataGridViewTextBoxColumn attr;
        private System.Windows.Forms.DataGridViewComboBoxColumn attrType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isUnique;
        private System.Windows.Forms.DataGridViewTextBoxColumn defaultVal;
        private System.Windows.Forms.DataGridViewTextBoxColumn restrict;
        private System.Windows.Forms.DataGridViewCheckBoxColumn mainField;
        private System.Windows.Forms.DataGridViewTextBoxColumn comments;
        private System.Windows.Forms.DataGridViewImageColumn Delete;
        private System.Windows.Forms.DataGridViewImageColumn Up;
        private System.Windows.Forms.DataGridViewImageColumn Down;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Reuse;
        private System.Windows.Forms.DataGridViewButtonColumn Overwrite;
    }
}