using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using IDCM.Service.Common;
using System.Windows.Forms;

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
        /// choose or create a new workspace
        /// </summary>
        /// <returns></returns>
        public static bool chooseWorkspace(string location,string loginName)
        {
            dataSource = new DataSourceMHub(location, loginName);
            return dataSource.connect();
        }
        /// <summary>
        /// 启动工作空间实例
        /// </summary>
        public static bool prepareInstance()
        {
            if (dataSource.Connected)
            {
                if (dataSource.prepare())
                {
                    log.Debug("Prepare datasource succeed.");
                    return true;
                }else
                {
                    MessageBox.Show("Failed to open the data source. ");
                }
            }
            return false;
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
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
