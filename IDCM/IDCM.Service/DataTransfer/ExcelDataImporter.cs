using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Threading;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Data.Base.Utils;

namespace IDCM.Service.DataTransfer
{
    class ExcelDataImporter
    {
        /// <summary>
        /// 解析指定的Excel文档，执行数据转换.
        /// 本方法调用对类功能予以线程包装，用于异步调用如何方法。
        /// 在本线程调用下的控件调用，需通过UI控件的Invoke/BegainInvoke方法更新。
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns>返回请求流程是否执行完成</returns>
        public static bool parseExcelData(string fpath, long lid, long plid)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPath = System.IO.Path.GetFullPath(fpath);
            IWorkbook workbook = null;
            try
            {
                using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    workbook = WorkbookFactory.Create(fs);
                    ISheet dataSheet = workbook.GetSheet("Core Datasets");
                    if (dataSheet == null)
                        dataSheet = workbook.GetSheetAt(0);
                    parseSheetInfo(dataSheet, Convert.ToString(lid, 10), Convert.ToString(plid, 10));
                }
            }
            catch (Exception ex)
            {
                log.Info("ERROR: Excel文件导入失败！ ", ex);
                MessageBox.Show("ERROR: Excel文件导入失败！ " + ex.Message + "\n" + ex.StackTrace);
            }
            return true;
        }
        /// <summary>
        /// 通过NPOI读取Excel文档，转换可识别内容至本地数据库中
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dgv"></param>
        public static void parseSheetInfo(ISheet sheet, string lid, string plid)
        {
            int skipIdx = 1;
            if (sheet == null || sheet.LastRowNum < skipIdx) //no data
                return;
            /////////////////////////////////////////////////////////
            IRow titleRow = sheet.GetRow(skipIdx - 1);
            int columnSize = titleRow.LastCellNum;
            int rowSize = sheet.LastRowNum;
            List<string> xlscols = new List<string>(columnSize);
            for (int i = titleRow.FirstCellNum; i < columnSize; i++)
            {
                ICell titleCell = titleRow.GetCell(i);
                if (titleCell != null && titleCell.ToString().Length > 0)
                {
                    string cellData = titleCell.ToString();
                    xlscols.Add(CVNameConverter.toViewName(cellData.Trim().ToLower()));
                }
                else
                {
                    xlscols.Add(null);
                }
            }
            /////////////////////////////////////////////////////////////////
            //AttrMapOptionDlg amoDlg = new AttrMapOptionDlg();
            //Dictionary<string, string> dataMapping = new Dictionary<string, string>();
            //amoDlg.setInitCols(xlscols, ColumnMappingHolder.getViewAttrs(false), ref dataMapping);
            //amoDlg.ShowDialog();
            /////////////////////////////////////////////
            //if (amoDlg.DialogResult == DialogResult.OK)
            //{
            //    amoDlg.Dispose();
            //    for (int i = skipIdx; i <= rowSize; ++i)
            //    {
            //        IRow row = sheet.GetRow(i);
            //        if (row == null) continue; //没有数据的行默认是null　
            //        ICell headCell = row.GetCell(row.FirstCellNum);
            //        if (headCell == null || headCell.ToString().Length == 0 || headCell.ToString().Equals("end!"))
            //            break;
            //        Dictionary<string, string> mapValues = new Dictionary<string, string>();
            //        for (int j = row.FirstCellNum; j < columnSize; j++)
            //        {
            //            if (row.GetCell(j) != null && xlscols[j] != null)
            //            {
            //                string cellData = row.GetCell(j).ToString().Trim();
            //                string mapName = null;
            //                dataMapping.TryGetValue(xlscols[j], out mapName);
            //                if (mapName != null)
            //                {
            //                    mapValues[mapName] = cellData;
            //                }
            //            }
            //        }
            //        mapValues[CTDRecordA.CTD_LID] = lid;
            //        mapValues[CTDRecordA.CTD_PLID] = plid;
            //        long nuid = CTDRecordDAM.mergeRecord(mapValues);
            //    }
            //}
            //else
            //    amoDlg.Dispose();
            //////////////////////////////////////////////////
            //事务逻辑拆分不够恰当，暂行阻断，有待补充
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
