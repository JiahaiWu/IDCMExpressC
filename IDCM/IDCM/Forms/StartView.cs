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
using IDCM.Core;

namespace IDCM.Forms
{
    /// <summary>
    /// 初始化欢迎页面窗体实现类
    /// 说明：
    /// 1.默认情况下每次启动都将显示欢迎页面，当有用户设置了默认的工作空间生效后，则会在启动过程中快速跳过用户输入的环节。
    /// 2.用户登录名称建议使用与GCM上注册通过的账号，以便可以有效实现数据同步功能。使用无效用户名，将仅能实现本地数据库访问管理。
    /// 3.这里的登录用户名将作为本地数据库访问的校验码，数据库实例成功加载要求正确的输入登录用户名（初始数据库指定用户名例外）。
    /// 4.这里的登录密码对应的是GCM上注册账号的密码，该密码不在本地数据库中发挥作用，主要用于GCM网络数据资源访问的登录验证的条件。
    /// 5.在输入登录密码的时候，登录用户名不可为空，否则登录用户名和登录密码均可以空值设定。
    /// 6.GCM注册账号的有效性验证并不会阻止用户进入本地数据库管理界面，但正确输入可以及时打开GCM网络数据资源同步服务。
    /// </summary>
    public partial class StartView : Form
    {
        public StartView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设定外部依赖的输入数据值实例对象，并显示必要的默认设定值。
        /// </summary>
        /// <param name="startInfo"></param>
        public void setReferStartInfo(ref StartInfo startInfo)
        {
            this.startInfo = startInfo;
            if (this.startInfo.Location != null)
                this.textBox_datasource.Text = this.startInfo.Location;
            else
                this.startInfo.asDefaultWorkspace = false;
            if (this.startInfo.asDefaultWorkspace)
                this.checkBox_defaultWS.Checked = true;
            if (this.startInfo.LoginName != null)
                this.textBox_loginName.Text = this.startInfo.LoginName;
            if (this.startInfo.GCMPassword != null)
                this.textBox_pwd.Text = this.startInfo.GCMPassword;
            else
                this.startInfo.rememberPassword = false;
            if (this.startInfo.rememberPassword)
                this.checkBox_remember.Checked = true;
        }
        /// <summary>
        /// 用户确认输入信息后，记录用户输入数据值，关闭窗口实例等候父级例程操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_confirm_Click(object sender, EventArgs e)
        {
            if (this.textBox_datasource.Text.Length<1 || !FileUtil.isValidFilePath(this.textBox_datasource.Text))
            {
                MessageBox.Show("Please Create a new empty file or choose older lib path for the library.");
                this.textBox_datasource.Focus();
                return;
            }
            ////////////////////////////////////////////////////////////////
            //if (this.textBox_loginName.Text.Length<1)
            //{
            //    MessageBox.Show("Please specify the login Name for your data library, and it refers your GCM ID normally.");
            //    this.textBox_loginName.Focus();
            //    return;
            //}
            ////////////////////////////////////////////////////////////////
            //@Deprecated
            if (this.textBox_pwd.Text.Length > 0 && this.textBox_loginName.Text.Length < 1)
            {
                MessageBox.Show("The 'LoginName' should not be empty while 'GCMPassword' is not empty, or you can use empty value for both text box。\n"
                    + "The 'GCMPassword' and 'LoginName' will be used for GCM data synchronization service by Network, and the 'LoginName' used as check code for existing data source.\n"
                    +"Use real GCM account for login is recommended.");
                return;
            }
            this.startInfo.Location = this.textBox_datasource.Text.Trim();
            this.startInfo.LoginName = this.textBox_loginName.Text;
            this.startInfo.GCMPassword = this.textBox_pwd.Text;
            this.startInfo.rememberPassword = this.checkBox_remember.Checked;
            this.startInfo.asDefaultWorkspace = this.checkBox_defaultWS.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 欢迎界面的显示事件处理方法
        /// 说明：
        /// 1.如果用户启用了默认工作空间，且参数设置合法则自动提交欢迎窗口关闭事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartView_Shown(object sender, EventArgs e)
        {
            if (this.startInfo.asDefaultWorkspace)
            {
                if (FileUtil.isValidFilePath(this.textBox_datasource.Text) && this.textBox_loginName.Text.Length > 0)
                {
                    if (this.textBox_pwd.Text.Length > 0 && this.textBox_loginName.Text.Length < 1)
                        return;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
        /// <summary>
        /// 取消欢迎页面登录请求。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.startInfo.Location = null;
            this.startInfo.asDefaultWorkspace = false;
            this.startInfo.LoginName = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 通过GCM数据中心获取意备份的数据文档到本地工作目录中
        /// 说明：
        /// 1.具体实现有待补充，still uninplemented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_download_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry, it;s unimplemented yet.");
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
        /// <summary>
        /// 请求帮助说明文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_help_Click(object sender, EventArgs e)
        {
            OnRequestHelp(this, null);
        }
        /// <summary>
        /// From关闭处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartView_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnFormClose(this, new IDCMViewEventArgs(new FormClosedEventArgs[] { e }));
        }
        public event IDCMViewEventHandler OnRequestHelp;
        public event IDCMViewEventHandler OnFormClose;

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private StartInfo startInfo = null;

        

    }
}
