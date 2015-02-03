using IDCM.Data.Base;
using IDCM.Service.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace IDCM.Service.DataTransfer
{
    class GCMXMLExporter
    {
        public bool exportXML(string filepath, DataTable strainViewList, bool exportDetail, Common.GCMSiteMHub gcmSiteHolder)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\r");
                    strbuilder.Append("<strains>\n\r");
                    ///////////////////
                    foreach(DataRow row in strainViewList.Rows)
                    {
                        XmlElement strainEle = xmlDoc.CreateElement("strain");
                        foreach(DataColumn column in strainViewList.Columns)
                        {
                            XmlElement attrEle = xmlDoc.CreateElement(column.ColumnName);
                            attrEle.InnerText = row[column.ColumnName] as string;
                            strainEle.AppendChild(attrEle);
                        }
                        if (exportDetail)
                        {
                            string strainID = row[0] as string;
                            StrainView sv = getStrainView(gcmSiteHolder, strainID);
                            Dictionary<string, object> maps = sv.ToDictionary();
                            mergeDataToXmlDocument(xmlDoc, strainEle, maps);
                        }
                        strbuilder.Append(strainEle.OuterXml).Append("\n\r").Append("</strains>");
                        if (strbuilder.Length > 0)
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                            BinaryWriter bw = new BinaryWriter(fs);
                            fs.Write(info, 0, info.Length);
                            strbuilder.Length = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR::" + ex.Message + "\n" + ex.StackTrace);
                log.Error(ex);
                return false;
            }
            return true;
        }
        internal void exportXML(string filepath, DataGridViewSelectedRowCollection selectedRows, bool exportDetail, GCMSiteMHub gcmSiteHolder)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\r");
                    strbuilder.Append("<strains>\n\r");
                    ///////////////////
                    int rowIndex = 0;
                    foreach (DataGridViewRow row in selectedRows)
                    {
                        XmlElement strainEle = xmlDoc.CreateElement("strain");
                        foreach (DataGridViewColumn column in row.DataGridView.Columns)
                        {
                            XmlElement attrEle = xmlDoc.CreateElement(column.Name);
                            attrEle.InnerText = row.Cells[column.Name].Value as string;
                            strainEle.AppendChild(attrEle);
                        }
                        if (exportDetail)
                        {
                            string strainID = row.Cells[1].Value as string;
                            StrainView sv = getStrainView(gcmSiteHolder, strainID);
                            Dictionary<string, object> maps = sv.ToDictionary();
                            mergeDataToXmlDocument(xmlDoc, strainEle, maps);
                        }
                        strbuilder.Append(strainEle.OuterXml).Append("\n\r").Append("</strains>");
                        if (strbuilder.Length > 0)
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                            BinaryWriter bw = new BinaryWriter(fs);
                            fs.Write(info, 0, info.Length);
                            strbuilder.Length = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR::" + ex.Message + "\n" + ex.StackTrace);
                log.Error(ex);
                return false;
            }
            return true;
        }
        private void mergeDataToXmlDocument(XmlDocument xmlDoc, XmlElement strainEle, Dictionary<string, object> maps)
        {
            foreach(KeyValuePair<string,object> strainTreeNode in maps)
            {
                XmlElement attrEle = null;
                if (strainTreeNode.Value is string)
                {
                    attrEle = xmlDoc.CreateElement(strainTreeNode.Key);
                    attrEle.InnerText = strainTreeNode.Value as string;
                    strainEle.AppendChild(attrEle);
                }
                if(strainTreeNode.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subNode in strainTreeNode.Value as Dictionary<string, dynamic>)
                    {
                        string nodeName = strainTreeNode.Key + "_" + subNode.Key;
                        attrEle = xmlDoc.CreateElement(nodeName);
                        attrEle.InnerText = Convert.ToString(subNode.Value);
                        strainEle.AppendChild(attrEle);
                    }
                }
                if(strainTreeNode.Value == null)
                {
                    attrEle = xmlDoc.CreateElement(strainTreeNode.Key);
                    attrEle.InnerText = "";
                    strainEle.AppendChild(attrEle);
                }              
            }
        }

        private StrainView getStrainView(GCMSiteMHub gcmSiteHolder, string strainID)
        {
            GCMDataMHub gcmDataHub = new GCMDataMHub();
            return gcmDataHub.strainViewQuery(gcmSiteHolder, strainID);
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();  
    }
}
