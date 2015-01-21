using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Core;

namespace IDCM.Forms
{
    public partial class SignInDlg : Form
    {
        public SignInDlg()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设置引用对象实例
        /// </summary>
        /// <param name="auth"></param>
        public void setReferAuthInfo(AuthInfo auth)
        {
            if (auth != null)
            {
                if(auth.Username != null)
                    this.textBox_uname.Text = auth.Username;
                if (auth.Password != null)
                    this.textBox_pwd.Text = auth.Password;
                if (auth.Username != null)
                    this.checkBox_auto.Checked = auth.autoLogin;
            }
            this.textBox_uname.Focus();
        }
        /// <summary>
        /// 确认用户登录按钮事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_signin_Click(object sender, EventArgs e)
        {
            string uname = this.textBox_uname.Text.Trim();
            string pwd = this.textBox_pwd.Text;
            if (uname.Length > 0 && pwd.Length > 0)
            {
                this.Enabled = false;
                bool res = DataSourceHolder.connectGCM(uname, pwd, this.checkBox_auto.Checked);
                this.Enabled = true;
                if (res == false)
                {
                    MessageBox.Show("Sign in request failed!", "Notice", MessageBoxButtons.OK);
                    this.textBox_uname.Focus();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please input login name and password.", "Notice", MessageBoxButtons.OK);
            }
        }
        public bool isAutoLogin()
        {
            return this.checkBox_auto.Checked;
        }
    }
}
