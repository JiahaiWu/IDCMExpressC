using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace IDCM.Data.Base
{
    class ConfigurationHelper
    {
        /// <summary>
        /// SetAppConfig
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appValue"></param>
        /// <param name="configPath"></param>
        public static void SetAppConfig(string appKey, string appValue, string configPath = null)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(File.Exists(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config"), 
                System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config Not Exist!");
#endif
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(configPath == null ? System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config" : configPath);
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
            xDoc.Save(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config");
        }
        /// <summary>
        /// GetAppConfig
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static string GetAppConfig(string appKey, string configPath = null)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(File.Exists(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config"),
                System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config Not Exist!");
#endif
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(configPath == null ? System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config" : configPath);
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
