using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.DataTransfer
{
    class GCMTextExporter
    {
        public bool exportText(string filepath, DataGridView dgv, string spliter = " ")
        {
            try
            {
                int rowsConut = dgv.Rows.Count;
                int columnCount = dgv.Columns.Count;

                StringBuilder strbuilder = new StringBuilder();

                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    for (int i = 0; i < rowsConut; i++)
                    {  
                        for (int j = 1; j < columnCount; j++)
                        { 
                            DataGridViewCell cell = dgv.Rows[i].Cells[j];
                            strbuilder.Append(cell.Value).Append(spliter);
                        }
                        strbuilder.Append("\n");
                    }
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show("ERROR::" + ex.Message + "\n" + ex.StackTrace);
                log.Error(ex);
                return false;
            }

            return true ;
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
