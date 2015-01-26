using IDCM.Data.Base;
using IDCM.Service.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IDCM.Forms
{
    public partial class TaskInfoDlg : Form
    {
        public TaskInfoDlg()
        {
            taskInfoMonitor.Interval = 1000;
            taskInfoMonitor.Tick += OnLoadDataSet;
            taskInfoMonitor.Start();
            InitializeComponent();
        }
        private void loadDataSet()
        {
            dataGridView1.Columns.Clear();
            HandleRunInfo[] handRunInfoArray = DWorkMHub.getRunInfoList();
            DataTable dataTable = new DataTable();
            DataColumn firstColumn = new DataColumn("Name", typeof(string));
            DataColumn secondColumn = new DataColumn("Status", typeof(string));
            DataColumn thirdColumn = new DataColumn("RunTime", typeof(string));
            DataColumn fourthColumn = new DataColumn("Description", typeof(string));
            dataTable.Columns.Add(firstColumn);
            dataTable.Columns.Add(secondColumn);
            dataTable.Columns.Add(thirdColumn);
            dataTable.Columns.Add(fourthColumn);
            foreach (HandleRunInfo handRunInfo in handRunInfoArray)
            {
                if (true)//handRunInfo.handleType.Equals(typeof(Thread).Name) || handRunInfo.handleType.Equals(typeof(BackgroundWorker).Name)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = handRunInfo.HName;
                    dataRow[1] = handRunInfo.Status;
                    TimeSpan tspan = new TimeSpan(handRunInfo.RunTime);
                    string tspanDesc = String.Format("{0}h {1}min {2}sec",tspan.TotalHours.ToString("0"), tspan.Minutes, tspan.Seconds);
                    dataRow[2] = tspanDesc;
                    dataRow[3] = handRunInfo.Description;
                    dataTable.Rows.Add(dataRow);
                }

            }
            dataGridView1.DataSource = dataTable;
            label1.Text = "Task Count：" + dataTable.Rows.Count;
        }
        private void TaskInfoDlg_Shown(object sender, EventArgs e)
        {
            loadDataSet();
        }
        private void OnLoadDataSet(object sender, EventArgs e)
        {
            loadDataSet();
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
        private static System.Windows.Forms.Timer taskInfoMonitor = new System.Windows.Forms.Timer();

        private void dataGridView1_RowPostPaint_1(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, this.dataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), this.dataGridView1.RowHeadersDefaultCellStyle.Font, rectangle,
                this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            this.dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }

    }
}
