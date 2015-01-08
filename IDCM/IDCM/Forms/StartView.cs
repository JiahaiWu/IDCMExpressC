using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Service.POO;
using System.IO;
using IDCM.Data.Base;
using IDCM.Data.Base.Utils;
using IDCM.Service.Common;

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
            if (this.startInfo.asDefaultWorkspace)
                this.checkBox_defaultWS.Checked = true;
            if (this.startInfo.LoginName != null)
                this.textBox_loginName.Text = this.startInfo.LoginName;
            if (this.startInfo.GCMPassword != null)
                this.textBox_pwd.Text = this.startInfo.GCMPassword;
            if (this.startInfo.rememberPassword)
                this.checkBox_remember.Checked = true;
        }
        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (this.textBox_datasource.Text.Length<1 || !FileUtil.isValidFilePath(this.textBox_datasource.Text))
            {
                MessageBox.Show("Please Create a new empty file or choose older lib path for the library.");
                this.textBox_datasource.Focus();
                return;
            }
            if (this.textBox_loginName.Text.Length<1)
            {
                MessageBox.Show("Please specify the login Name for your data library, and it refers your GCM ID normally.");
                this.textBox_loginName.Focus();
                return;
            }
            this.startInfo.Location = this.textBox_datasource.Text.Trim();
            this.startInfo.LoginName = this.textBox_loginName.Text;
            this.startInfo.GCMPassword = this.textBox_pwd.Text;
            this.startInfo.rememberPassword = this.checkBox_remember.Checked;
            this.startInfo.asDefaultWorkspace = this.checkBox_defaultWS.Checked;
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
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_download_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 选取或新建一个数据源文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_datasource_Click(object sender, EventArgs e)
        {
            string initDir = "";
            try
            {
                initDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                DirectoryInfo dirInfo = Directory.CreateDirectory(initDir + Path.DirectorySeparatorChar + SysConstants.APP_Assembly);
                initDir = dirInfo.FullName;
            }
            catch (Exception ex)
            {
                log.Warn("Default Initial Directory based from Environment.SpecialFolder.CommonApplicationData fetch failed.",ex);
            }
            SaveFileDialog fbd = new SaveFileDialog();
            fbd.InitialDirectory = initDir;
            fbd.FileName = this.textBox_datasource.Text.Length>0 ? this.textBox_datasource.Text : CUIDGenerator.getUID(CUIDGenerator.Radix_32) + SysConstants.DB_SUFFIX;
            fbd.Title = "Please Create a new empty file or choose older lib path for the library.";
            fbd.Filter = "IDCM Database File(*" + SysConstants.DB_SUFFIX + ")|*" + SysConstants.DB_SUFFIX;
            fbd.SupportMultiDottedExtensions = false;
            fbd.OverwritePrompt = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.textBox_datasource.Text = fbd.FileName;
            }
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private StartInfo startInfo = null;
    }
}
