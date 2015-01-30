using IDCM.Service.UIM;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.DataTransfer
{
    class GCMExcelExporter
    {
        public bool exportExcel(string filepath, DataGridView dgv)
        {
            int rowsCount = dgv.Rows.Count ;
            int columnCount = dgv.ColumnCount;
            
            try
            {
                IWorkbook workbook = null;
                string suffix = Path.GetExtension(filepath).ToLower();
                if (suffix.Equals(".xlsx"))
                    workbook = new XSSFWorkbook();
                else
                    workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Core Datasets");
                HashSet<int> excludes = new HashSet<int>();
                
                    //填充dgv单元格内容
                    for (int i = 0; i < rowsCount; i++)
                    {
                        IRow Irow = sheet.CreateRow(i);
                        for (int j = 0; j < columnCount; j++)
                        {
                            DataGridViewCell cell = dgv.Rows[i].Cells[j];
                            ICell Icell = Irow.CreateCell(j, CellType.String);
                            Icell.SetCellValue(Convert.ToString(cell.Value));
                        }
                    }
                using (FileStream fs = File.Create(filepath))
                {
                    workbook.Write(fs);
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
        public bool exportExcel(DataTable dTable,string filepath)
        {
            int rowsCount = dTable.Rows.Count;
            int columnCount = dTable.Columns.Count; ;
           try
            {
                IWorkbook workbook = null;
                string suffix = Path.GetExtension(filepath).ToLower();
                if (suffix.Equals(".xlsx"))
                    workbook = new XSSFWorkbook();
                else
                    workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Core Datasets");
                HashSet<int> excludes = new HashSet<int>();
                
                    //填充dgv单元格内容
                    for (int i = 0; i < rowsCount; i++)
                    {
                        IRow Irow = sheet.CreateRow(i);
                        string strainId = (string)dTable.Rows[i][0];
                        
                        for (int j = 0; j < columnCount; j++)
                        {
                            //DataGridViewCell cell = dTable.Rows[i].Cells[j];
                            ICell Icell = Irow.CreateCell(j, CellType.String);
                            Icell.SetCellValue(Convert.ToString(dTable.Rows[i][j]));
                        }
                    }
                using (FileStream fs = File.Create(filepath))
                {
                    workbook.Write(fs);
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
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
