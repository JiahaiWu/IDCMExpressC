using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.Forms;
using IDCM.Service.Common;
using IDCM.Data.Base;
using IDCM.Service;
using IDCM.Modules;
using IDCM.Service.BGHandler;
using IDCM.Service.Utils;
using IDCM.Core;
using IDCM.Service.UIM;

namespace IDCM.ViewManager
{
    /// <summary>
    /// HomeView布局管理器及动态表现事务调度中心
    /// @author JiahaiWu 2014-10-15
    /// </summary>
    public class HomeViewManager : ManagerA
    {
        #region 构造&析构
        public HomeViewManager()
        {
            homeView = new HomeView();
            homeView.setManager(this);

            homeView.Load += OnHomeView_Load;
            homeView.Shown += OnHomeView_Shown;

            frontFindDlg = new LocalFrontFindDlg(homeView.getItemGridView());
            frontFindDlg.setCellHit += new LocalFrontFindDlg.SetHit<DataGridViewCell>(setDGVCellHit);
            frontFindDlg.cancelCellHit += new LocalFrontFindDlg.CancelHit<DataGridViewCell>(cancelDGVCellHit);

            catBuilder = new LocalCatBuilder(homeView.getBaseTree(), homeView.getLibTree());
            datasetBuilder = new LocalDataSetBuilder(homeView.getItemGridView(), homeView.getAttachTabControl());
            searchBuilder = new LocalDBSearchBuilder(homeView.getDBSearchPanel(), homeView.getSearchSpliter());
        }

        ~HomeViewManager()
        {
            dispose();
        }
        #endregion
        #region 实例对象保持部分

        
        //页面窗口实例
        private volatile HomeView homeView = null;
        private volatile LocalCatBuilder catBuilder = null;
        private volatile LocalDataSetBuilder datasetBuilder = null;
        private volatile LocalDBSearchBuilder searchBuilder = null;
        private LocalFrontFindDlg frontFindDlg = null;
        
        #endregion
        #region 接口实例化部分
        public override void dispose()
        {
            _isDisposed = true;
            if (catBuilder != null)
            {
                catBuilder.Dispose();
                catBuilder = null;
            }
            if (datasetBuilder != null)
            {
                datasetBuilder.Dispose();
                datasetBuilder = null;
            }
            if (searchBuilder != null)
            {
                searchBuilder.Dispose();
                searchBuilder = null;
            }
            if (homeView != null && !homeView.IsDisposed)
            {
                homeView.Close();
                homeView.Dispose();
                homeView = null;
            }
            if (frontFindDlg != null && !frontFindDlg.IsDisposed)
            {
                frontFindDlg.Close();
                frontFindDlg.Dispose();
                frontFindDlg = null;
            }
        }
        /// <summary>
        /// 对象实例化初始化方法
        /// </summary>
        /// <returns></returns>
        public override bool initView(bool activeShow = true)
        {
            if (homeView == null || homeView.IsDisposed)
            {
                homeView = new HomeView();
                homeView.setManager(this);
                catBuilder = new LocalCatBuilder(homeView.getBaseTree(), homeView.getLibTree());
                datasetBuilder = new LocalDataSetBuilder(homeView.getItemGridView(), homeView.getAttachTabControl());
                searchBuilder = new LocalDBSearchBuilder(homeView.getDBSearchPanel(), homeView.getSearchSpliter());
            }
            if (DataSourceHolder.InWorking)
            {
                if (activeShow)
                {
                    homeView.WindowState = FormWindowState.Maximized;
                    homeView.Show();
                    homeView.Activate();
                }
                else
                {
                    homeView.Hide();
                }
                return true;
            }
            dispose();
            return false;
        }
        public override void setMaxToNormal()
        {
            if (homeView.WindowState.Equals(FormWindowState.Maximized))
                homeView.WindowState = FormWindowState.Normal;
        }
        public override void setToMaxmize(bool activeFront = false)
        {
            homeView.WindowState = FormWindowState.Maximized;
            if (activeFront)
            {
                homeView.Show();
                homeView.Activate();
            }
        }
        public override void setMdiParent(Form pForm)
        {
            homeView.MdiParent = pForm;
        }
        public override bool isDisposed()
        {
            if (_isDisposed == false)
            {
                _isDisposed = (homeView == null || homeView.Disposing || homeView.IsDisposed);
            }
            return _isDisposed;
        }
        public override bool isActive()
        {
            if (homeView == null || homeView.Disposing || homeView.IsDisposed)
                return false;
            else
                return homeView.Visible;
        }
        #endregion
        #region 接管视图组件的关键的事件处理区
        public void OnHomeView_Load(object sender, EventArgs e)
        {
            //加载默认的分类目录树展示
            catBuilder.loadTreeSet();
            updateCatRecCount();
        }
        public void OnHomeView_Shown(object sender, EventArgs e)
        {
            //加载用数据记录
            loadDataSetView(homeView.getBaseTree().Nodes[0]);
            //resize for data view
            homeView.getItemGridView().AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //dataGridView_items.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None);
            homeView.getItemGridView().AllowUserToResizeColumns = true;
        }

        #endregion
        public void loadDataSetView(TreeNode tnode)
        {
            datasetBuilder.loadDataSetView();
            noteCurSelectedNode(tnode);
        }
        /// <summary>
        /// 导入Excel数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void importData(string fpath)
        {
            Dictionary<string, string> dataMapping = new Dictionary<string, string>();
            AbsHandler eih = null;
            if (fpath.ToLower().EndsWith("xls") || fpath.ToLower().EndsWith(".xlsx"))
            {                
                if (datasetBuilder.checkForExcelImport(fpath, ref dataMapping,homeView))
                    eih = new ExcelImportHandler(DataSourceHolder.DataSource,fpath,ref dataMapping, CatalogNode.REC_UNFILED, CatalogNode.REC_ALL);
            }
            if(fpath.ToLower().EndsWith("xml") || fpath.ToLower().EndsWith(".xml"))
            {
                if (datasetBuilder.checkForXMLImport(fpath, ref dataMapping, homeView))
                    eih = new XMLImportHandler(DataSourceHolder.DataSource,fpath,ref dataMapping, CatalogNode.REC_UNFILED, CatalogNode.REC_ALL);
            }
            if (eih != null)
            {
                eih.addHandler(new UpdateHomeDataViewHandler(DataSourceHolder.DataSource, catBuilder.RootNode_unfiled, homeView.getItemGridView()));
                UpdateHomeLibCountHandler uhlch = new UpdateHomeLibCountHandler(DataSourceHolder.DataSource, homeView.getLibTree(), homeView.getBaseTree());
                eih.addHandler(uhlch);
                uhlch.addHandler(new SelectDataRowHandler(DataSourceHolder.DataSource, homeView.getItemGridView(), homeView.getAttachTabControl()));
                DWorkMHub.callAsyncHandle(eih);
            }
        }
        /// <summary>
        /// 导出数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void exportData(ExportType etype, string fpath)
        {
            KeyValuePair<string, int> lastQuery = LocalRecordMHub.getLastDGVRQuery();
            AbsHandler handler = null;
            DataGridViewSelectedRowCollection selectedRows = homeView.getItemGridView().SelectedRows;
            if (selectedRows != null && selectedRows.Count>0)
            {
                switch (etype)
                {
                    case ExportType.Excel:
                        handler = new ExcelExportHandler(DataSourceHolder.DataSource, fpath, selectedRows);
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    case ExportType.JSONList:
                        handler = new JSONListExportHandler(DataSourceHolder.DataSource, fpath, selectedRows);
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    case ExportType.TSV:
                        handler = new TextExportHandler(DataSourceHolder.DataSource, fpath, selectedRows, "\t");
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    case ExportType.CSV:
                        handler = new TextExportHandler(DataSourceHolder.DataSource, fpath, selectedRows, ",");
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    case ExportType.XML:
                        handler = new XMLExportHandler(DataSourceHolder.DataSource, fpath, selectedRows);
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    default:
                        MessageBox.Show("Unsupport export type!");
                        break;
                }
            }
            else 
            {
                switch (etype)
                {
                    case ExportType.Excel:
                        handler = new ExcelExportHandler(DataSourceHolder.DataSource, fpath, lastQuery.Key, lastQuery.Value);
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    case ExportType.JSONList:
                        handler = new JSONListExportHandler(DataSourceHolder.DataSource, fpath, lastQuery.Key, lastQuery.Value);
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    case ExportType.TSV:
                        handler = new TextExportHandler(DataSourceHolder.DataSource, fpath, lastQuery.Key, lastQuery.Value, "\t");
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    case ExportType.CSV:
                        handler = new TextExportHandler(DataSourceHolder.DataSource, fpath, lastQuery.Key, lastQuery.Value, ",");
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    case ExportType.XML:
                        handler = new XMLExportHandler(DataSourceHolder.DataSource, fpath, lastQuery.Key, lastQuery.Value);
                        DWorkMHub.callAsyncHandle(handler);
                        break;
                    default:
                        MessageBox.Show("Unsupport export type!");
                        break;
                }
            }   
        }
        /// <summary>
        /// 更新分类目录关联文档数显示
        /// </summary>
        /// <param name="focusNode"></param>
        public void updateCatRecCount(TreeNode focusNode = null)
        {
            UpdateHomeLibCountHandler ulch = null;
            if (focusNode == null)
                ulch = new UpdateHomeLibCountHandler(DataSourceHolder.DataSource, homeView.getLibTree(), homeView.getBaseTree());
            else
                ulch = new UpdateHomeLibCountHandler(DataSourceHolder.DataSource, focusNode);
            DWorkMHub.callAsyncHandle(ulch);
        }
        public void selectViewRecord(DataGridViewRow dgvr)
        {
            datasetBuilder.selectViewRecord(dgvr);
        }
        public void trashDataSet(TreeNode filteNode, int newlid = CatalogNode.REC_TRASH)
        {
            if (filteNode.Equals(catBuilder.RootNode_trash))
            {
                datasetBuilder.dropDataSet(filteNode);
            }
            else
            {
                datasetBuilder.trashDataSet(filteNode, newlid);
            }
            UpdateHomeDataViewHandler uhdvh = new UpdateHomeDataViewHandler(DataSourceHolder.DataSource, filteNode, homeView.getItemGridView());
            UpdateHomeLibCountHandler uhlch = new UpdateHomeLibCountHandler(DataSourceHolder.DataSource, homeView.getLibTree(), homeView.getBaseTree());
            uhdvh.addHandler(uhlch);
            DWorkMHub.callAsyncHandle(uhdvh);
        }
        public void deleteNode(TreeNode treeNode)
        {
            datasetBuilder.trashDataSet(treeNode, CatalogNode.REC_TRASH);
            catBuilder.deleteNode(treeNode);
            UpdateHomeDataViewHandler uhdvh = new UpdateHomeDataViewHandler(DataSourceHolder.DataSource,catBuilder.RootNode_all, homeView.getItemGridView());
            UpdateHomeLibCountHandler uhlch = new UpdateHomeLibCountHandler(DataSourceHolder.DataSource, homeView.getLibTree(), homeView.getBaseTree());
            uhdvh.addHandler(uhlch);
            DWorkMHub.callAsyncHandle(uhdvh);
        }
        public void addGroup(TreeNode treeNode)
        {
            catBuilder.addGroup(treeNode);
        }
        public void addGroupSet(TreeNode treeNode)
        {
            catBuilder.addGroupSet(treeNode);
        }
        public void renameNode(TreeNode treeNode, string label)
        {
            catBuilder.renameNode(treeNode, label);
        }

        public void noteCurSelectedNode(TreeNode node)
        {
            bool needUpdateData = catBuilder.noteCurSelectedNode(node);
            if (needUpdateData)
            {
                updateDataSet(node);
            }
        }
        public void CopyClipboard()
        {
            DataObject d = homeView.getItemGridView().GetClipboardContent();
            Clipboard.SetDataObject(d);
        }
        /// <summary>
        /// This will be moved to the util class so it can service any paste into a DGV
        /// </summary>
        public void PasteClipboard()
        {
            datasetBuilder.PasteClipboard();
        }
        /// <summary>
        /// 根据指定的数据集合加载数据报表显示
        /// </summary>
        public void updateDataSet(TreeNode filterNode)
        {
            datasetBuilder.noteDataSetLib(filterNode); //待考虑顺序问题///////////
            UpdateHomeDataViewHandler uhdvh = new UpdateHomeDataViewHandler(DataSourceHolder.DataSource,filterNode, homeView.getItemGridView());
            uhdvh.addHandler(new UpdateHomeLibCountHandler(DataSourceHolder.DataSource, filterNode));
            uhdvh.addHandler(new SelectDataRowHandler(DataSourceHolder.DataSource, homeView.getItemGridView(), homeView.getAttachTabControl()));
            DWorkMHub.callAsyncHandle(uhdvh);
        }
        /// <summary>
        /// 将选中列提交至GCM目录的处理入口方法
        /// 说明：
        /// 该方法实现主要包括以下四部分内容
        /// 1.验证选择行记录
        /// 2.验证GCM登录状态
        /// 3.获取本地数据字段至GCM目标字段的映射关系
        /// 4.建立上传目标数据至GCM目录事务（内置临时XML导出及更新菌种资源软链接消息）
        /// </summary>
        public void uploadSelectionToGCM()
        {
            DataGridViewSelectedRowCollection selectedRows = homeView.getItemGridView().SelectedRows;
            AuthInfo authInfo = DataSourceHolder.getLoginAuthInfo();
            if (authInfo != null && authInfo.LoginFlag == true) //登录成功
            {
                if (selectedRows != null && selectedRows.Count > 0)
                {
                    Dictionary<string, string> dataMapping = new Dictionary<string, string>();
                    if (datasetBuilder.checkForGCMImport(ref dataMapping))
                    {
                        LocalDataUploadHandler lduh = new LocalDataUploadHandler(DataSourceHolder.DataSource, DataSourceHolder.GCMHolder, selectedRows, dataMapping);
                        DWorkMHub.callAsyncHandle(lduh);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please sign in brfore this operation request.", "Sign In Notice", MessageBoxButtons.OK);
            }
        }
        public void showDBDataSearch()
        {
            searchBuilder.showDBDataSearch();
        }
        public void doDBDataSearch()
        {
            string whereCmd = searchBuilder.buildWhereCmd();
            datasetBuilder.doDBDataSearch(whereCmd);
        }
        private void setDGVCellHit(DataGridViewCell cell)
        {
            if (cell.Visible == false)
                return;
            cell.DataGridView.EndEdit();
            int colCount = DGVUtil.getTextColumnCount(cell.DataGridView);
            DataGridViewCell rightCell = cell.DataGridView.Rows[cell.RowIndex].Cells[colCount - 1];
            while (rightCell.Visible == false && rightCell.ColumnIndex > -1)
            {
                rightCell = rightCell.OwningRow.Cells[rightCell.ColumnIndex - 1];
            }
            cell.DataGridView.CurrentCell = rightCell;
            cell.DataGridView.CurrentCell = cell;
            cell.Selected = true;
            cell.DataGridView.BeginEdit(true);
        }
        private void cancelDGVCellHit(DataGridViewCell cell)
        {
            if (cell.Visible == false)
                return;
            cell.DataGridView.EndEdit();
            cell.Selected = false;
        }
        public void frontDataSearch()
        {
            if (frontFindDlg == null || frontFindDlg.IsDisposed)
            {
                frontFindDlg = new LocalFrontFindDlg(homeView.getItemGridView());
                frontFindDlg.setCellHit += new LocalFrontFindDlg.SetHit<DataGridViewCell>(setDGVCellHit);
                frontFindDlg.cancelCellHit += new LocalFrontFindDlg.CancelHit<DataGridViewCell>(cancelDGVCellHit);
            }
            frontFindDlg.Show();
            frontFindDlg.Visible = true;
            frontFindDlg.Activate();
        }
        /// <summary>
        /// 更新目标记录的归档目录属性信息
        /// </summary>
        /// <param name="newlid"></param>
        /// <param name="newplid"></param>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        public int updateCTCRecordLid(int newlid, int newplid = CatalogNode.REC_UNFILED, long rid = -1)
        {
           return LocalRecordMHub.updateCTCRecordLid(DataSourceHolder.DataSource, newlid, newplid, rid);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="uid"></param>
        public int deleteRec(long rid)
        {
            return LocalRecordMHub.deleteRec(DataSourceHolder.DataSource, rid);
        }
        /// <summary>
        /// 根据当前焦点的TreeNode筛选可用的右键菜单显示列表项
        /// </summary>
        /// <param name="cms"></param>
        /// <param name="snode"></param>
        public void filterContextMenuItems(ContextMenuStrip cms, TreeNode snode)
        {
            LocalCatBuilder.filterContextMenuItems(cms, snode);
        }
        /// <summary>
        /// 更新数据记录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cellVal"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public int updateAttrVal(string rid, string cellVal, string attrName)
        {
            return LocalRecordMHub.updateAttrVal(DataSourceHolder.DataSource, rid, cellVal, attrName);
        }
        public void activeGCMView()
        {
            DWorkMHub.note(AsyncMessage.RequestGCMView);
        }
        public void quickSearch(string findTerm)
        {
            DataGridViewCell ncell = datasetBuilder.quickSearch(findTerm);
            if (ncell != null)
                setDGVCellHit(ncell);
        }
        public void frontSearchNext()
        {
            frontFindDlg.findDown();
        }
        public void frontSearchPrev()
        {
            frontFindDlg.findRev();
        }

        public void addNewRecord()
        {
            datasetBuilder.addNewRecord();
        }
        public TreeNode SelectedNode_Current
        {
            get { return catBuilder != null ? catBuilder.SelectedNode_Current : null; }
        }
        public long CURRENT_RID
        {
            get { return datasetBuilder.CURRENT_RID; }
            set { datasetBuilder.CURRENT_RID=value; }
        }
        public long CURRENT_LID
        {
            get { return datasetBuilder.CURRENT_LID; }
        }
        public TreeNode RootNode_unfiled
        {
            get
            {
                return catBuilder.RootNode_unfiled;
            }
        }
    }
}
