using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using IDCM.Service.POO;
using IDCM.Service.Utils;

namespace IDCM.Service
{
    public class IDCMEnvironment
    {
        public static StartInfo getLastStartInfo()
        {
            StartInfo si = new StartInfo();
            string val = null;
            val=ConfigurationManager.AppSettings.Get(LastWorkSpace);
            if (val != null && val.IndexOf(",") < 0)
                si.Location = val;
            val = ConfigurationManager.AppSettings.Get(LWSAsDefault);
            if (val != null && val.IndexOf(",") < 0)
                si.asDefaultWorkspace = Convert.ToBoolean(val);
            val = ConfigurationManager.AppSettings.Get(LUID);
            if (val != null && val.IndexOf(",") < 0)
                si.LoginName = val;
            val = ConfigurationManager.AppSettings.Get(LPWD);
            if (val != null && val.IndexOf(",") < 0)
                si.GCMPassword = val;
            return si;
        }

        public static void noteStartInfo(string location, bool asDefaultWorkSpace, string loginName, string gcmPassword)
        {
            ConfigurationHelper.SetAppConfig(LastWorkSpace, location);
            ConfigurationHelper.SetAppConfig(LWSAsDefault, asDefaultWorkSpace.ToString());
            ConfigurationHelper.SetAppConfig(LUID, loginName);
            ConfigurationHelper.SetAppConfig(LPWD, gcmPassword);
        }

        #region 配置参数参数设定
        internal const string LastWorkSpace = "LWS";
        internal const string LWSAsDefault = "LWS_As_Default";
        internal const string LUID = "LUID";
        internal const string LPWD = "LPWD";
        #endregion
    }
}
