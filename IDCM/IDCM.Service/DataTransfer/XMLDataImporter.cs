using IDCM.Data.Base;
using IDCM.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace IDCM.Service.DataTransfer
{
    class XMLDataImporter
    {
         /// <summary>
        /// 解析指定的Excel文档，执行数据转换.
        /// 本方法调用对类功能予以线程包装，用于异步调用如何方法。
        /// 在本线程调用下的控件调用，需通过UI控件的Invoke/BegainInvoke方法更新。
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns>返回请求流程是否执行完成</returns>
        public static bool parseXMLData(DataSourceMHub datasource, string fpath, ref Dictionary<string, string> dataMapping, long lid, long plid)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPaht = System.IO.Path.GetFullPath(fpath);
            try
            {
                XmlDocument xDoc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                using (XmlReader xRead = XmlReader.Create(fullPaht))
                {
                    xDoc.Load(xRead);
                    parseXMLMappingInfo(datasource, xDoc, ref dataMapping, Convert.ToString(lid, 10), Convert.ToString(plid, 10));
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Info("ERROR: XML文件导入失败！ ", ex);
                MessageBox.Show("ERROR: XML文件导入失败！ " + ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }
        public static void parseXMLMappingInfo(DataSourceMHub datasource, XmlDocument xDoc, ref Dictionary<string, string> dataMapping, string lid, string plid)
        {
            XmlNodeList strainChildNodes = xDoc.DocumentElement.ChildNodes;
            XmlNode strainNode = null;
            while (strainChildNodes.Count > 0)
            {
                XmlNode node = strainChildNodes[0];
                if (node.ChildNodes.Count <= 0)
                    break;
                strainChildNodes = node.ChildNodes;
                strainNode = node;
            }
            List<string> attrNameList = new List<string>(strainChildNodes.Count);
            foreach (XmlNode strainChildNode in strainChildNodes)
            {
                attrNameList.Add(strainChildNode.Name);
            }
            XmlNodeList strainParentList = xDoc.GetElementsByTagName(strainNode.Name);
            /////////////////////////////////////////////////////////////////////////////
            if (dataMapping != null && dataMapping.Count > 0)
            {
                foreach(XmlNode strain in strainParentList)//循环的strains -> strain
                {
                    Dictionary<string, string> mapValues = new Dictionary<string, string>();
                    foreach (XmlNode attrNode in strain.ChildNodes)//循环的是strain -> strainAttr
                    { 
                        string xmlAttrName = attrNode.Name;
                        string xmlAttrValue = attrNode.InnerText;
                        string dbName = dataMapping[xmlAttrName]; 
                        if (dbName != null && xmlAttrValue!=null && xmlAttrValue.Length>0)
                            mapValues[dbName] = xmlAttrValue;
                    }   
                    mapValues[CTDRecordA.CTD_LID] = lid;
                    mapValues[CTDRecordA.CTD_PLID] = plid;
                    long nuid = LocalRecordMHub.mergeRecord(datasource, mapValues);
                } 
            }
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
