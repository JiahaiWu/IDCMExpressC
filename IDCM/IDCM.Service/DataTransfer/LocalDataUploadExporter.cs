using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Service.Common;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using IDCM.Data.Base;
using System.Data;

namespace IDCM.Service.DataTransfer
{
    public class LocalDataUploadExporter
    {
        /// <summary>
        /// 从DataGridViewSelectedRowCollection中选取的行数据，到处XML形式文档数据
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="selectedRows"></param>
        /// <param name="dbLinkMaps"></param>
        /// <param name="xmldata"></param>
        /// <returns></returns>
        internal bool exportGCMXML(DataSourceMHub datasource, DataGridViewSelectedRowCollection selectedRows, Dictionary<string, int> dbLinkMaps, out string xmldata)
        {
            bool res = false;
            using (MemoryStream xmlStream = new MemoryStream())
            {
                res = exportGCMXML(datasource, selectedRows, dbLinkMaps, xmlStream);
                if (res)
                {
                    xmldata = SysConstants.defaultEncoding.GetString(xmlStream.ToArray());
                }
                else
                    xmldata = null;
            }
            return res;
        }
        /// <summary>
        /// 从DataGridViewSelectedRowCollection中选取的行数据，到处XML形式文档数据
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="selectedRows"></param>
        /// <param name="dbLinkMaps"></param>
        /// <param name="ms"></param>
        /// <returns></returns>
        internal bool exportGCMXML(DataSourceMHub datasource, DataGridViewSelectedRowCollection selectedRows, Dictionary<string, int> dbLinkMaps, MemoryStream xmlStream)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
                XmlDocument xmlDoc = new XmlDocument();
                strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                strbuilder.Append("<strains>\r\n");
                ///////////////////
                for (int ridx = selectedRows.Count - 1; ridx >= 0; ridx--)
                {
                    DataGridViewRow dgvRow = selectedRows[ridx];
                    string recordId = dgvRow.Cells[CTDRecordA.CTD_RID].Value as string;
                    DataTable table = LocalRecordMHub.queryCTDRecord(datasource, null, recordId);
                    foreach (DataRow row in table.Rows)
                    {
                        XmlElement xmlEle = convertToXML(xmlDoc, dbLinkMaps, row);
                        strbuilder.Append(xmlEle.OuterXml).Append("\r\n");
                    }
                    if (++count % 100 == 0)
                    {
                        byte[] info = SysConstants.defaultEncoding.GetBytes(strbuilder.ToString());
                        xmlStream.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                }
                strbuilder.Append("</strains>");
                if (strbuilder.Length > 0)
                {
                    byte[] info = SysConstants.defaultEncoding.GetBytes(strbuilder.ToString());
                    xmlStream.Write(info, 0, info.Length);
                    strbuilder.Length = 0;
                }
                xmlStream.Position = 0;
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
        /// 根据字段将一行记录转换成xmlElement
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="maps"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private static XmlElement convertToXML(XmlDocument xmlDoc, Dictionary<string, int> dbLinkMaps, DataRow row)
        {
            XmlElement strainEle = xmlDoc.CreateElement("strain");
            foreach (KeyValuePair<string, int> mapEntry in dbLinkMaps)
            {
                XmlElement attrEle = xmlDoc.CreateElement(mapEntry.Key);
                attrEle.InnerText = row[mapEntry.Value].ToString();
                strainEle.AppendChild(attrEle);
            }
            return strainEle;
        }
        
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();  
    }
}
