using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using IDCM.Data.Common;
using IDCM.Data.Base;

namespace IDCM.Data
{
    /// <summary>
    /// 工作空间保持及验证工具类
    /// @author Jiahai Wu
    /// @Date 2014-08-19
    /// </summary>
    class WorkSpaceHolder
    {
        /// <summary>
        /// 检查当前目录下的进程实例是否已存在，如果存在执行退出操作;
        /// 检查目标工作空间的文档是否已占用，如果被占用则执行退出操作;
        /// </summary>
        /// <param name="workspacePath"></param>
        public static void checkWorkSpaceSingleton(string preparepath = null)
        {
            if (preparepath != null)
            {
                if (FileUtil.isFileInUse(preparepath) == true)
                {
                    DialogResult res = MessageBox.Show("The Client is in working. Choose \"OK\" to change a workspace or choose \"Cancel\" to quit.", "Duplicated Workspace Notice", MessageBoxButtons.OKCancel);
                    if (res.Equals(DialogResult.Cancel))
                    {
                        System.Environment.Exit(0);
                    }
                }
                if (preparepath != null && File.Exists(preparepath))
                {
                    IDCMEnvironment.CURRENT_WORKSPACE = Path.GetDirectoryName(preparepath);
                    IDCMEnvironment.LUID = Path.GetFileName(preparepath);
                }
            }
            if (checkDuplicateProcess() != null)
            {
                MessageBox.Show("The process of current directionary is in working. Choose \"OK\" to quit.", "Duplicated Instance Notice", MessageBoxButtons.OK);
                System.Environment.Exit(0);
            }
        }
        /// <summary>
        /// 查询同一目录下是否存在已经运行的进程实例
        /// </summary>
        /// <returns></returns>
        public static Process checkDuplicateProcess()
        {
            Process currentProcess = Process.GetCurrentProcess(); //获取当前进程 
            //获取当前运行程序完全限定名 
            string currentFileName = currentProcess.MainModule.FileName;
            //获取进程名为ProcessName的Process数组。 
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            //遍历有相同进程名称正在运行的进程 
            foreach (Process process in processes)
            {
                if (process.MainModule.FileName == currentFileName)
                {
                    if (process.Id != currentProcess.Id) //根据进程ID排除当前进程 
                    {
                        //当前目录存在已运行的进程实例
                        return process;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// choose or create a new empty file for the new workspace
        /// </summary>
        /// <returns></returns>
        public static bool chooseWorkspace()
        {
            SaveFileDialog fbd = new SaveFileDialog();
            fbd.FileName = CUIDGenerator.getUID(CUIDGenerator.Radix_32) + SysConstants.DB_SUFFIX;
            fbd.InitialDirectory = IDCMEnvironment.DEFAULT_WORKSPACE;
            fbd.Title = "Please Create a new empty file or choose older lib path for the library.";
            fbd.Filter = "IDCM Database File(*" + SysConstants.DB_SUFFIX + ")|*" + SysConstants.DB_SUFFIX;
            fbd.SupportMultiDottedExtensions = false;
            fbd.OverwritePrompt = false;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(fbd.FileName);
                //if (fi.Exists)
                //{
                //    if (fi.Length > 0)
                //    {
                //        MessageBox.Show("The destination file is not empty, please choose again.");
                //        return chooseWorkspace();
                //    }
                //    File.Delete(fbd.FileName);
                //}
                IDCMEnvironment.CURRENT_WORKSPACE = Path.GetDirectoryName(fbd.FileName);
                IDCMEnvironment.LUID = Path.GetFileName(fbd.FileName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 启动工作空间实例
        /// </summary>
        public static string startInstance()
        {
            lock (ShareSyncLockers.WorkSpaceHolder_Lock)
            {
                if (inWorking == false)
                {
                    connectStr = DAMBase.startDBInstance();
                    if (connectStr == null)
                    {
                        MessageBox.Show("Failed to open the data file!  @filepath=" + IDCMEnvironment.CURRENT_WORKSPACE + "\\" + IDCMEnvironment.LUID);
                    }
                    else
                    {
                        inWorking = true;
                        BaseInfoNoteDAM.loadBaseInfo();
                    }
                }
            }
            return connectStr;
        }
        /// <summary>
        /// 初始化用户工作空间
        /// </summary>
        /// <returns></returns>
        public static bool verifyForLoad()
        {
            if (IDCMEnvironment.CURRENT_WORKSPACE == null)
                IDCMEnvironment.CURRENT_WORKSPACE = IDCMEnvironment.DEFAULT_WORKSPACE;
            FileInfo mrcFile = new FileInfo(IDCMEnvironment.CURRENT_WORKSPACE + "\\" + IDCMEnvironment.LUID);
            if (mrcFile == null || !mrcFile.Exists)
            {
                if (chooseWorkspace() == false)
                    return false;
            }
            else
            {
                IDCMEnvironment.CURRENT_WORKSPACE = Path.GetDirectoryName(mrcFile.FullName);
                IDCMEnvironment.LUID = Path.GetFileName(mrcFile.FullName);
            }
            connectStr = startInstance();
            if (connectStr == null && inWorking == false) // For try again
            {
                if (chooseWorkspace())
                {
                    connectStr = startInstance();
                }
            }
            return inWorking;
        }
        /// <summary>
        /// 关闭用户工作空间
        /// </summary>
        /// <returns></returns>
        public static bool close()
        {
            inWorking = false;
            DAMBase.stopDBInstance();
            return true;
        }
        /// <summary>
        /// 当前工作空间状态标识
        /// </summary>
        private static volatile bool inWorking = false;

        public static bool InWorking
        {
            get { return WorkSpaceHolder.inWorking; }
        }
        private static string connectStr = null;
        /// <summary>
        /// 数据库连接串
        /// </summary>
        public static string ConnectStr
        {
            get { return WorkSpaceHolder.connectStr; }
            set { WorkSpaceHolder.connectStr = value; }
        }
    }
}
