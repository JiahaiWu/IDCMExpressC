using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.DataTransfer
{
    class GCMJSONExporter
    {
        public bool exportJSONList(DataGridView dgv,string xpath)
        {
            DataTable dt = GetDgvToTable(dgv);
            
            StringBuilder strbuilder = new StringBuilder();
            using (FileStream fs = new FileStream(xpath, FileMode.Create))
            {
                string json = JsonConvert.SerializeObject(dt);
                strbuilder.Append(json);
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
                for (int countsub = 0; countsub < dgv.Columns.Count-1; countsub++)
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
    }
}
