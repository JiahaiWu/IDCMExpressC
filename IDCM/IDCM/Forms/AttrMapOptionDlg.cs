using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.ServiceBL.Common;
using IDCM.ServiceBL.Common.Converter;
using IDCM.ControlMBL.Utilities;

namespace IDCM.Forms
{
    public partial class AttrMapOptionDlg : Form
    {
        public AttrMapOptionDlg()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设置初始化映射源和映射目标字符串集合，并指定有效映射返回字典的引用对象
        /// </summary>
        /// <param name="xlscols"></param>
        /// <param name="dbList"></param>
        /// <param name="mapping"></param>
        public void setInitCols(List<string> xlscols,List<string> dbList,ref Dictionary<string,string> mapping)
        {
            this.srcCols = xlscols;
            this.destCols = dbList;
            this.mapping = mapping;
            computeSimilarMapping();
        }
        /// <summary>
        /// 根据字符串源和目标集合，使用编辑距离的计算方法计算相似度，并以一定的阈值通过筛选相似的映射对
        /// </summary>
        /// <param name="threshold"></param>
        public void computeSimilarMapping(double threshold = 0.7)
        {
            Dictionary<ObjectPair<string, string>, double> mappingEntries = new Dictionary<ObjectPair<string, string>, double>();
            List<string> baseList = new List<string>();
            foreach (string str in destCols)
            {
                baseList.Add(CVNameConverter.toViewName(str));
            }
            StringSimilarity.computeSimilarMap(srcCols, baseList, ref mappingEntries);
            mapping.Clear();
            foreach (KeyValuePair<ObjectPair<string, string>, double> kvpair in mappingEntries)
            {
                if (kvpair.Value >= threshold)
                {
                    mapping[kvpair.Key.Val] = CVNameConverter.toDBName(kvpair.Key.Key);
                }
            }
            foreach (string col in srcCols)
            {
                if (!mapping.ContainsKey(col))
                {
                    mapping[col] = null;
                }
            }
            this.dataGridView_map.Rows.Clear();
            foreach (KeyValuePair<string, string> mappair in mapping)
            {
                this.dataGridView_map.Rows.Add(new string[] { mappair.Key, null, null, CVNameConverter.toViewName(mappair.Value), null });
            }
            radioButton_similarity.Checked = true;
        }
        /// <summary>
        /// 更新引用的字典的有效映射的返回映射对
        /// </summary>
        public void setExtractMapping()
        {
            HashSet<string> baseSet = new HashSet<string>(destCols);
            mapping.Clear();
            foreach (string col in srcCols)
            {
                if (baseSet.Contains(CVNameConverter.toDBName(col)))
                {
                    mapping[col] = CVNameConverter.toDBName(col);
                }
                else
                {
                    mapping[col] = null;
                }
            }
            this.dataGridView_map.Rows.Clear();
            foreach (KeyValuePair<string, string> mappair in mapping)
            {
                this.dataGridView_map.Rows.Add(new string[] { mappair.Key, null, null, CVNameConverter.toViewName(mappair.Value), null });
            }
            radioButton_exact.Checked = true;
        }
        /// <summary>
        /// 取消操作事件处理并关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            mapping.Clear();
            this.Close();
        }
        /// <summary>
        /// 确认映射对配置并关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 启用模糊匹配模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_similarity_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_similarity.Checked)
                computeSimilarMapping();
        }
        /// <summary>
        /// 启用精确匹配模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_exact_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_exact.Checked)
                setExtractMapping();
        }

        /// <summary>
        /// 自动行号实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_map_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_map.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGridView_map.RowHeadersDefaultCellStyle.Font, rectangle,
                dataGridView_map.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            dataGridView_map.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }
        /// <summary>
        /// 映射编辑事件处理入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_map_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;
                if (dataGridView_map.Columns[e.ColumnIndex].HeaderText.Equals("Unbound"))
                {
                    DataGridViewCell dgvcell = dataGridView_map.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvcell.Value = null;
                    string col = dataGridView_map.Rows[e.RowIndex].Cells[0].Value.ToString();
                    mapping[col] = null;
                    dataGridView_map.Rows[e.RowIndex].Cells[3].Value = null;
                    radioButton_custom.Checked = true;
                }
                else if (dataGridView_map.Columns[e.ColumnIndex].HeaderText.Equals("Config"))
                {
                    toolStripComboBox_dest.Items.AddRange(unboundDestCols());
                    toolStripComboBox_dest.SelectedIndex = 0;
                    ControlUtil.ClearEvent(toolStripComboBox_dest, "SelectedIndexChanged");
                    toolStripComboBox_dest.Click += delegate(object tsender, EventArgs te) { toolStripComboBox_dest_Changed(tsender, te, e.ColumnIndex, e.RowIndex); };
                    contextMenuStrip_destList.Show(MousePosition);
                }
            }
        }
        /// <summary>
        /// 追加映射对配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        private void toolStripComboBox_dest_Changed(object sender, EventArgs e, int columnIndex, int rowIndex)
        {
            string stext=toolStripComboBox_dest.SelectedText;
            if (stext!=null)
            {
                dataGridView_map.Rows[rowIndex].Cells[3].Value =stext;
                mapping[dataGridView_map.Rows[rowIndex].Cells[0].Value.ToString()] = CVNameConverter.toDBName(stext);
            }
        }
        /// <summary>
        /// 获取未匹配的目标字符串集合
        /// </summary>
        /// <returns></returns>
        private string[] unboundDestCols()
        {
            List<string> res = new List<string>();
            HashSet<string> dests=new HashSet<string>(mapping.Values);
            foreach (string col in destCols)
            {
                if (!dests.Contains(col))
                    res.Add(CVNameConverter.toViewName(col));
            }
            return res.ToArray();
        }
        private List<string> srcCols = null;
        private List<string> destCols = null;
        private Dictionary<string, string> mapping = null;
    }
}
