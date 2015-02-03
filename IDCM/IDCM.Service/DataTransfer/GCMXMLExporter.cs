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

        //public bool exportXML(DataSourceMHub datasource, string filepath, string cmdstr, int tcount, string spliter = " ")
        //{
        //    try
        //    {
        //        StringBuilder strbuilder = new StringBuilder();
        //        int count = 0;
        //        using (FileStream fs = new FileStream(filepath, FileMode.Create))
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n\r");
        //            strbuilder.Append("<strains>\n\r");
        //            Dictionary<string, int> maps = LocalRecordMHub.getCustomAttrDBMapping(datasource);
        //            ///////////////////
        //            int offset = 0;
        //            int stepLen = SysConstants.EXPORT_PAGING_COUNT;
        //            while (offset < tcount)
        //            {
        //                int lcount = tcount - offset > stepLen ? stepLen : tcount - offset;
        //                DataTable table = LocalRecordMHub.queryCTDRecordByHistSQL(datasource, cmdstr, lcount, offset);
        //                foreach (DataRow row in table.Rows)
        //                {
        //                    XmlElement xmlEle = convertToXML(xmlDoc, maps, row, spliter);
        //                    strbuilder.Append(xmlEle.OuterXml).Append("\n\r");
        //                    /////////////
        //                    if (++count % 100 == 0)
        //                    {
        //                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
        //                        BinaryWriter bw = new BinaryWriter(fs);
        //                        fs.Write(info, 0, info.Length);
        //                        strbuilder.Length = 0;
        //                    }
        //                }
        //                strbuilder.Append("</strains>");
        //                if (strbuilder.Length > 0)
        //                {
        //                    Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
        //                    BinaryWriter bw = new BinaryWriter(fs);
        //                    fs.Write(info, 0, info.Length);
        //                    strbuilder.Length = 0;
        //                }
        //                offset += lcount;
        //            }
        //            fs.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("ERROR::" + ex.Message + "\n" + ex.StackTrace);
        //        log.Error(ex);
        //        return false;
        //    }
        //    return true;
        //}
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();  
    }
}
