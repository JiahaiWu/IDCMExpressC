using IDCM.Data.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace IDCM.Service
{
    /// <summary>
    /// Local-GCM 软连接映射缓存实现类
    /// </summary>
    public class LGCMLinkMapHolder
    {
        public static List<string> fetchPublishGCMFields()
        {
            Dictionary<string,int> gcmCols=new Dictionary<string,int>();
            string fpath = ConfigurationManager.AppSettings.Get(SysConstants.GCMUploadTemplate);
            if (fpath == null || fpath.Length < 1)
                return null;
            XmlDocument xmlDoc = new XmlDocument();
            using (FileStream fs = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                xmlDoc.Load(fs);
                foreach (XmlNode sxnode in xmlDoc.ChildNodes)
                {
                    if (sxnode.Name.Equals("strain"))
                    {
                        foreach (XmlNode attrNode in sxnode.ChildNodes)
                        {
                            gcmCols.Add(attrNode.Name, 0);
                        }
                    }
                }
            }
            return gcmCols.Keys.ToList();
        }
    }
}
