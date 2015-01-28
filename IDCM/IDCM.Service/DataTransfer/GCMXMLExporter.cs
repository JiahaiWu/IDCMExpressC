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

        public bool exportXML(DataGridView dgv, string xpath)
        {
            DataTable dt = GetDgvToTable(dgv);
            string XMLStr = ConvertDataTableToXML(dt);
            StringBuilder strbuilder = new StringBuilder();
            using (FileStream fs = new FileStream(xpath, FileMode.Create))
            {
                strbuilder.Append(XMLStr);
                if (strbuilder.Length > 0)
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                    BinaryWriter bw = new BinaryWriter(fs);
                    fs.Write(info, 0, info.Length);
                    strbuilder.Length = 0;
                }
            }
            return true;
        }

        public static DataTable GetDgvToTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();

            for (int count = 1; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < dgv.Columns.Count - 1; countsub++)
                {
                    int j = 1;
                    string cellStr = dgv.Rows[count].Cells[j++].Value.ToString();
                    if (cellStr == null) cellStr = "";
                    dr[countsub] = cellStr;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private string ConvertDataTableToXML(DataTable xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                writer = new XmlTextWriter(stream, Encoding.Default);
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                UTF8Encoding utf = new UTF8Encoding();
                return utf.GetString(arr).Trim();
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }     
    }
}
