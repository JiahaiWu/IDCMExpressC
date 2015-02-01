using IDCM.Data.Base;
using IDCM.Service.Common;
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
        internal bool exportText(string fpath, DataTable strainViewList, bool exportDetail, Common.GCMSiteMHub gcmSiteHolder)
        {
            try
            {
                IWorkbook workbook = null;
                string suffix = Path.GetExtension(fpath).ToLower();
                if (suffix.Equals(".xlsx"))
                    workbook = new XSSFWorkbook();
                else
                    workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Core Datasets");
                HashSet<int> excludes = new HashSet<int>();
                
                int startIndex = strainViewList.Columns.Count;
                //填充dgv单元格内容
                for (int i = 0; i < strainViewList.Rows.Count; i++)
                {
                    IRow Irow = sheet.CreateRow(i+1);
                    for (int j = 0; j < strainViewList.Columns.Count; j++)
                    {
                        ICell Icell = Irow.CreateCell(j, CellType.String);
                        Icell.SetCellValue(Convert.ToString(strainViewList.Rows[i][j]));
                    }
                    if(exportDetail)
                    {
                        string strainID = Convert.ToString(strainViewList.Rows[i][0]);
                        StrainView sv = getStrainView(gcmSiteHolder,strainID);
                        Dictionary<string, object> maps = sv.ToDictionary();
                        
                        buildIrow(Irow, maps, startIndex);
                    }
                }
                using (FileStream fs = File.Create(fpath))
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

        private void buildIrow(IRow Irow, Dictionary<string, object> maps, int startIndex)
        {
            foreach (KeyValuePair<string, object> svEntry in maps)
            {
                if (svEntry.Value is string)
                {
                    ICell Icell = Irow.CreateCell(startIndex++, CellType.String);
                    Icell.SetCellValue(Convert.ToString(svEntry.Value));
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        ICell Icell = Irow.CreateCell(startIndex++, CellType.String);
                        string value = Convert.ToString(subEntry.Value);
                        Icell.SetCellValue(value);
                    }
                }
            }            
        }

        internal bool exportText(string fpath, DataGridViewSelectedRowCollection selectedRows, bool exportDetail, Common.GCMSiteMHub gcmSiteHolder)
        {
            try
            {
                IWorkbook workbook = null;
                string suffix = Path.GetExtension(fpath).ToLower();
                if (suffix.Equals(".xlsx"))
                    workbook = new XSSFWorkbook();
                else
                    workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Core Datasets");
                HashSet<int> excludes = new HashSet<int>();

                int startIndex = selectedRows[0].Cells.Count;
                //填充dgv单元格内容
                for (int i = 0; i < selectedRows.Count; i++)
                {
                    IRow Irow = sheet.CreateRow(i);
                    DataGridViewRow dgvRow = selectedRows[i];
                    for (int j = 1; j < dgvRow.Cells.Count; j++)
                    {
                        ICell Icell = Irow.CreateCell(j, CellType.String);
                        Icell.SetCellValue(Convert.ToString(dgvRow.Cells[j].Value));
                    }
                    if (exportDetail)
                    {
                        string strainID = Convert.ToString(dgvRow.Cells[1].Value);
                        StrainView sv = getStrainView(gcmSiteHolder, strainID);
                        Dictionary<string, object> maps = sv.ToDictionary();

                        buildIrow(Irow, maps, startIndex);
                    }
                }
                using (FileStream fs = File.Create(fpath))
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

        private void addStrainViewColumnStr(StringBuilder strBuilder, string spliter, Dictionary<string, object> maps)
        {
            foreach (KeyValuePair<string, object> svEntry in maps)
            {
                if (svEntry.Value is string)
                {
                    strBuilder.Append(svEntry.Key).Append(spliter);
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        string columnName = svEntry.Key + "/" + subEntry.Key;
                        strBuilder.Append(columnName).Append(spliter);
                    }
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
