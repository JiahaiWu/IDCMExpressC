using IDCM.ViewLL.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Forms
{
    public partial class StackInfoView : Form
    {
        private StackInfoManager manager;

        internal void setManager(StackInfoManager manager) {
            this.manager = manager;
        }

        public void loadData(DataTable dataTable) {
            this.label1.Text = "后台任务数：" + dataTable.Rows.Count;
            this.dataGridView1.DataSource = dataTable;    
            
        }

       

        public StackInfoView()
        {
            
            InitializeComponent();
        }


        private void StackInfoView_Load(object sender, EventArgs e)
        {
            manager.loadStackData();            
        }

        /// <summary>
        /// 设置Datagridview显示行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, this.dataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), this.dataGridView1.RowHeadersDefaultCellStyle.Font, rectangle,
                this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            this.dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }

        
    }
}
