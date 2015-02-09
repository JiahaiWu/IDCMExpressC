using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using IDCM.AppContext;
using IDCM.ViewManager;
using IDCM.Service.Utils;
using IDCM.Core;

namespace IDCM.Forms
{
    public partial class LibFieldSettingView : Form
    {
        private LibFieldManager manager = null;
        

        internal void setManager(LibFieldManager manager)
        {
            this.manager = manager;
        }

        public LibFieldSettingView()
        {
            InitializeComponent();
            
        }

        public event IDCMViewEventHandler OnRemoveField;
        public event IDCMViewEventHandler OnSelectTemplate;
        public event IDCMViewEventHandler OnAppendField;
        public event IDCMViewEventHandler OnOverwriteField;
        public event IDCMViewEventHandler OnSubmitSetting;
        public event IDCMViewEventHandler OnCheckFields;

        public ComboBox getTemplateChx()
        {
            return comboBox_templ;
        }
        public DataGridView getFieldDGV()
        {
            return dataGridView_fields;
        }

        private void LibFieldSettingDlg_Shown(object sender, EventArgs e)
        {
            
        }

        private void comboBox_templ_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectTemplate(this, new IDCMViewEventArgs(new object[] { comboBox_templ.SelectedIndex }));
            if (comboBox_templ.SelectedIndex > 0)
            {
                toolStripMenuItem_copy.Visible = true;
                toolStripMenuItem_merge.Visible = true;
                toolStripMenuItem_down.Visible = false;
                toolStripMenuItem_up.Visible = false;
                toolStripMenuItem_del.Visible = false;
            }
            else
            {
                toolStripMenuItem_copy.Visible = false;
                toolStripMenuItem_merge.Visible = false;
                toolStripMenuItem_down.Visible = true;
                toolStripMenuItem_up.Visible = true;
                toolStripMenuItem_del.Visible = true;
            }
        }

        private void LibFieldSettingView_Load(object sender, EventArgs e)
        {
            comboBox_templ.SelectedIndex = 0;
            //Thread.CurrentThread.Name = "LibFieldSettingView" + HandleToken.nextTempID();
        }

        private void dataGridView_fields_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_fields.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGridView_fields.RowHeadersDefaultCellStyle.Font, rectangle,
                dataGridView_fields.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            dataGridView_fields.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }

        private void dataGridView_fields_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            if (comboBox_templ.SelectedIndex == 0)
            {
                if (dataGridView_fields.Columns[e.ColumnIndex].HeaderText.Equals("Down"))
                {
                    int selectedRowIndex = e.RowIndex;
                    int rowCount = dataGridView_fields.Rows.Count - 1;
                    if (dataGridView_fields.Rows[rowCount].IsNewRow)
                        --rowCount;
                    if (selectedRowIndex < rowCount)
                    {
                        DataGridViewRow newRow = dataGridView_fields.Rows[selectedRowIndex];
                        dataGridView_fields.Rows.RemoveAt(selectedRowIndex);
                        dataGridView_fields.Rows.Insert(selectedRowIndex + 1, newRow);
                        //dataGridView_fields.Rows[selectedRowIndex + 1].Selected = true;
                    }
                }
                else if (dataGridView_fields.Columns[e.ColumnIndex].HeaderText.Equals("Up"))
                {
                    int selectedRowIndex = e.RowIndex;
                    if (selectedRowIndex > 0)
                    {
                        DataGridViewRow newRow = dataGridView_fields.Rows[selectedRowIndex];
                        dataGridView_fields.Rows.RemoveAt(selectedRowIndex);
                        dataGridView_fields.Rows.Insert(selectedRowIndex - 1, newRow);
                        //dataGridView_fields.Rows[selectedRowIndex - 1].Selected = true;
                    }
                }
                else if (dataGridView_fields.Columns[e.ColumnIndex].HeaderText.Equals("Delete"))
                {
                    OnRemoveField(this, new IDCMViewEventArgs(new DataGridViewRow[] { dataGridView_fields.Rows[e.RowIndex] }));
                    dataGridView_fields.Rows.RemoveAt(e.RowIndex);
                }
            }
            else
            {
                if (dataGridView_fields.Columns[e.ColumnIndex].HeaderText.Equals("Reuse"))
                {
                    DataGridViewCheckBoxCell dgvc = dataGridView_fields.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                    if(dgvc.FormattedValue.Equals(true))
                        OnAppendField(this,new IDCMViewEventArgs(new object[]{dataGridView_fields.Rows[e.RowIndex],this.comboBox_templ.SelectedItem.ToString()}));
                    else
                        OnRemoveField(this, new IDCMViewEventArgs(new DataGridViewRow[] { dataGridView_fields.Rows[e.RowIndex] }));                        
                }
                else if (dataGridView_fields.Columns[e.ColumnIndex].HeaderText.Equals("Overwrite"))
                {
                    OnOverwriteField(this, new IDCMViewEventArgs(new object[] { dataGridView_fields.Rows[e.RowIndex], this.comboBox_templ.SelectedItem.ToString() }));
                }
            }
        }

        private void dataGridView_fields_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            if (comboBox_templ.SelectedIndex > 0)
            {
                if (dataGridView_fields.Columns[e.ColumnIndex].HeaderText.Equals("Reuse"))
                {
                    DataGridViewCheckBoxCell dgvc = dataGridView_fields.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                    if (dgvc.FormattedValue.Equals(true))
                        OnAppendField(this,new IDCMViewEventArgs(new object[]{dataGridView_fields.Rows[e.RowIndex],this.comboBox_templ.SelectedItem.ToString()}));
                    else
                        OnRemoveField(this, new IDCMViewEventArgs(new DataGridViewRow[] { dataGridView_fields.Rows[e.RowIndex] }));  
                }
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            //this.Close();
            //IDCMAppContext.MainManger.activeChildView(typeof(HomeView), true);
            //this.Dispose();
        }

        private void button_submit_Click(object sender, EventArgs e)
        {
            OnSubmitSetting(this,null);
            this.Close();
            this.Dispose();
        }

        private void button_check_Click(object sender, EventArgs e)
        {
            if (comboBox_templ.SelectedIndex == 0)
            {
              manager.checkFields();//此方法有返回值
            }
        }

        private void dataGridView_fields_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //右键
            if (e.Button != MouseButtons.Right)
                return;
            if (!dataGridView_fields.Rows[e.RowIndex].IsNewRow)
            {
                //contextMenuStrip_field.Tag = e.ColumnIndex + "," + e.RowIndex;
                ControlUtil.ClearEvent(toolStripMenuItem_copy,"Click");
                toolStripMenuItem_copy.Click += delegate(object tsender, EventArgs te) { toolStripMenuItem_copy_Click(tsender, te, e.ColumnIndex,e.RowIndex); };
                ControlUtil.ClearEvent(toolStripMenuItem_merge, "Click");
                toolStripMenuItem_merge.Click += delegate(object tsender, EventArgs te) { toolStripMenuItem_merge_Click(tsender, te, e.ColumnIndex, e.RowIndex); };
                ControlUtil.ClearEvent(toolStripMenuItem_up, "Click");
                toolStripMenuItem_up.Click += delegate(object tsender, EventArgs te) { toolStripMenuItem_up_Click(tsender, te, e.ColumnIndex, e.RowIndex); };
                ControlUtil.ClearEvent(toolStripMenuItem_down, "Click");
                toolStripMenuItem_down.Click += delegate(object tsender, EventArgs te) { toolStripMenuItem_down_Click(tsender, te, e.ColumnIndex, e.RowIndex); };
                ControlUtil.ClearEvent(toolStripMenuItem_del, "Click");
                toolStripMenuItem_del.Click += delegate(object tsender, EventArgs te) { toolStripMenuItem_del_Click(tsender, te, e.ColumnIndex, e.RowIndex); };

                contextMenuStrip_field.Show(MousePosition);
            }
        }
        private void toolStripMenuItem_copy_Click(object sender, EventArgs e, int columnIndex, int rowIndex)
        {
            OnAppendField(this, new IDCMViewEventArgs(new object[] { dataGridView_fields.Rows[rowIndex], this.comboBox_templ.SelectedText }));
        }

        private void toolStripMenuItem_merge_Click(object sender, EventArgs e, int columnIndex, int rowIndex)
        {
            OnOverwriteField(this, new IDCMViewEventArgs(new object[] { dataGridView_fields.Rows[rowIndex], this.comboBox_templ.SelectedText }));
        }

        private void toolStripMenuItem_up_Click(object sender, EventArgs e, int columnIndex, int rowIndex)
        {
            if (comboBox_templ.SelectedIndex == 0)
            {
                if (rowIndex > 0)
                {
                    DataGridViewRow newRow = dataGridView_fields.Rows[rowIndex];
                    dataGridView_fields.Rows.RemoveAt(rowIndex);
                    dataGridView_fields.Rows.Insert(rowIndex - 1, newRow);
                    //dataGridView_fields.Rows[rowIndex - 1].Selected = true;
                }
            }
        }

        private void toolStripMenuItem_down_Click(object sender, EventArgs e, int columnIndex, int rowIndex)
        {
            
            if (comboBox_templ.SelectedIndex == 0)
            {
                int rowCount = dataGridView_fields.RowCount - 1;
                if (dataGridView_fields.Rows[rowCount].IsNewRow)
                    --rowCount;
                if (rowIndex < rowCount)
                {
                    DataGridViewRow newRow = dataGridView_fields.Rows[rowIndex];
                    dataGridView_fields.Rows.RemoveAt(rowIndex);
                    dataGridView_fields.Rows.Insert(rowIndex + 1, newRow);
                    //dataGridView_fields.Rows[rowIndex + 1].Selected = true;
                }
            }
        }

        private void toolStripMenuItem_del_Click(object sender, EventArgs e, int columnIndex, int rowIndex)
        {
            if (comboBox_templ.SelectedIndex == 0)
            {
                OnRemoveField(this, new IDCMViewEventArgs(new DataGridViewRow[] { dataGridView_fields.Rows[rowIndex] }));
                dataGridView_fields.Rows.RemoveAt(rowIndex);
            }
        }

    }
}
