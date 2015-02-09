using IDCM.Service.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace IDCM.Modules
{
    public class GCMDataSetBuilder
    {
        #region 构造&析构
        public GCMDataSetBuilder(DataGridView dgv)
        {
            this.itemDGV = dgv;
        }
        ~GCMDataSetBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            itemDGV = null;
        }
        #endregion
        #region 实例对象保持部分
        private DataGridView itemDGV=null;
        
        #endregion

        /// <summary>
        /// 添加一列CheckBoxColumn
        /// </summary>
        public void addCheckBoxColumn()
        {
            DataGridViewCheckBoxColumn chxCol = new DataGridViewCheckBoxColumn();
            chxCol.ReadOnly = true;
            chxCol.Resizable = DataGridViewTriState.False;
            chxCol.FlatStyle = FlatStyle.Popup;
            chxCol.CellTemplate.Style.ForeColor = Color.LightGray;
            chxCol.Width = 25;
            chxCol.Name = "　";
            itemDGV.Columns.Add(chxCol);
        }

        public DataGridViewCell quickSearch(string findTerm)
        {
            if (findTerm.Length > 0)
            {
                DataGridViewCell ncell = null;
                if (itemDGV.SelectedCells != null && itemDGV.SelectedCells.Count > 0)
                {
                    if (itemDGV.SelectedCells[0].Displayed)
                        ncell = itemDGV.SelectedCells[0];
                }
                while ((ncell = nextTextCell(ncell)) != null)
                {
                    string cellval = DGVUtil.getCellValue(ncell);
                    if (cellval.ToLower().Contains(findTerm.ToLower()))
                    {
                        return ncell;
                    }
                }
                if (ncell == null)
                {
                    MessageBox.Show("It's reached the end, and no subsequent matches.");
                }
            }
            return null;
        }

        /// <summary>
        /// 下一个单元格定位，如定位失败返回null
        /// </summary>
        /// <returns></returns>
        private DataGridViewCell nextTextCell(DataGridViewCell cell = null)
        {
            DataGridViewCell ncell = null;
            int rowCount = DGVUtil.getRowCount(itemDGV);
            int columnCount = DGVUtil.getTextColumnCount(itemDGV);
            int rowIndex = cell == null ? 0 : cell.RowIndex;
            int colIndex = cell == null ? 0 : cell.ColumnIndex + 1;
            if (colIndex >= columnCount)
            {
                rowIndex = rowIndex + 1;
                colIndex = 0;
            }
            if (colIndex < columnCount && rowIndex < rowCount)
            {
                DataGridViewRow dgvr = itemDGV.Rows[rowIndex];
                if (!dgvr.IsNewRow)
                {
                    ncell = dgvr.Cells[colIndex];
                    if (ncell.Visible && ncell is DataGridViewTextBoxCell)
                    {
                        return ncell;
                    }
                    else
                        return nextTextCell(ncell);
                }
            }
            return ncell;
        }

        /// <summary>
        /// 从剪贴板粘贴文本型数据记录到目标区域
        /// This will be moved to the util class so it can service any paste into a DGV
        /// </summary>
        internal void PasteClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');
                int iFail = 0, iRow = itemDGV.CurrentCell.RowIndex;
                int iCol = itemDGV.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                foreach (string line in lines)
                {
                    if (iRow < itemDGV.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this.itemDGV.ColumnCount)
                            {
                                oCell = itemDGV[iCol + i, iRow];
                                if (!oCell.ReadOnly)
                                {
                                    if (oCell.Value == null || oCell.Value.ToString() != sCells[i])
                                    {
                                        oCell.Value = Convert.ChangeType(sCells[i], oCell.ValueType);
                                        oCell.Style.BackColor = Color.Tomato;
                                    }
                                    else
                                        iFail++;
                                }
                            }
                            else
                            { break; }
                        }
                        iRow++;
                    }
                    else
                    { break; }
                    if (iFail > 0)
                        MessageBox.Show(string.Format("{0} updates failed due to read only column setting", iFail));
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }



        /// <summary>
        /// DGV转DataTable,此方法不是通用方法，构建的dataTable是从gcm dgv的列1开始
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        public DataTable DgvToTable(DataGridView dgv)
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
                for (int countsub = 1; countsub < dgv.Columns.Count; countsub++)
                {
                    string cellStr = null;
                    object cellValueObj = dgv.Rows[count].Cells[countsub].Value;
                    if (cellValueObj == null)
                        cellStr = "";
                    else
                        cellStr = cellValueObj.ToString();
                    dr[countsub-1] = cellStr;
                    Console.Write(cellStr);
                }
                dt.Rows.Add(dr);
                //if (dt.Rows.Count == 10) return dt;
            }
            return dt;
        }
    }
}
