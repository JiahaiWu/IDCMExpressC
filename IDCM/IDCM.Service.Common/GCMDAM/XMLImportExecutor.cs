using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Xml;
using IDCM.Data.Base.Utils;

namespace IDCM.Service.Common.GCMDAM
{
    class XMLImportExecutor
    {
        public static List<string> fetchPublishGCMFields()
        {
            Dictionary<string, int> gcmCols = new Dictionary<string, int>();
            string fpath = ConfigurationManager.AppSettings.Get(SysConstants.GCMUploadTemplate);
            if (fpath == null || fpath.Length < 1)
                return null;
            XmlDocument xmlDoc = new XmlDocument();
            using (FileStream fs = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                xmlDoc.Load(fs);
                XmlNode sxnode = xmlDoc.SelectSingleNode("/strains/strain");
                if (sxnode == null)
                    return null;
                foreach (XmlNode attrNode in sxnode.ChildNodes)
                {
                    gcmCols[attrNode.Name]=0;
                }
            }
            return gcmCols.Keys.ToList();
        }

        /// <summary>
        /// XML上传，批量导入（如果菌号相同，则更新均中信息）
        /// 说明：
        /// 返回结果	例：{"msg_num":"2"}
        /// 返回结果代码参考:
        /// 0:文件类型错误
        /// 1:xml文件内容错误并返回错误行数据
        /// 2:导入成功
        /// 3:xml解析异常，xml文件格式不正确
        /// 4:导入失败，请与管理员联系
        /// loginflag:"false" 没有登录 JSESSIONID失效
        /// </summary>
        /// <param name="xmlImportStream"></param>
        /// <param name="authInfo"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static XMLImportStrainsRes xmlImportStrains(MemoryStream xmlImportStream, AuthInfo authInfo = null, int timeout = 10000)
        {
            if (xmlImportStream != null)
            {
                string signInUri = ConfigurationManager.AppSettings[SysConstants.XMLImportUri];
                string url = string.Format(signInUri, new object[] { authInfo.Jsessionid});
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                log.Info("xmlImportStrains Request Url=" + url);
                //string resStr = HttpRequestUtil.HttpPostMultipartData(url, xmlImportStream, null, timeout);
                string resStr = HttpRequestUtil.HttpPostFileData(url, "F:/EQ31L1VD4ZII1NBE4IJINM3W2K.xml", null, timeout);
                log.Info("XMLImportExecutor Response=" + resStr);
                XMLImportStrainsRes xisr = parserToXMLImportStrainsRes(resStr);
                if (xisr != null)
                {
                    xisr.Jsessionid = authInfo.Jsessionid;
                }
                return xisr;
            }
            return null;
        }

        protected static XMLImportStrainsRes parserToXMLImportStrainsRes(string jsonStr)
        {
            XMLImportStrainsRes xisr = JsonConvert.DeserializeObject<XMLImportStrainsRes>(jsonStr);
            return xisr;
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
