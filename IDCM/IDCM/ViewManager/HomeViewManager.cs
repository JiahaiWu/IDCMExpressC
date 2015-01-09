using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.Forms;
using IDCM.Data.Base;

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
            frontFindDlg = new LocalFrontFindDlg(homeView.getItemGridView());
            frontFindDlg.setCellHit += new LocalFrontFindDlg.SetHit<DataGridViewCell>(setDGVCellHit);
            frontFindDlg.cancelCellHit += new LocalFrontFindDlg.CancelHit<DataGridViewCell>(cancelDGVCellHit);

            //libBuilder = new LocalLibBuilder(homeView.getBaseTree(), homeView.getLibTree());
            //datasetBuilder = new LocalDataSetBuilder(homeView.getItemGridView(), homeView.getAttachTabControl());
            //searchBuilder = new LocalDBSearchBuilder(homeView.getDBSearchPanel(), homeView.getSearchSpliter());
            //BackProgressIndicator.addIndicatorBar(homeView.getProgressBar());
        }
        public static HomeViewManager getInstance()
        {
            //ManagerI hvm = IDCMAppContext.MainManger.getManager(typeof(HomeViewManager));
            //return hvm == null ? null : (hvm as HomeViewManager);
            return null;
        }
        ~HomeViewManager()
        {
            dispose();
        }
        #endregion
        #region 实例对象保持部分

        
        //页面窗口实例
        private volatile HomeView homeView = null;
        //private volatile LocalLibBuilder libBuilder = null;
        //private volatile LocalDataSetBuilder datasetBuilder = null;
        //private volatile LocalDBSearchBuilder searchBuilder = null;
        private LocalFrontFindDlg frontFindDlg = null;
        
        #endregion
        #region 接口实例化部分
        public override void dispose()
        {
            _isDisposed = true;
            //if (libBuilder != null)
            //{
            //    libBuilder.Dispose();
            //    libBuilder = null;
            //}
            //if (datasetBuilder != null)
            //{
            //    datasetBuilder.Dispose();
            //    datasetBuilder = null;
            //}
            //if (searchBuilder != null)
            //{
            //    searchBuilder.Dispose();
            //    searchBuilder = null;
            //}
            if (homeView != null && !homeView.IsDisposed)
            {
                //BackProgressIndicator.removeIndicatorBar(homeView.getProgressBar());
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
                //libBuilder = new LocalLibBuilder(homeView.getBaseTree(), homeView.getLibTree());
                //datasetBuilder = new LocalDataSetBuilder(homeView.getItemGridView(), homeView.getAttachTabControl());
                //searchBuilder = new LocalDBSearchBuilder(homeView.getDBSearchPanel(), homeView.getSearchSpliter());
            }
            //if (CustomTColDefDAM.checkTableSetting())
            //{
            //    if (activeShow)
            //    {
            //        homeView.WindowState = FormWindowState.Maximized;
            //        homeView.Show();
            //        homeView.Activate();
            //    }
            //    else
            //    {
            //        homeView.Hide();
            //    }
            //    return true;
            //}
            //dispose();
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

        public void loadDataSetView(TreeNode tnode)
        {
            //datasetBuilder.loadDataSetView();
            //noteCurSelectedNode(tnode);
        }
        public void loadTreeSet()
        {
            //libBuilder.loadTreeSet();
        }
        /// <summary>
        /// 导入数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void importData(string fpath)
        {
            if (fpath.ToLower().EndsWith("xls") || fpath.ToLower().EndsWith(".xlsx"))
            {
                //ExcelImportHandler eih = new ExcelImportHandler(fpath, LibraryNodeDAM.REC_UNFILED, LibraryNodeDAM.REC_ALL);
                //eih.addHandler(new UpdateHomeDataViewHandler(libBuilder.RootNode_unfiled, homeView.getItemGridView()));
                //UpdateHomeLibCountHandler uhlch = new UpdateHomeLibCountHandler(homeView.getLibTree(), homeView.getBaseTree());
                //eih.addHandler(uhlch);
                //uhlch.addHandler(new SelectDataRowHandler(homeView.getItemGridView(), homeView.getAttachTabControl()));
                //CmdConsole.call(eih, CmdConsole.CmdReqOption.L);
            }
        }
        /// <summary>
        /// 导出数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void exportData(ExportType etype, string fpath)
        {
            ////DataGridView itemDGV = homeView.getItemGridView();
            //KeyValuePair<string, int> lastQuery = QueryCmdCache.getLastDGVRQuery();
            //AbsHandler handler = null;
            //switch (etype)
            //{
            //    case ExportType.Excel:
            //        handler = new ExcelExportHandler(fpath, lastQuery.Key,lastQuery.Value);
            //        CmdConsole.call(handler);
            //        break;
            //    case ExportType.JSONList:
            //        handler = new JSONListExportHandler(fpath, lastQuery.Key, lastQuery.Value);
            //        CmdConsole.call(handler);
            //        break;
            //    case ExportType.TSV:
            //        handler = new TextExportHandler(fpath, lastQuery.Key, lastQuery.Value, "\t");
            //        CmdConsole.call(handler);
            //        break;
            //    case ExportType.CSV:
            //        handler = new TextExportHandler(fpath, lastQuery.Key, lastQuery.Value, ",");
            //        CmdConsole.call(handler);
            //        break;
            //    default:
            //        MessageBox.Show("Unsupport export type!");
            //        break;
            //}
        }
        /// <summary>
        /// 更新分类目录关联文档数显示
        /// </summary>
        /// <param name="focusNode"></param>
        public void updateLibRecCount(TreeNode focusNode = null)
        {
            //UpdateHomeLibCountHandler ulch = null;
            //if (focusNode == null)
            //    ulch = new UpdateHomeLibCountHandler(homeView.getLibTree(),homeView.getBaseTree());
            //else
            //    ulch = new UpdateHomeLibCountHandler(focusNode);
            //CmdConsole.call(ulch);
        }
        public void selectViewRecord(DataGridViewRow dgvr)
        {
            //datasetBuilder.selectViewRecord(dgvr);
        }
        public void trashDataSet(TreeNode filteNode, int newlid = CatalogNode.REC_TRASH)
        {
            //if(filteNode.Equals(libBuilder.RootNode_trash))
            //{
            //    datasetBuilder.dropDataSet(filteNode);
            //}else{
            //    datasetBuilder.trashDataSet(filteNode, newlid);
            //}
            //UpdateHomeDataViewHandler uhdvh = new UpdateHomeDataViewHandler(filteNode, homeView.getItemGridView());
            //UpdateHomeLibCountHandler uhlch = new UpdateHomeLibCountHandler(homeView.getLibTree(), homeView.getBaseTree());
            //uhdvh.addHandler(uhlch);
            //CmdConsole.call(uhdvh, CmdConsole.CmdReqOption.L);
        }
        public void deleteNode(TreeNode treeNode)
        {
            //datasetBuilder.trashDataSet(treeNode, LibraryNodeDAM.REC_TRASH);
            //libBuilder.deleteNode(treeNode);
            //UpdateHomeDataViewHandler uhdvh = new UpdateHomeDataViewHandler(libBuilder.RootNode_all, homeView.getItemGridView());
            //UpdateHomeLibCountHandler uhlch = new UpdateHomeLibCountHandler(homeView.getLibTree(), homeView.getBaseTree());
            //uhdvh.addHandler(uhlch);
            //CmdConsole.call(uhdvh, CmdConsole.CmdReqOption.L);
        }
        public void addGroup(TreeNode treeNode)
        {
            //libBuilder.addGroup(treeNode);
        }
        public void addGroupSet(TreeNode treeNode)
        {
            //libBuilder.addGroupSet(treeNode);
        }
        public void renameNode(TreeNode treeNode, string label)
        {
            //libBuilder.renameNode(treeNode, label);
        }

        public void noteCurSelectedNode(TreeNode node)
        {
           //bool needUpdateData= libBuilder.noteCurSelectedNode(node);
           //if (needUpdateData)
           //{
           //    updateDataSet(node);
           //}
        }
        /// <summary>
        /// 根据指定的数据集合加载数据报表显示
        /// </summary>
        public void updateDataSet(TreeNode filterNode)
        {
            //datasetBuilder.noteDataSetLib(filterNode); //待考虑顺序问题///////////
            //UpdateHomeDataViewHandler uhdvh = new UpdateHomeDataViewHandler(filterNode, homeView.getItemGridView());
            //uhdvh.addHandler(new SelectDataRowHandler(homeView.getItemGridView(), homeView.getAttachTabControl()));
            //uhdvh.addHandler(new UpdateHomeLibCountHandler(filterNode));
            //CmdConsole.call(uhdvh);
        }
        public void showDBDataSearch()
        {
            //searchBuilder.showDBDataSearch();
        }
        public void doDBDataSearch()
        {
            //string whereCmd=searchBuilder.buildWhereCmd();
            //datasetBuilder.doDBDataSearch(whereCmd);
        }
        private void setDGVCellHit(DataGridViewCell cell)
        {
            cell.DataGridView.EndEdit();
            //int colCount = DGVUtil.getTextColumnCount(cell.DataGridView);
            //DataGridViewCell rightCell = cell.DataGridView.Rows[cell.RowIndex].Cells[colCount - 1];
            //cell.DataGridView.CurrentCell = rightCell;
            //cell.DataGridView.CurrentCell = cell;
            //cell.Selected = true;
            //cell.DataGridView.BeginEdit(true);
        }
        private void cancelDGVCellHit(DataGridViewCell cell)
        {
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
        public void quickSearch(string findTerm)
        {
           //DataGridViewCell ncell= datasetBuilder.quickSearch(findTerm);
           //if(ncell!=null)
           //    setDGVCellHit(ncell);
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
            //datasetBuilder.addNewRecord();
        }
        public TreeNode SelectedNode_Current
        {
            //get { return libBuilder != null ? libBuilder.SelectedNode_Current : null; }
            get
            {
                return null;
            }
        }
        public long CURRENT_RID
        {
            //get { return datasetBuilder.CURRENT_RID; }
            //set { datasetBuilder.CURRENT_RID=value; }
            get
            {
                return 0;
            }
        }
        public long CURRENT_LID
        {
            //get { return datasetBuilder.CURRENT_LID; }
            get
            {
                return 0;
            }
        }
        public TreeNode RootNode_unfiled
        {
            //get
            //{
            //    return libBuilder.RootNode_unfiled;
            //}
            get
            {
                return null;
            }
        }
    }
}
