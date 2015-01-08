using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using IDCM.Data.Common;
using IDCM.Data.Base.Utils;

namespace IDCM.Data.Core
{
    /// <summary>
    /// 工作空间保持及验证工具类
    /// @author Jiahai Wu
    /// @Date 2014-08-19
    /// </summary>
    class WorkSpaceHelper
    {
        /// <summary>
        /// 检查目标工作空间的文档当前是否为可访问状态
        /// 说明：
        /// 1.如果文件不存在或文件为另一个进程所访问，则为不可访问内容状态，返回false
        /// </summary>
        /// <param name="checkPath">目标文件路径</param>
        /// <returns>文件内容是否可访问</returns>
        public static bool isWorkSpaceAccessible(string checkPath)
        {
            try
            {
                if (File.Exists(checkPath))
                {
                    return !FileUtil.isFileInUse(checkPath);
                }
                else
                {
                    return FileUtil.isValidFilePath(checkPath);
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        /// <summary>
        /// 检查当前路径的源程序是否已存在运行实例
        /// 参考：
        /// checkDuplicateProcess()
        /// </summary>
        /// <returns>是否存在</returns>
        public static bool isProcessDuplicate()
        {
            if (checkDuplicateProcess()!=null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 查询当前路径的源程序是否已存在运行实例
        /// 说明：
        /// 如果存在同一程序集创建的进程实例，返回进程句柄对象，否则返回空
        /// </summary>
        /// <returns>进程实例句柄对象（null able）</returns>
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
                try
                {
                    if (process.MainModule.FileName.Equals(currentFileName))
                    {
                        if (process.Id != currentProcess.Id) //根据进程ID排除当前进程 
                        {
                            //当前目录存在已运行的进程实例
                            return process;
                        }
                    }
                }
                catch (Exception)
                {
                    // GetProcessesByName simply calls GetProcesses and iterates over the whole list and compares the ProcessName.
                    // However there are processes which aren't accessible and throw an exception when you try to read the ProcessName.
                    // So you should iterate through the array returned by GetProcesses yourself and catch exceptions for those processes which aren't accessible.
                    // Refer: http://answers.unity3d.com/questions/541990/processgetprocessesbynamestring-str-not-working.html
                    continue;
                }
            }
            return null;
        }
    }
}
