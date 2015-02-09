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
        /// <param name="xmlImportData"></param>
        /// <param name="authInfo"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static XMLImportStrainsRes xmlImportStrains(string xmlImportData, AuthInfo authInfo = null, int timeout = 10000)
        {
            if (xmlImportData != null)
            {
                string signInUri = ConfigurationManager.AppSettings[SysConstants.XMLImportUri];
                string url = string.Format(signInUri, new object[] { authInfo.Jsessionid});
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                log.Info("xmlImportStrains Request Url=" + url);
                string resStr = HttpPostData(url, xmlImportData, null, timeout);
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

        /// <summary>
        /// 实现了post的时候即可以有字符串的key-value,还可以带文件。
        /// 在C#中有HttpWebRequest类，可以很方便用来获取http请求，但是这个类对Post方式没有提供一个很方便的方法来获取数据。
        /// Post提交数据的时候最重要就是把Key-Value的数据放到http请求流中，而HttpWebRequest没有提供一个属性之类的东西可以让我们自由添加Key-Value，因此就必须手工构造这个数据。
        /// 参考：
        /// 1. http://www.cnblogs.com/greenerycn/archive/2010/05/15/csharp_http_post.html
        /// 2. http://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileData"></param>
        /// <param name="nvc"></param>
        /// <returns></returns>
        protected static string HttpPostData(string url, string fileData, NameValueCollection nvc = null, int timeout = 10000)
        {
            string responseContent = null;
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x"); //边界符
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" +boundary;
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
            httpWebRequest.Timeout = timeout;
            httpWebRequest.ReadWriteTimeout = timeout;
            httpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream memStream = new System.IO.MemoryStream();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +boundary + "\r\n");
            string formdataTemplate = "\r\n--" + boundary +"\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";
            if (nvc != null)
            {
                foreach (string key in nvc.Keys)
                {
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }
            }
            memStream.Write(boundarybytes, 0, boundarybytes.Length);
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";
            string header = string.Format(headerTemplate, "filename", "xmlImport.xml");
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            memStream.Write(headerbytes, 0, headerbytes.Length);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(fileData);
            memStream.Write(buffer, 0, buffer.Length);
            httpWebRequest.ContentLength = memStream.Length;
            Stream requestStream = httpWebRequest.GetRequestStream();
            memStream.Position = 0;
            byte[] tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();
            WebResponse webResponse = httpWebRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            responseContent = reader.ReadToEnd();
            webResponse.Close();
            httpWebRequest = null;
            webResponse = null;
            return responseContent;
        }

        protected static XMLImportStrainsRes parserToXMLImportStrainsRes(string jsonStr)
        {
            XMLImportStrainsRes xisr = JsonConvert.DeserializeObject<XMLImportStrainsRes>(jsonStr);
            return xisr;
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
