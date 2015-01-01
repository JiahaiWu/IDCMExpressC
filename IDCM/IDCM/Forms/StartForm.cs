using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.ViewLL.Manager;
using IDCM.ServiceBL;
using IDCM.SimpleDAL.POO;
using IDCM.SimpleDAL.DAM;
using IDCM.AppContext;

namespace IDCM.Forms
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            this.pictureBox_loading.Visible = false;
            this.BringToFront();
        }

        private void StartForm_Shown(object sender, EventArgs e)
        {
            //Run HandleInstanceMonitor
            loadingMonitor.Interval = 500;
            loadingMonitor.Tick += OnHeartBreak;
            loadingMonitor.Start();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.button_confirm.Visible = false;
            OnHeartBreak(this, null);
        }
        /// <summary>
        /// 初始化载入监视器的心跳检测事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeartBreak(object sender, EventArgs e)
        {
            if (IDCMFormManger.getInstance().isAllInited())
            {
                if(loadingMonitor.Enabled)
                    loadingMonitor.Stop();
                this.pictureBox_loading.Visible = false;
                this.DialogResult = DialogResult.Yes;
                this.Close();
                this.Dispose();
            }
            else if (!this.button_confirm.Visible)
            {
                this.pictureBox_loading.Visible = true;
            }
        }
        /// <summary>
        /// Start IDCM Instance Monitor
        /// </summary>
        private static System.Windows.Forms.Timer loadingMonitor = new System.Windows.Forms.Timer();
    }
}
