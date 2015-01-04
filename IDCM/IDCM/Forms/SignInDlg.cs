using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Data.Base;

namespace IDCM.Forms
{
    public partial class SignInDlg : Form
    {
        public SignInDlg()
        {
            InitializeComponent();
        }
        public void setReferAuthInfo(AuthInfo auth)
        {
            this.authInfo = auth;
            if (authInfo.Username != null)
                this.textBox_uname.Text = authInfo.Username;
            this.textBox_uname.Focus();
        }

        private void button_signin_Click(object sender, EventArgs e)
        {
            //string uname = this.textBox_uname.Text.Trim();
            //string pwd = this.textBox_pwd.Text;
            //if (uname.Length > 0 && pwd.Length > 0)
            //{
            //    this.Enabled = false;
            //    authInfo = SignInExecutor.SignIn(uname, pwd, 3000, this.checkBox_auto.Checked);
            //    authInfo.autoLogin = this.checkBox_auto.Checked;
            //    authInfo.Username = uname;
            //    authInfo.Password = pwd;
            //    authInfo.Timestamp = DateTime.Now.Ticks;
            //    this.Enabled = true;
            //}
            //if (authInfo == null || !authInfo.LoginFlag)
            //{
            //    MessageBox.Show("Sign in request failed!", "Notice", MessageBoxButtons.OK);
            //    this.textBox_uname.Focus();
            //}
            this.Close();
        }
        public bool isAutoLogin()
        {
            return this.checkBox_auto.Checked;
        }
        public AuthInfo getAuthInfo()
        {
            return authInfo;
        }
        private AuthInfo authInfo =null;
    }
}
