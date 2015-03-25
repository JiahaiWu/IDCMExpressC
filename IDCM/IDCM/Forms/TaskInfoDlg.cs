using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.Utils;
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
            InitializeComponent();
            taskInfoMonitor.Interval = 1000;
            taskInfoMonitor.Tick += OnLoadTaskInfo;
            taskInfoMonitor.Start();
        }
        /// <summary>
        /// 加载任务信息
        /// </summary>
        private void loadTaskInfo()
        {
            this.dataGridView1.Rows.Clear();
            HandleRunInfo[] handRunInfoArray = DWorkMHub.getRunInfoList();           
            foreach (HandleRunInfo handRunInfo in handRunInfoArray)
            {
                TimeSpan tspan = new TimeSpan(handRunInfo.RunTime);
                string tspanDesc = String.Format("{0}h {1}min {2}sec", tspan.TotalHours.ToString("0"), tspan.Minutes, tspan.Seconds);
                string[] values = new string[] { handRunInfo.HName, handRunInfo.Status, tspanDesc, handRunInfo.Description };
                if (isShowFrom)
                    DGVAsyncUtil.syncAddRow(this.dataGridView1, values, this.dataGridView1.RowCount);
                else
                { 
                    if(!handRunInfo.handleType.Equals(typeof(Form).Name))
                        DGVAsyncUtil.syncAddRow(this.dataGridView1, values, this.dataGridView1.RowCount);
                }         
            }
            label1.Text = "Task Count：" + this.dataGridView1.Rows.Count;
        }
        /// <summary>
        /// 定时刷新任务信息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadTaskInfo(object sender, EventArgs e)
        {
            loadTaskInfo();
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

        private void dataGridView1_RowPostPaint_1(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, this.dataGridView1.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), this.dataGridView1.RowHeadersDefaultCellStyle.Font, rectangle,
                this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            this.dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }       

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.isShowFrom = this.checkBox1.Checked;
        }

        private bool isShowFrom = false;

        private void TaskInfoDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskInfoMonitor.Stop();
        }

        private System.Windows.Forms.Timer taskInfoMonitor = new System.Windows.Forms.Timer();
    }
}
