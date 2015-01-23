using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Core;

namespace IDCM.Forms
{
    public partial class LoginStatusDlg : Form
    {
        public LoginStatusDlg()
        {
            InitializeComponent();
        }
        public void setSignInInfo(string username, long ticks)
        {
            this.linkLabel_uname.Text = username;
            DateTime datetime = new DateTime(ticks);
            string dateStr = datetime.ToString();
            this.label_timeTag.Text = dateStr;
        }

        private void button_singout_Click(object sender, EventArgs e)
        {
            DataSourceHolder.disconnectGCM(true);
            this.Close();
        }

    }
}
