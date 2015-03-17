using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace IDCM.Test
{
    class HttpUpload
    {
        /// <summary>
        /// 实现了post的时候即可以有字符串的key-value,还可以带文件。
        /// 在C#中有HttpWebRequest类，可以很方便用来获取http请求，但是这个类对Post方式没有提供一个很方便的方法来获取数据。
        /// Post提交数据的时候最重要就是把Key-Value的数据放到http请求流中，而HttpWebRequest没有提供一个属性之类的东西可以让我们自由添加Key-Value，因此就必须手工构造这个数据。
        /// 参考：
        /// 1. http://www.cnblogs.com/greenerycn/archive/2010/05/15/csharp_http_post.html
        /// 2. http://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="files"></param>
        /// <param name="logpath"></param>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public static string UploadFilesToRemoteUrl(string url, string[] files, string logpath, NameValueCollection nvc)
        {
            string responseContent = null;
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x"); //边界符
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" +boundary;
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream memStream = new System.IO.MemoryStream();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +boundary + "\r\n");
            string formdataTemplate = "\r\n--" + boundary +"\r\nContent-Disposition: form-data; name=\"{0}\";\r\r\n\n{1}";
            foreach (string key in nvc.Keys)
            {
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }
            memStream.Write(boundarybytes, 0, boundarybytes.Length);
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\r\n\n";

            for (int i = 0; i < files.Length; i++)
            {
                string header = string.Format(headerTemplate, Path.GetFileName(files[i]), files[i]);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                memStream.Write(headerbytes, 0, headerbytes.Length);
                FileStream fileStream = new FileStream(files[i], FileMode.Open,FileAccess.Read);
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }
                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                fileStream.Close();
            }
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
    }
}
