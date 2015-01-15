using IDCM.AppContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IDCM.Data.Base.Utils;
using IDCM.Data.Base;
/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM
{
    static class Program
    {
        /// <summary>
        /// IDCM应用程序的主入口点。
        /// 说明：
        /// 1.应用程序支持参数请求，请求格式为 IDCM.exe [-Option1 arg1] [-Option2 arg2] ...，目前支持项有
        ///   1） -ws ${worksSpacePath}
        /// 2.组件依赖主要包括
        ///   1）.Net framework 4.0
        ///   2）Nlog
        ///   3）NPOI
        ///   4) SQLite
        ///   5) Dapper
        ///   6) Newtonsoft.Json
        ///   7) CSharpTest.Net.Libray
        ///   8) Nuit
        /// 注意：
        /// 无效参数请求将被忽略处理，参数选项开关大小写敏感。
        /// 
        /// @author JiahaiWu
        /// </summary>
        /// <param name="args">应用命令调用请求附加参数</param>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Dictionary<string, string> preActions=commandArgScreening(args);
                if (preActions.Count > 0)
                    prepareAppContext(preActions);
                IDCMAppContext appContext = new IDCMAppContext();
                Application.Run(appContext);
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("It's Crash! \n FATAL ERROR:" + ex.Message + "\n" + ex.StackTrace);
#endif
                log.Info("FATAL!", ex);
            }
        }
        /// <summary>
        /// 控制台请求参数初筛
        /// </summary>
        /// <param name="args">应用程序启动时命令行参数</param>
        /// <returns>返回过滤后的“有效”命令-参数表达式集合</returns>
        private static Dictionary<string,string> commandArgScreening(string[] args)
        {
            Dictionary<string, string> actions = new Dictionary<string, string>();
#if DEBUG
            System.Diagnostics.Debug.Assert(args != null);
#endif
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length - 1; i++)
                {
                    if (args[i].StartsWith("-"))
                    {
                        string cmd = args[i].Substring(1);
                        if (cmd.Length>0 && prepareAbleCmds.Contains(cmd.ToLower()))
                        {
                            string param = args[i + 1].Trim(new char[] { '"','\'' });
                            actions[cmd.ToLower()] = param;
                        }
                    }
                }
            }
            return actions;
        }
        /// <summary>
        /// 命令行参数预处理
        /// </summary>
        /// <param name="preActions"></param>
        private static void prepareAppContext(Dictionary<string, string> preActions)
        {
            foreach (KeyValuePair<string, string> kvpair in preActions)
            {
                if(kvpair.Key.Equals(CMD_RESET))
                {
                    ConfigurationHelper.SetAppConfig(kvpair.Value, "");
                }
            }
        }

        private const string CMD_RESET = "reset";
        private static string[] prepareAbleCmds = new string[] { CMD_RESET };

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
