using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using IDCM.Service.POO;
using IDCM.Service.Utils;
using System.IO;
using IDCM.Data.Base;
using IDCM.Data.Base.Utils;

namespace IDCM.Service
{
    public class IDCMEnvironment
    {
        public static StartInfo getLastStartInfo()
        {
            StartInfo si = new StartInfo();
            try
            {
                string val = null;
                val = ConfigurationManager.AppSettings.Get(SysConstants.LastWorkSpace);
                if (val != null && val.IndexOf(",") < 0)
                    si.Location = val;
                val = ConfigurationManager.AppSettings.Get(SysConstants.LWSAsDefault);
                if (val != null && val.IndexOf(",") < 0)
                    si.asDefaultWorkspace = val.Length>0?Convert.ToBoolean(val):false;
                val = ConfigurationManager.AppSettings.Get(SysConstants.LUID);
                if (val != null && val.IndexOf(",") < 0 && val.Length > 0)
                    si.LoginName = val;
                val = ConfigurationManager.AppSettings.Get(SysConstants.LPWD);
                if (val != null && val.IndexOf(",") < 0 && val.Length>0)
                {
                    si.GCMPassword = Base64DESEncrypt.CreateInstance(si.LoginName).Decrypt(val);
                }

                if (si.Location == null)
                {
                    string initDir = "";
                    try
                    {
                        initDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                        DirectoryInfo dirInfo = Directory.CreateDirectory(initDir + Path.DirectorySeparatorChar + SysConstants.APP_Assembly);
                        initDir = dirInfo.FullName;
                        si.Location = initDir + Path.DirectorySeparatorChar+ CUIDGenerator.getUID(CUIDGenerator.Radix_32)+ SysConstants.DB_SUFFIX;
                    }
                    catch (Exception ex)
                    {
                        log.Warn("Default Initial Directory based from Environment.SpecialFolder.CommonApplicationData fetch failed.", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Can not load Last configuration of IDCM profile!",ex);
            }
            return si;
        }
        public static void noteStartInfo(string location, bool asDefaultWorkSpace, string loginName, string gcmPassword)
        {
            //ConfigurationManager.AppSettings.Set(SysConstants.LastWorkSpace, location);
            //ConfigurationManager.AppSettings.Set(SysConstants.LWSAsDefault, asDefaultWorkSpace.ToString());
            //ConfigurationManager.AppSettings.Set(SysConstants.LUID, loginName);
            //ConfigurationManager.AppSettings.Set(SysConstants.LPWD, Base64DESEncrypt.CreateInstance(loginName).Encrypt(gcmPassword));

            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            string defaultCfgPath = Path.GetDirectoryName(exePath) + Path.DirectorySeparatorChar + Path.GetFileName(exePath).Replace(".vshost.exe",".exe") + ".config";
            ConfigurationHelper.SetAppConfig(SysConstants.LastWorkSpace, location, defaultCfgPath);
            ConfigurationHelper.SetAppConfig(SysConstants.LWSAsDefault, asDefaultWorkSpace.ToString(), defaultCfgPath);
            ConfigurationHelper.SetAppConfig(SysConstants.LUID, loginName, defaultCfgPath);
            ConfigurationHelper.SetAppConfig(SysConstants.LPWD, Base64DESEncrypt.CreateInstance(loginName).Encrypt(gcmPassword), defaultCfgPath);
        }

        #region 配置参数参数设定

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        #endregion
    }
}
