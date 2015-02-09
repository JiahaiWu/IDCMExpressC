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
        /// <summary>
        /// 导出数据到XML,数据源DataTable
        /// </summary>
        /// <param name="filepath">导出路径</param>
        /// <param name="strainViewList">数据源</param>
        /// <param name="exportDetail">是否导出stran_tree</param>
        /// <param name="gcmSiteHolder">底层GCMSiteMHub句柄，用于获取strain_tree，封装GCM账户信息</param>
        /// <returns></returns>
        public bool exportXML(string filepath, DataTable strainViewList, bool exportDetail, Common.GCMSiteMHub gcmSiteHolder)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
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
                            StrainView sv = GCMDataMHub.strainViewQuery(gcmSiteHolder, strainID);
                            Dictionary<string, object> maps = sv.ToDictionary();
                            mergeDataToXmlDocument(xmlDoc, strainEle, maps);
                        }
                        strbuilder.Append(strainEle.OuterXml).Append("\n\r");
                        if (++count % 100 == 0)
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                            BinaryWriter bw = new BinaryWriter(fs);
                            fs.Write(info, 0, info.Length);
                            strbuilder.Length = 0;
                        }
                    }
                    strbuilder.Append("</strains>");
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                    fs.Close();
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
        /// <summary>
        /// 导出数据到XML,数据源DataGridViewSelectedRowCollection
        /// </summary>
        /// <param name="filepath">导出路径</param>
        /// <param name="selectedRows">数据源</param>
        /// <param name="exportDetail">是否导出stran_tree</param>
        /// <param name="gcmSiteHolder">底层GCMSiteMHub句柄，用于获取strain_tree，封装GCM账户信息</param>
        /// <returns></returns>
        internal bool exportXML(string filepath, DataGridViewSelectedRowCollection selectedRows, bool exportDetail, GCMSiteMHub gcmSiteHolder)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\r");
                    strbuilder.Append("<strains>\n\r");
                    ///////////////////
                    for (int ridx = selectedRows.Count - 1; ridx >= 0; ridx--)
                    {
                        DataGridViewRow row = selectedRows[ridx];
                        int columnIndex = 0;
                        XmlElement strainEle = xmlDoc.CreateElement("strain");
                        foreach (DataGridViewColumn column in row.DataGridView.Columns)
                        {
                            if (columnIndex++ == 0)
                                continue;
                            XmlElement attrEle = xmlDoc.CreateElement(column.Name);
                            attrEle.InnerText = row.Cells[column.Name].Value as string;
                            strainEle.AppendChild(attrEle);
                        }
                        if (exportDetail)
                        {
                            string strainID = row.Cells[1].Value as string;
                            StrainView sv = GCMDataMHub.strainViewQuery(gcmSiteHolder, strainID);
                            Dictionary<string, object> maps = sv.ToDictionary();
                            mergeDataToXmlDocument(xmlDoc, strainEle, maps);
                        }
                        strbuilder.Append(strainEle.OuterXml).Append("\n\r");
                        if (++count % 100 == 0)
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                            BinaryWriter bw = new BinaryWriter(fs);
                            fs.Write(info, 0, info.Length);
                            strbuilder.Length = 0;
                        }
                    }
                    strbuilder.Append("</strains>");
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                    fs.Close();
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
        /// <summary>
        /// 向strain节点中插入strain_tree数据
        /// </summary>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="strainEle">strain节点</param>
        /// <param name="strain_treeMap">strain_tree</param>
        private void mergeDataToXmlDocument(XmlDocument xmlDoc, XmlElement strainEle, Dictionary<string, object> strain_treeMap)
        {
            foreach(KeyValuePair<string,object> strainTreeNode in strain_treeMap)
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
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();  
    }
}
