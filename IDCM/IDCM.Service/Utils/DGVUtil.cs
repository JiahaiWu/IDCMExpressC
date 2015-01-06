using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.Utils
{
    public class DGVUtil
    {
        public static int getTextColumnCount(DataGridView dgv)
        {
            int columnCount = dgv.ColumnCount;
            for (int k = columnCount - 1; k > 0; k--)
            {
                if (!(dgv.Columns[k] is DataGridViewTextBoxColumn))
                    columnCount--;
            }
            return columnCount;
        }
        public static int getTextualColumnCount(DataGridView dgv)
        {
            int columnCount = dgv.ColumnCount;
            for (int k = columnCount - 1; k > 0; k--)
            {
                if (!(dgv.Columns[k] is DataGridViewTextBoxColumn || dgv.Columns[k] is DataGridViewComboBoxColumn || dgv.Columns[k] is DataGridViewLinkColumn))
                    columnCount--;
            }
            return columnCount;
        }
        public static int getRowCount(DataGridView dgv)
        {
            int rowCount = dgv.RowCount;
            if (rowCount > 0 && dgv.Rows[rowCount - 1].IsNewRow)
                return rowCount - 1;
            return rowCount;
        }
        public static string getCellValue(DataGridViewCell cell, string NULLValue = null)
        {
            if (cell == null || cell.Value == null)
                return NULLValue;
            else
                return cell.Value.ToString();
        }
        public static void setDGVCellHit(DataGridViewCell cell)
        {
            cell.DataGridView.EndEdit();
            int colCount = DGVUtil.getTextColumnCount(cell.DataGridView);
            DataGridViewCell rightCell = cell.DataGridView.Rows[cell.RowIndex].Cells[colCount - 1];
            cell.DataGridView.CurrentCell = rightCell;
            cell.DataGridView.CurrentCell = cell;
            cell.Selected = true;
            cell.DataGridView.BeginEdit(true);
        }
        public static void cancelDGVCellHit(DataGridViewCell cell)
        {
            cell.DataGridView.EndEdit();
            cell.Selected = false;
        }
    }
}
