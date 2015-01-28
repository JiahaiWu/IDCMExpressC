using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Service.Utils;

namespace IDCM.Forms
{
    public partial class LocalFrontFindDlg : Form
    {
        public LocalFrontFindDlg()
        {
            InitializeComponent();
            if (lastFindTerm != null && lastFindTerm.Length > 0)
                this.comboBox_find.FormatString = lastFindTerm;
        }
        public LocalFrontFindDlg(params DataGridView[] dgvs)
        {
            dgvPool = new List<DataGridView>(dgvs.Length);
            dgvPool.AddRange(dgvs);
            InitializeComponent();
            if (lastFindTerm != null && lastFindTerm.Length > 0)
                this.comboBox_find.FormatString = lastFindTerm;
        }
        ~LocalFrontFindDlg()
        {
            foundCell = null;
            if (dgvPool != null && dgvPool.Count > 0)
            {
                dgvPool.Clear();
                dgvPool = null;
            }
        }
        private void FrontFindDlg_Load(object sender, EventArgs e)
        {
            this.comboBox_find.Focus();
        }
        private void comboBox_find_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                findDown();
            }
        }
        private void button_searchDown_Click(object sender, EventArgs e)
        {
            findDown();
        }

        private void button_findRev_Click(object sender, EventArgs e)
        {
            findRev();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            resetIndex(true);
        }
        public void findDown()
        {
            if (foundCell != null)
                cancelCellHit(foundCell);
            foundCell = null;
            string findTerm = this.comboBox_find.Text.Trim();
            if (findTerm.Length > 0)
            {
                if (!findTerm.Equals(lastFindTerm))
                    resetIndex();
                lastFindTerm = findTerm;
                DataGridViewCell ncell = null;
                while ((ncell = nextCell()) != null)
                {
                    string cellval = DGVUtil.getCellValue(ncell);
                    if (checkItemMatch(findTerm, cellval, checkBox_matchCase.Checked, checkBox_matchAll.Checked))
                    {
                        foundCell = ncell;//查找成功
                        setCellHit(ncell);
                        this.Hide();
                        break;
                    }
                }
                if (ncell == null)
                {
                    MessageBox.Show("It's reached the end, and traverse over.");
                    this.Hide();
                }
            }
            else
            {
                this.comboBox_find.FormatString = "";
                this.comboBox_find.Focus();
            }
        }
        public void findRev()
        {
            if (foundCell != null)
                cancelCellHit(foundCell);
            foundCell = null;
            string findTerm = this.comboBox_find.Text.Trim();
            if (findTerm.Length > 0)
            {
                if (!findTerm.Equals(lastFindTerm))
                    resetIndex();
                lastFindTerm = findTerm;
                DataGridViewCell ncell = null;
                while ((ncell = nextCell(true)) != null)
                {
                    string cellval = DGVUtil.getCellValue(ncell);
                    if (checkItemMatch(findTerm, cellval, checkBox_matchCase.Checked, checkBox_matchAll.Checked))
                    {
                        foundCell = ncell;//查找成功
                        setCellHit(ncell);
                        this.Hide();
                        break;
                    }
                }
                if (ncell == null)
                {
                    MessageBox.Show("It's reached the end, and traverse over.");
                    this.Hide();
                }
            }
            else
            {
                this.comboBox_find.FormatString = "";
                this.comboBox_find.Focus();
            }
        }
        /// <summary>
        /// 字符串单点匹配方法
        /// </summary>
        /// <param name="testTerm"></param>
        /// <param name="cellText"></param>
        /// <param name="matchCase"></param>
        /// <param name="matchAll"></param>
        /// <returns></returns>
        private bool checkItemMatch(string testTerm, string cellText, bool matchCase = false, bool matchAll = false)
        {
            if (cellText == null)
                return false;
            if (matchAll)
            {
                if (matchCase)
                    return testTerm.Equals(cellText);
                else
                    return testTerm.Equals(cellText, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                if (matchCase)
                    return cellText.Contains(testTerm);
                else
                    return cellText.ToLower().Contains(testTerm.ToLower());
            }
        }
        /// <summary>
        /// 下一个单元格定位，如定位失败返回null
        /// </summary>
        /// <returns></returns>
        private DataGridViewCell nextCell(bool reverse = false)
        {
            DataGridViewCell ncell = null;
            if (reverse == false)
            {
                int arrayIndex = found_arrayIndex < 0 ? 0 : found_arrayIndex;
                if (arrayIndex < dgvPool.Count)
                {
                    int rowIndex = found_rowIndex < 0 ? 0 : found_rowIndex;
                    if (rowIndex >= DGVUtil.getRowCount(dgvPool[arrayIndex]))
                    {
                        found_arrayIndex = arrayIndex + 1;
                        found_rowIndex = -2;
                        return nextCell(reverse);
                    }
                    int colIndex = found_colIndex < 0 ? 0 : found_colIndex + 1;
                    if (colIndex >= dgvPool[arrayIndex].ColumnCount-1)
                    {
                        found_rowIndex = rowIndex + 1;
                        found_colIndex = -2;
                        return nextCell(reverse);
                    }
                    if (arrayIndex < dgvPool.Count)
                    {
                        ncell = dgvPool[arrayIndex].Rows[rowIndex].Cells[colIndex];
                        if (ncell.Visible == false || !(ncell is DataGridViewTextBoxCell))
                        {
                            found_colIndex = colIndex+1;
                            return nextCell(reverse);
                        }
                        found_arrayIndex = arrayIndex;
                        found_rowIndex = rowIndex;
                        found_colIndex = colIndex;
                    }
                }
            }
            else
            {
                int arrayIndex = found_arrayIndex < -1 ? dgvPool.Count - 1 : found_arrayIndex;
                if (arrayIndex > -1)
                {
                    int rowIndex = found_rowIndex < -1 ? DGVUtil.getRowCount(dgvPool[arrayIndex]) - 1 : found_rowIndex;
                    if (rowIndex < 0)
                    {
                        found_arrayIndex = arrayIndex - 1;
                        found_rowIndex = -2;
                        return nextCell(reverse);
                    }
                    int colIndex = found_colIndex < -1 ? dgvPool[arrayIndex].ColumnCount - 1 : found_colIndex - 1;
                    if (colIndex < 0)
                    {
                        found_rowIndex = rowIndex - 1;
                        found_colIndex = -2;
                        return nextCell(reverse);
                    }
                    if (arrayIndex > -1)
                    {
                        ncell = dgvPool[arrayIndex].Rows[rowIndex].Cells[colIndex];
                        if (ncell.Visible == false || !(ncell is DataGridViewTextBoxCell))
                        {
                            found_colIndex = colIndex - 1;
                            return nextCell(reverse);
                        }
                        found_arrayIndex = arrayIndex;
                        found_rowIndex = rowIndex;
                        found_colIndex = colIndex;
                    }
                }
            }
            if (ncell == null)
            {
                resetIndex();
            }
            return ncell;
        }
        private void resetIndex(bool clearTerm = false)
        {
            found_arrayIndex = -2;
            found_rowIndex = -2;
            found_colIndex = -2;
            if (clearTerm)
            {
                comboBox_find.FormatString = "";
                checkBox_matchAll.Checked = false;
                checkBox_matchCase.Checked = false;
            }
        }
        public delegate void SetHit<T>(T fc);
        public delegate void CancelHit<T>(T fc);
        public event SetHit<DataGridViewCell> setCellHit;
        public event CancelHit<DataGridViewCell> cancelCellHit;
        private static List<DataGridView> dgvPool = null;
        private static string lastFindTerm = "";
        private static int found_arrayIndex = -2;
        private static int found_rowIndex = -2;
        private static int found_colIndex = -2;
        private static DataGridViewCell foundCell = null;
    }
}
