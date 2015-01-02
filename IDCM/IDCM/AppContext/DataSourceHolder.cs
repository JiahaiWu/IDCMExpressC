using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using IDCM.Service.Common;

namespace IDCM.AppContext
{
    /// <summary>
    /// 工作空间保持及验证工具类
    /// @author Jiahai Wu
    /// @Date 2014-08-19
    /// </summary>
    class DataSourceHolder
    {
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
            return dataSource.disconnect();
        }
        /// <summary>
        /// 数据源实例
        /// </summary>
        private static volatile DataSourceMHub dataSource = null;
        /// <summary>
        /// 返回是否处于数据源连接保持中
        /// </summary>
        public static bool InWorking
        {
            get { return dataSource!=null && dataSource.InWorking; }
        }
        /// <summary>
        /// 数据库连接串
        /// </summary>
        private static string connectStr = null;
        /// <summary>
        /// 获取或设置数据库连接串
        /// </summary>
        public static string ConnectStr
        {
            get { return connectStr; }
            set { connectStr = value; }
        }
    }
}
