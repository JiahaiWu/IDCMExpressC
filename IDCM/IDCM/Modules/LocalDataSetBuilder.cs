using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Data.Base.Utils;
using IDCM.Service.Utils;
using IDCM.Core;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using IDCM.Forms;

namespace IDCM.Modules
{
    class LocalDataSetBuilder
    {
        #region 构造&析构
        /// <summary>
        /// 初始化个人资源目录树
        /// </summary>
        /// <param name="libTree"></param>
        public LocalDataSetBuilder(DataGridView dgv_items, TabControl tc_attach)
        {
            this.itemDGV = dgv_items;
            this.attachTC = tc_attach;

        }
        ~LocalDataSetBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            itemDGV = null;
            attachTC = null;
        }
        #endregion
        #region 实例对象保持部分
        private long CUR_LID = CatalogNode.REC_ALL;

        public long CURRENT_LID
        {
            get { return CUR_LID; }
        }
        private long CUR_PLID = CatalogNode.REC_ALL;

        public long CURRENT_PLID
        {
            get { return CUR_PLID; }
        }
        
        private long CUR_RID = -1L;

        public long CURRENT_RID
        {
            get { return CUR_RID; }
            set { CUR_RID=value; }
        }
        /// <summary>
        /// 当前表单显示的查询条件缓存
        /// </summary>
        private volatile string queryCondtion = null;

        public string QueryCondtion
        {
            get { return queryCondtion; }
        }

        #endregion

        /// <summary>
        /// 根据指定的索引位序更新显示附加的属性信息
        /// </summary>
        /// <param name="dgvr"></param>
        /// <param name="tc"></param>
        public void selectViewRecord(DataGridViewRow dgvr)
        {
            int rIdx = dgvr.DataGridView.Columns[CTDRecordA.CTD_RID.ToString()].Index;
            if (dgvr.Cells.Count > rIdx)
            {
                CUR_RID = Convert.ToInt64(dgvr.Cells[rIdx].FormattedValue.ToString());
                DataTable table = LocalRecordMHub.queryCTDRecord(null, CUR_RID.ToString());
                if (table.Rows.Count > 0)
                {
                    DataRow dr = table.Rows[0];
                    List<string> viewAttrs = LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource);
                    showReferences(viewAttrs, dr);
                }
            }
        }

        /// <summary>
        /// 根据指定的数据集合加载数据报表显示，指定的字段映射为空时则使用默认的字段映射显示规则。
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="dataset"></param>
        /// <param name="colMap"></param>
        public void loadDataSetView()
        {
            List<string> viewAttrs = LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource);//获取所有属性名称集合
            lock (LocalDataGridView_Lock)
            {
                if (itemDGV.ColumnCount > 0)
                {
                    DGVAsyncUtil.syncRemoveAllRow(itemDGV);
                    resetReferences(viewAttrs);
                }
                else
                {
                    //行列表头显示
                    loadDGVColumns(viewAttrs);
                    loadReferences(viewAttrs);
                }
            }
        }
        /// <summary>
        /// 标记目标数据报表关联文档目录键值
        /// </summary>
        public void noteDataSetLib(TreeNode filterNode)
        {
            long lid = Convert.ToInt64(filterNode.Name);

            if (filterNode.Level > 0)
            {
                CUR_LID = lid;
                CUR_PLID = Convert.ToInt64(filterNode.Parent.Name);
            }
            else
            {
                CUR_LID = lid;
                CUR_PLID = lid;
            }
        }
        /// <summary>
        /// 删除数据归档目标的数据记录，置入未分类目录
        /// </summary>
        /// <param name="filteNode"></param>
        /// <param name="lid"></param>
        public void trashDataSet(TreeNode filteNode, int newlid = CatalogNode.REC_TRASH)
        {
            List<string> viewAttrs = LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource);
            long lid = Convert.ToInt64(filteNode.Name);
            lock (LocalDataGridView_Lock)
            {
                if (filteNode.Level > 0)
                {
                    CUR_LID = lid;
                    CUR_PLID = Convert.ToInt64(filteNode.Parent.Name);
                }
                else
                {
                    CUR_LID = lid;
                    CUR_PLID = lid;
                }
                itemDGV.Rows.Clear();
                resetReferences(viewAttrs);
                string filterLids = lid.ToString();
                if (lid > 0)
                {
                    long[] lids = LocalRecordMHub.extractToLids(DataSourceHolder.DataSource,lid);
                    if (lids != null)
                    {
                        filterLids = "";
                        foreach (long _lid in lids)
                        {
                            filterLids += "," + _lid;
                        }
                        filterLids = filterLids.Substring(1);
                    }
                }
                //数据归档更新
                LocalRecordMHub.updateCTCRecordLid(DataSourceHolder.DataSource, newlid, CatalogNode.REC_ALL, filterLids);
            }
        }
        /// <summary>
        /// 删除数据归档目标的数据记录，置入未分类目录
        /// </summary>
        /// <param name="filteNode"></param>
        public void dropDataSet(TreeNode filteNode)
        {
            List<string> viewAttrs = LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource);
            long lid = Convert.ToInt64(filteNode.Name);
            lock (LocalDataGridView_Lock)
            {
                if (filteNode.Level > 0)
                {
                    CUR_LID = lid;
                    CUR_PLID = Convert.ToInt64(filteNode.Parent.Name);
                }
                else
                {
                    CUR_LID = lid;
                    CUR_PLID = lid;
                }
                itemDGV.Rows.Clear();
                resetReferences(viewAttrs);
                string filterLids = lid.ToString();
                if (lid > 0)
                {
                    long[] lids = LocalRecordMHub.extractToLids(DataSourceHolder.DataSource,lid);
                    if (lids != null)
                    {
                        filterLids = "";
                        foreach (long _lid in lids)
                        {
                            filterLids += "," + _lid;
                        }
                        filterLids = filterLids.Substring(1);
                    }
                }
                //数据归档更新
                LocalRecordMHub.dropCTCRecordLid(DataSourceHolder.DataSource, filterLids);
            }
        }
        
        /// <summary>
        /// 转换数据对象值到列表显示
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pCtd"></param>
        protected void loadCTableData(DataRow dr, List<string> viewAttrs)
        {
            string[] vals = new string[viewAttrs.Count];
            int index = 0;
            foreach (string attr in viewAttrs)
            {
#if DEBUG
                //Console.WriteLine("[DEBUG](loadCTableData) " + attr + "-->" + CustomTColMapDA.getDBOrder(attr) + ">>" + dr[CustomTColMapDA.getDBOrder(attr)].ToString());
#endif
                vals[index] = dr[LocalRecordMHub.getDBOrder(DataSourceHolder.DataSource, attr)].ToString();
                ++index;
            }
            DGVAsyncUtil.syncAddRow(itemDGV, vals);
        }
        /// <summary>
        /// 加载数据表头展示
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="attrs"></param>
        private void loadDGVColumns(List<string> viewAttrs)
        {
            DGVAsyncUtil.syncClearAll(itemDGV);
            //创建显性列属性
            foreach (string attr in viewAttrs)
            {
                int viewOrder = LocalRecordMHub.getViewOrder(DataSourceHolder.DataSource, attr);//返回属性显示位序
                Console.Write("##"+attr+"->"+viewOrder);
                if (viewOrder < CustomTColMap.MaxMainViewCount)
                {
                    CustomTColDef ctcd = LocalRecordMHub.getCustomTColDef(DataSourceHolder.DataSource,attr);
                    Type colType = RecordControlTypeConverter.getDGVColType(ctcd.AttrType);
                    DataGridViewColumn dgvCol = Activator.CreateInstance(colType) as DataGridViewColumn;
                    dgvCol.Name = ctcd.Attr;
                    dgvCol.HeaderText = CVNameConverter.toViewName(ctcd.Attr);
                    if (ctcd.Attr.Equals(CTDRecordA.CTD_RID) || attr.Equals(CTDRecordA.CTD_PLID) || attr.Equals(CTDRecordA.CTD_LID))
                    {
                        dgvCol.Visible = false;
                        dgvCol.Width = 0;
                    }
                    DGVAsyncUtil.syncAddCol(itemDGV, dgvCol);
                    if (viewOrder != dgvCol.Index)
                        LocalRecordMHub.updateViewOrder(DataSourceHolder.DataSource,attr, dgvCol.Index);
                }
            }
        }
        /// <summary>
        /// 加载附加注解字段名展示
        /// </summary> 
        /// <param name="dgv"></param>
        /// <param name="attrs"></param>
        private void loadReferences(List<string> viewAttrs)
        {
            //清理tabControl控件
            foreach (TabPage tp in attachTC.TabPages)
            {
                foreach (Control ictl in tp.Controls)
                {
                    ictl.Dispose();
                }
            }
            //重新build references Page
            TabPage tabPage = attachTC.TabPages["references"];
            int idx = 0;
            foreach (string attr in viewAttrs)
            {
                if (LocalRecordMHub.getViewOrder(DataSourceHolder.DataSource, attr) < CustomTColMap.MaxMainViewCount)
                {
                    ++idx;
                    continue;
                }
                CustomTColDef ctcd = LocalRecordMHub.getCustomTColDef(DataSourceHolder.DataSource, attr);
                Panel panel = new Panel();
                panel.Name = "referPanel_" + idx;
                panel.Dock = DockStyle.Top;
                panel.Margin = new Padding(0, 5, 0, 0);
                panel.BorderStyle = System.Windows.Forms.BorderStyle.None;
                panel.Height = 45;
                //panel.BorderStyle = BorderStyle.FixedSingle;
                TextBox label = new TextBox();
                label.ReadOnly = true;
                label.BorderStyle = System.Windows.Forms.BorderStyle.None;
                label.BackColor = Color.WhiteSmoke;
                label.Name = "referLabel_" + ctcd.Attr;
                string pattr =CVNameConverter.toViewName(ctcd.Attr);
                label.Text = pattr;
                label.Font = new Font(label.Font, label.Font.Style ^ FontStyle.Bold);
                label.Height = 14;
                label.Dock = DockStyle.Top;
                panel.Controls.Add(label);
                Type ctype = RecordControlTypeConverter.getControlType(ctcd.AttrType);
                Control control = Activator.CreateInstance(ctype) as Control;
                if (control is TextBox)
                {
                    (control as TextBox).BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                }
                control.Name = pattr;
                control.Dock = DockStyle.Bottom;
                control.Height = 20;
                control.Margin = new Padding(0, 1, 0, 0);
                control.TextChanged += Refers_record_TextChanged;
                panel.Controls.Add(control);
                tabPage.Controls.Add(panel);
                ++idx;
            }
            Label ridLabel = new Label();
            ridLabel.Name = "ctd_rid_Label";
            ridLabel.Visible = false;
            ridLabel.Height = 0;
            ridLabel.Dock = DockStyle.Top;
            ridLabel.Margin = new Padding(0, 0, 0, 0);
            tabPage.Controls.Add(ridLabel);
        }
        /// <summary>
        /// 根据指定的数据行置空所有附加的属性信息
        /// </summary>
        /// <param name="viewAttrs"></param>
        public void resetReferences(List<string> viewAttrs)
        {
            showReferences(viewAttrs, null);
        }
        /// <summary>
        /// 根据指定的数据行更新显示附加的属性信息
        /// </summary>
        /// <param name="viewAttrs"></param>
        /// <param name="dr"></param>
        /// dr为null则等效于清空表单操作
        public void showReferences(List<string> viewAttrs, DataRow dr = null)//参数是所有属性名称集合
        {
            TabPage tabPage = attachTC.TabPages["references"];
            if (dr != null)
            {
                (tabPage.Controls["ctd_rid_Label"] as Label).Text = dr[CTDRecordA.CTD_RID].ToString();
            }
            foreach (Control ctl in tabPage.Controls)//获取选项卡内所有集合
            {
                if (ctl is Panel)//如果空间是面板
                {
                    //获取，选择项卡中的集合名称从referPanel_开始处的索引
                    int idx = Convert.ToInt32(ctl.Name.Substring("referPanel_".Length));

                    //从viewAttrs[idx]第一位开始，viewAttrs[idx]-2个长度的字符串
                    string attr =CVNameConverter.toViewName(viewAttrs[idx]);//注意：
                    Control ictl = ctl.Controls[attr];
                    if (ictl != null)
                    {
                        if (ictl is TextBox)
                        {
                            (ictl as TextBox).Text = dr == null ? "" : dr[attr].ToString();
                        }
                        else if (ictl is ComboBox)
                        {
                            (ictl as ComboBox).FormatString = dr == null ? "" : dr[attr].ToString();
                        }
                        else if (ictl is DateTimePicker)
                        {
                            (ictl as DateTimePicker).CustomFormat = dr == null ? "" : dr[attr].ToString();
                        }
                    }
                }
            }
        }
        public DataGridViewCell quickSearch(string findTerm)
        {
            if (findTerm.Length > 0)
            {
                DataGridViewCell ncell = null;
                if (itemDGV.SelectedCells != null && itemDGV.SelectedCells.Count > 0)
                {
                    if(itemDGV.SelectedCells[0].Displayed)
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
        public void doDBDataSearch(string whereCmd)
        {
            //unimplemented
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
            if (colIndex < columnCount && rowIndex<rowCount)
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
        /// 添加新纪录，自动生成默认的数据字段值
        /// </summary>
        /// <param name="dgv"></param>
        public void addNewRecord()
        {
            long nuid = LocalRecordMHub.addNewRecord(DataSourceHolder.DataSource,CUR_LID, CURRENT_PLID);
            if (nuid > 0)
            {
                int idx = itemDGV.Rows.Add();
                itemDGV.Rows[idx].Cells[CTDRecordA.CTD_RID].Value = nuid;
                itemDGV.Rows[idx].Cells[CTDRecordA.CTD_LID].Value = CUR_LID;
                itemDGV.Rows[idx].Cells[CTDRecordA.CTD_PLID].Value = CURRENT_PLID;
            }
        }
        
        /// <summary>
        /// 伴随用户修改操作同步更新记录属性信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Refers_record_TextChanged(object sender, EventArgs e)
        {
            if ((sender as Control).Focused)
            {
                string rid = null;
                string attrName = null;
                string cellVal = null;
                if (sender is TextBox)
                {
                    TextBox tb = (sender as TextBox);
                    rid = (tb.Parent.Parent.Controls["ctd_rid_Label"] as Label).Text;
                    attrName = tb.Name;
                    cellVal = tb.Text.Trim();
                }
                else if (sender is ComboBox)
                {
                    ComboBox tb = (sender as ComboBox);
                    rid = (tb.Parent.Parent.Controls["ctd_rid_Label"] as Label).Text;
                    attrName = tb.Name;
                    cellVal = tb.FormatString.Trim();
                }
                else if (sender is DateTimePicker)
                {
                    DateTimePicker tb = (sender as DateTimePicker);
                    rid = (tb.Parent.Parent.Controls["ctd_rid_Label"] as Label).Text;
                    attrName = tb.Name;
                    cellVal = tb.Text.Trim();
                }
                if (rid.Length > 0 && cellVal.Length > 0)
                {
                    LocalRecordMHub.updateAttrVal(DataSourceHolder.DataSource,rid, cellVal, "[" + attrName + "]");
                }
            }
        }
        /// <summary>
        /// 解析指定的Excel文档，验证数据转换的属性映射条件.
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns></returns>
        public bool checkForExcelImport(string fpath,ref Dictionary<string, string> dataMapping)
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
                    return fetchSheetMappingInfo(dataSheet, ref dataMapping) && dataMapping.Count>0;
                }
            }
            catch (Exception ex)
            {
                log.Info("ERROR: Excel文件导入失败！ ", ex);
                MessageBox.Show("ERROR: Excel文件导入失败！ " + ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }
        /// <summary>
        /// 通过NPOI读取Excel文档，转换可识别内容至本地数据库中
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dgv"></param>
        private static bool fetchSheetMappingInfo(ISheet sheet, ref Dictionary<string, string> dataMapping)
        {
            int skipIdx = 1;
            if (sheet == null || sheet.LastRowNum < skipIdx) //no data
                return false;
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
            ///////////////////////////////////////////////////////////////
            using (AttrMapOptionDlg amoDlg = new AttrMapOptionDlg())
            {
                amoDlg.setInitCols(xlscols, LocalRecordMHub.getViewAttrs(DataSourceHolder.DataSource,false), ref dataMapping);
                amoDlg.ShowDialog();
                ///////////////////////////////////////////
                if (amoDlg.DialogResult == DialogResult.OK)
                    return true;
            }
            return false;
        }
        private DataGridView itemDGV;

        public DataGridView ItemDGV
        {
            get { return itemDGV; }
        }
        private TabControl attachTC;

        public TabControl AttachTC
        {
            get { return attachTC; }
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 本地表单数据视图控件的独占保持的共享锁对象
        /// </summary>
        public static object LocalDataGridView_Lock = new object();
    }
}
