using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Service.POO;
using IDCM.Common;

namespace IDCM.Forms
{
    public partial class StartView : Form
    {
        public StartView()
        {
            InitializeComponent();
        }
        public void setReferStartInfo(ref StartInfo startInfo)
        {
            this.startInfo = startInfo;
            if (this.startInfo.Location != null)
                this.textBox_datasource.Text = this.startInfo.Location;
            if (this.startInfo.LoginName != null)
                this.textBox_loginName.Text = this.startInfo.LoginName;
            if (this.startInfo.GCMPassword != null)
                this.textBox_pwd.Text = this.startInfo.GCMPassword;
            if (this.startInfo.rememberPassword)
                this.checkBox_remember.Checked = true;
            if (this.startInfo.asDefaultWorkspace)
                this.checkBox_defaultWS.Checked = true;
        }
        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.startInfo.Location = this.textBox_datasource.Text.Trim();
            this.startInfo.LoginName = this.textBox_loginName.Text;
            this.startInfo.GCMPassword = this.textBox_pwd.Text;
            this.startInfo.rememberPassword = this.checkBox_remember.Checked;
            this.startInfo.asDefaultWorkspace = this.checkBox_defaultWS.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void StartView_Shown(object sender, EventArgs e)
        {
            if (this.startInfo.asDefaultWorkspace)
            {
                if (FileUtil.isValidFilePath(this.textBox_datasource.Text) && this.textBox_loginName.Text.Length > 0)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
        private StartInfo startInfo = null;
    }
}
