using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using IDCM.Data.Common;
using IDCM.Data.Common.Generator;

namespace IDCM.Data.Base
{
    class IDCMEnvironment
    {
        /// <summary>
        /// 工作空间名称预定义
        /// </summary>
        private const string default_workspace_name = "IDCMExpress";
        public static string DEFAULT_WORKSPACE
        {
            get
            {
                DirectoryInfo di = null;
                try
                {
                    di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + default_workspace_name);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                }
                catch (Exception)
                {
                    di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + default_workspace_name);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                }
                return di.FullName;
            }
        }
        /// <summary>
        /// 工作空间名称预定义
        /// </summary>
        private static string current_workspace = null;
        public static string CURRENT_WORKSPACE
        {
            get
            {
                if (current_workspace == null)
                {
                    string workspace = ConfigurationManager.AppSettings["WORKSPACE"];
                    if (workspace == null || workspace.Trim().Length < 0 || Directory.Exists(workspace) == false)
                        current_workspace = DEFAULT_WORKSPACE;
                    else
                        current_workspace = workspace;
                }
                return current_workspace;
            }
            set
            {
                DirectoryInfo di = new DirectoryInfo(value);
                if (!di.Exists)
                {
                    di.Create();
                }
                current_workspace = di.FullName;
                ConfigurationHelper.SetAppConfig("WORKSPACE", current_workspace);
            }
        }
        /// <summary>
        /// 客户端唯一性ID标识
        /// </summary>
        private static string gucid;
        public static string GUCID
        {
            get
            {
                if (gucid != null)
                    return gucid;
                gucid = ConfigurationManager.AppSettings["GUCID"];
                Guid _tmp;
                if (gucid == null || Guid.TryParse(gucid, out _tmp) == false)
                {
                    //gucid = Guid.NewGuid().ToString();
                    gucid = CUIDGenerator.getUID();
                    ConfigurationHelper.SetAppConfig("GUCID", gucid);
                }
                return gucid;
            }
        }
        /// <summary>
        /// 最近的用户会话ID标识
        /// </summary>
        private static string luid;
        public static string LUID
        {
            get
            {
                if (luid != null)
                    return luid;
                luid = ConfigurationManager.AppSettings["LUID"];
                return luid == null ? "" : luid;
            }
            set
            {
                luid = value;
                ConfigurationHelper.SetAppConfig("LUID", luid);
            }
        }
        /// <summary>
        /// 伪生硬件编码
        /// </summary>
        private static string fakeBID;
        public static string FAKE_BID
        {
            get
            {
                if (fakeBID != null)
                    return fakeBID;
                else
                {
                    fakeBID = null;
                    DirectoryInfo di = new DirectoryInfo(CURRENT_WORKSPACE);
                    FileInfo[] keyfiles = di.GetFiles("*.key");
                    foreach (FileInfo fi in keyfiles)
                    {
                        if (fi.Length == 0)
                        {
                            fakeBID = (fi.Name.Length > 35) ? fi.Name.Replace(".key", "") : null;
                            break;
                        }
                    }
                    if (fakeBID == null)
                    {
                        //fakeBID = System.Guid.NewGuid().ToString();
                        fakeBID = CUIDGenerator.getUID(CUIDGenerator.Radix_32);
                        FileUtil.writeToUTF8File(CURRENT_WORKSPACE + "\\" + fakeBID + ".key", "");
                    }
                    return fakeBID;
                }
            }
        }
        /// <summary>
        /// 自定义表生成类的默认的命名空间定义
        /// </summary>
        private static string default_customNamespace = "IDCMExpress.Custom";

        public static string Default_customNamespace
        {
            get { return default_customNamespace; }
            set { default_customNamespace = value; }
        }
    }
}
