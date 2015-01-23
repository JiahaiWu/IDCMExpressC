using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace IDCM.Data.Base.Utils
{
    public class ConfigurationHelper
    {
        /// <summary>
        /// SetAppConfig
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appValue"></param>
        /// <param name="configPath"></param>
        public static void SetAppConfig(string appKey, string appValue, string configPath = null)
        {
            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string defaultCfgPath = Path.GetDirectoryName(exePath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(exePath);
            defaultCfgPath = defaultCfgPath .EndsWith(".exe")?defaultCfgPath+".config":defaultCfgPath+ ".exe.config";
            string cfgPath = configPath == null ? defaultCfgPath : configPath;
#if DEBUG
            System.Diagnostics.Debug.Assert(File.Exists(cfgPath), cfgPath + " Not Exist!");
#endif
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(cfgPath);
            var xNode = xDoc.SelectSingleNode("//appSettings");
            var xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
            if (xElem != null) xElem.SetAttribute("value", appValue);
            else
            {
                var xNewElem = xDoc.CreateElement("add");
                xNewElem.SetAttribute("key", appKey);
                xNewElem.SetAttribute("value", appValue);
                xNode.AppendChild(xNewElem);
            }
            xDoc.Save(cfgPath);
        }
        /// <summary>
        /// GetAppConfig
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static string GetAppConfig(string appKey, string configPath = null)
        {
            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string defaultCfgPath = Path.GetDirectoryName(exePath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(exePath);
            defaultCfgPath = defaultCfgPath.EndsWith(".exe") ? defaultCfgPath + ".config" : defaultCfgPath + ".exe.config";
            string cfgPath = configPath == null ? defaultCfgPath : configPath;
#if DEBUG
            System.Diagnostics.Debug.Assert(File.Exists(cfgPath), cfgPath + " Not Exist!");
#endif
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(cfgPath);
            var xNode = xDoc.SelectSingleNode("//appSettings");
            var xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
            if (xElem != null)
            {
                return xElem.Attributes["value"].Value;
            }
            return string.Empty;
        }
    }
}
