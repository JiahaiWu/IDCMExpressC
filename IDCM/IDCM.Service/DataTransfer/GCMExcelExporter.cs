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
        /// <summary>
        /// 导出到excel，数据源DataTable
        /// </summary>
        /// <param name="fpath">导出路径</param>
        /// <param name="strainViewList">需要导出的DataTable</param>
        /// <param name="exportDetail">是否导出strain_tree</param>
        /// <param name="gcmSiteHolder">底层GCMSiteMHub句柄，用于获取strain_tree，封装GCM账户信息</param>
        /// <returns></returns>
        public bool exportExcel(string fpath, DataTable strainViewList, bool exportDetail, Common.GCMSiteMHub gcmSiteHolder)
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
                
                //填充列头
                IRow columnHead = sheet.CreateRow(0);
                for (int i = 0; i < strainViewList.Columns.Count;i++ )
                {
                    ICell Icell = columnHead.CreateCell(i, CellType.String);
                    Icell.SetCellValue(strainViewList.Columns[i].ColumnName);
                }

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
                        StrainView sv = GCMDataMHub.strainViewQuery(gcmSiteHolder, strainID);
                        Dictionary<string, object> maps = sv.ToDictionary();
                        if(i==0)mergeMapKeyToColumn(columnHead, maps, startIndex);
                        mergeMapValueToIrow(Irow, maps, startIndex);
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
        /// <summary>
        /// 导出到excel，数据源DataGridViewSelectedRowCollection
        /// </summary>
        /// <param name="fpath">导出路径</param>
        /// <param name="selectedRows">需要导出的DataGridViewSelectedRowCollection</param>
        /// <param name="exportDetail">是否导出strain_tree</param>
        /// <param name="gcmSiteHolder">底层GCMSiteMHub句柄，用于获取strain_tree，封装GCM账户信息</param>
        /// <returns></returns>
        public bool exportExcel(string fpath, DataGridViewSelectedRowCollection selectedRows, bool exportDetail, Common.GCMSiteMHub gcmSiteHolder)
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

                IRow columnHead = sheet.CreateRow(0);
                DataGridView dgv = selectedRows[0].DataGridView;
                for (int i = 1; i < dgv.Columns.Count; i++)
                {
                    ICell Icell = columnHead.CreateCell(i-1, CellType.String);
                    Icell.SetCellValue(dgv.Columns[i].Name);
                }
                int startIndex = selectedRows[0].Cells.Count;
                //填充dgv单元格内容
                int IrowIndex = 0;
                for (int ridx = selectedRows.Count - 1; ridx >= 0; ridx--)
                {
                    DataGridViewRow dgvRow = selectedRows[ridx];
                    IRow Irow = sheet.CreateRow(IrowIndex + 1);
                    for (int j = 1; j < dgvRow.Cells.Count; j++)
                    {
                        ICell Icell = Irow.CreateCell(j-1, CellType.String);
                        Icell.SetCellValue(Convert.ToString(dgvRow.Cells[j].Value));
                    }
                    if (exportDetail)
                    {
                        string strainID = Convert.ToString(dgvRow.Cells[1].Value);
                        StrainView sv = GCMDataMHub.strainViewQuery(gcmSiteHolder, strainID);
                        Dictionary<string, object> strain_treeMap = sv.ToDictionary();
                        if (IrowIndex == 0) mergeMapKeyToColumn(columnHead, strain_treeMap, startIndex);
                        mergeMapValueToIrow(Irow, strain_treeMap, startIndex);
                    }
                    IrowIndex++;
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
        /// <summary>
        /// 合并strain_tree中的key到数据源中的列
        /// 说明：
        /// 1：此方法只用于合并strain_tree中的列，数据源中的列构建不在此方法的考虑中
        /// </summary>
        /// <param name="dataSourceColumn">数据源中的列，构建好的IRow</param>
        /// <param name="strain_treeMap">strain_tree，属性名->属性值映射集合</param>
        /// <param name="startIndex">追加的开始位置</param>
        private void mergeMapKeyToColumn(IRow dataSourceColumn, Dictionary<string, object> strain_treeMap, int startIndex)
        {
            foreach (KeyValuePair<string, object> svEntry in strain_treeMap)
            {
                if (svEntry.Value is string)
                {
                    ICell Icell = dataSourceColumn.CreateCell(startIndex++,CellType.String);
                    Icell.SetCellValue(svEntry.Key);
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        string columnName = svEntry.Key + "_" + subEntry.Key;
                        ICell Icell = dataSourceColumn.CreateCell(startIndex++, CellType.String);
                        Icell.SetCellValue(columnName);
                    }
                }
            }
        }
        /// <summary>
        /// 合并一个strain_tree中的value到数据源的一行数据中
        /// 说明：
        /// 1：此方法只用于向数据源的一行数据中追加strain_tree中数据，数据源中的数据构建不在此方法的考虑中
        /// </summary>
        /// <param name="dataSourceRow">数据源中的一行数据，构建好的IRow</param>
        /// <param name="strain_treeMap">strain_tree，属性名->属性值映射集合</param>
        /// <param name="startIndex">追加的开始位置</param>
        private void mergeMapValueToIrow(IRow dataSourceRow, Dictionary<string, object> strain_treeMap, int startIndex)
        {
            foreach (KeyValuePair<string, object> svEntry in strain_treeMap)
            {
                if (svEntry.Value is string)
                {
                    ICell Icell = dataSourceRow.CreateCell(startIndex++, CellType.String);
                    Icell.SetCellValue(Convert.ToString(svEntry.Value));
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        ICell Icell = dataSourceRow.CreateCell(startIndex++, CellType.String);
                        string value = Convert.ToString(subEntry.Value);
                        Icell.SetCellValue(value);
                    }
                }
            }            
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
