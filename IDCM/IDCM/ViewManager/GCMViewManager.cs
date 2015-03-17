using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.Forms;
using IDCM.Service.Utils;
using IDCM.Service;
using IDCM.Service.Common;
using IDCM.Service.BGHandler;
using IDCM.Core;
using IDCM.Modules;
using IDCM.Data.Base;
using IDCM.Service.UIM;
using IDCM.Service.DataTransfer;
using IDCM.Service.POO;
using System.Data;

namespace IDCM.ViewManager
{
    public class GCMViewManager : ManagerA
    {
        #region 构造&析构
        public GCMViewManager()
        {
            gcmView = new GCMView();
            gcmView.setManager(this);

            gcmView.OnGCMViewShown += OnGCMViewShown;
            gcmView.OnItemsDGVCellClick += OnGCMViewItemsDGVCellClick;//点击cell查看strain节点树
            gcmView.OnStrainTreeNodeClick += OnGCMViewStrainTreeNodeClick;//查看树节点key value
            gcmView.OnRefreshData += OnGCMViewRefreshData;
            gcmView.OnActiveHomeView += OnGCMViewActiveHomeView;
            gcmView.OnRequestHelp += OnGCMViewRequestHelp;
            gcmView.OnSearchButtonClick += OnGCMViewSearchButtonClick;
            gcmView.OnExportData += OnGCMViewExportData;
            gcmView.OnCopyClipboard += OnGCMViewCopyClipboard;
            gcmView.OnFrontDataSearch += OnGCMViewFrontDataSearch;
            gcmView.OnFrontSearchNext += OnGCMViewFrontSearchNext;
            gcmView.OnFrontSearchPrev += OnGCMViewFrontSearchPrev;
            gcmView.OnQuickSearch += OnGCMViewQuickSearch;

            frontFindDlg = new GCMFrontFindDlg(gcmView.getItemGridView());
            frontFindDlg.setCellHit += new GCMFrontFindDlg.SetHit<DataGridViewCell>(DGVUtil.setDGVCellHit);
            frontFindDlg.cancelCellHit += new GCMFrontFindDlg.CancelHit<DataGridViewCell>(DGVUtil.cancelDGVCellHit);
           
            datasetBuilder = new GCMDataSetBuilder(gcmView.getItemGridView());
            searchBuilder = new GCMSearchBuilder(gcmView.getSearchPanel(), gcmView.getSearchSpliter());
            BackProgressIndicator.addIndicatorBar(gcmView.getProgressBar());//有待完善
        }

        ~GCMViewManager()
        {
            dispose();
        }
        #endregion
        #region 实例对象保持部分

        
        //页面窗口实例
        private volatile GCMView gcmView = null;
        private volatile GCMDataSetBuilder datasetBuilder = null;
        private volatile GCMSearchBuilder searchBuilder = null;
        private GCMFrontFindDlg frontFindDlg = null;
        
        #endregion
        #region 接口实例化部分
        public override void dispose()
        {
            _isDisposed = true;
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
            if (gcmView != null && !gcmView.IsDisposed)
            {
                BackProgressIndicator.removeIndicatorBar(gcmView.getProgressBar());
                gcmView.Close();
                gcmView.Dispose();
                gcmView = null;
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
            if (gcmView == null || gcmView.IsDisposed)
            {
                gcmView = new GCMView();
                gcmView.setManager(this);

                gcmView.OnGCMViewShown += OnGCMViewShown;
                gcmView.OnItemsDGVCellClick += OnGCMViewItemsDGVCellClick;//点击cell查看strain节点树
                gcmView.OnStrainTreeNodeClick += OnGCMViewStrainTreeNodeClick;//查看树节点key value
                gcmView.OnRefreshData += OnGCMViewRefreshData;
                gcmView.OnActiveHomeView += OnGCMViewActiveHomeView;
                gcmView.OnRequestHelp += OnGCMViewRequestHelp;
                gcmView.OnSearchButtonClick += OnGCMViewSearchButtonClick;
                gcmView.OnExportData += OnGCMViewExportData;
                gcmView.OnCopyClipboard += OnGCMViewCopyClipboard;
                gcmView.OnFrontDataSearch += OnGCMViewFrontDataSearch;
                gcmView.OnFrontSearchNext += OnGCMViewFrontSearchNext;
                gcmView.OnFrontSearchPrev += OnGCMViewFrontSearchPrev;
                gcmView.OnActiveHomeView += OnGCMViewQuickSearch;

                datasetBuilder = new GCMDataSetBuilder(gcmView.getItemGridView());
                BackProgressIndicator.addIndicatorBar(gcmView.getProgressBar());
                searchBuilder = new GCMSearchBuilder(gcmView.getSearchPanel(), gcmView.getSearchSpliter());
            }
            if (activeShow)
            {
                gcmView.WindowState = FormWindowState.Maximized;
                gcmView.Show();
                gcmView.Activate();
            }
            else
            {
                gcmView.Hide();
            }
            return true;
        }

        
        public override void setMaxToNormal()
        {
            if (gcmView.WindowState.Equals(FormWindowState.Maximized))
                gcmView.WindowState = FormWindowState.Normal;
        }
        public override void setToMaxmize(bool activeFront = false)
        {
            gcmView.WindowState = FormWindowState.Maximized;
            if (activeFront)
            {
                gcmView.Show();
                gcmView.Activate();
            }
        }
        public override void setMdiParent(Form pForm)
        {
            gcmView.MdiParent = pForm;
        }
        public override bool isDisposed()
        {
            if (_isDisposed==false)
            {
                _isDisposed= (gcmView==null || gcmView.Disposing || gcmView.IsDisposed);
            }
            return _isDisposed;
        }
        public override bool isActive()
        {
            if (gcmView == null || gcmView.Disposing || gcmView.IsDisposed)
                return false;
            else
                return gcmView.Visible;
        }
        #endregion
        #region GCM事件实现
        /// GCM第一次显示_加载GCM数据
        /// </summary>
        /// <param name="refresh"></param>
        public void OnGCMViewShown(object sender, IDCMViewEventArgs e)
        {
            //加载默认的数据报表展示
            loadDataSetView();
            //resize for data view
            gcmView.getItemGridView().AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            gcmView.getItemGridView().AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None);
            gcmView.getItemGridView().AllowUserToResizeColumns = true;
        }
        /// <summary>
        /// 查看Strain明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGCMViewItemsDGVCellClick(object sender, IDCMViewEventArgs e)
        {
            DataGridViewRow dgvRow = gcmView.getItemGridView().CurrentRow;
            String strainid = dgvRow.Cells["id"].FormattedValue.ToString();
            LoadGCMStrainViewHandler lsvh = new LoadGCMStrainViewHandler(DataSourceHolder.GCMHolder, strainid, gcmView.getRecordTree(), gcmView.getRecordList());
            DWorkMHub.callAsyncHandle(lsvh);
        }
        /// <summary>
        /// 查看GCM_TreeView节点明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGCMViewStrainTreeNodeClick(object sender, IDCMViewEventArgs e)
        {
            //e.values.length = 1 ### e.values[0] = TreeNodeMouseClickEventArgs.node
            GCMNodeDetailLoader.loadData(e.values[0], gcmView.getRecordList());
        }
        /// <summary>
        /// 激活HomeView视图
        /// </summary>
        private void OnGCMViewActiveHomeView(object sender, IDCMViewEventArgs e)
        {
            DWorkMHub.note(AsyncMessage.RequestHomeView);
        }
        /// <summary>
        /// 刷新数据展示
        /// </summary>
        private void OnGCMViewRefreshData(object sender, IDCMViewEventArgs e)
        {
            LoadGCMDataHandler lgdh = new LoadGCMDataHandler(DataSourceHolder.GCMHolder, gcmView.getItemGridView(), gcmView.getRecordTree(), gcmView.getRecordList());
            DWorkMHub.callAsyncHandle(lgdh);
        }
        /// <summary>
        /// 查看帮助文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="hlpevent"></param>
        private void OnGCMViewRequestHelp(object sender, IDCMViewEventArgs e)
        {
            HelpDocRequester.requestHelpDoc(HelpDocConstants.GCMViewTag);
        }
        private void OnGCMViewSearchButtonClick(object sender, IDCMViewEventArgs e)
        {
            //e.values.length = 1 ### e.values[0] = this.toolStripTextBox_search.Text.Trim()
            OnGCMViewQuickSearch(sender, e);
        }
        #endregion

        /// <summary>
        /// 载入数据显示
        /// 注意：
        /// 1：想了一下，不在在这用串联，加载TreeView需要strain_id。
        /// 如果以后参数内容增多只能在handler内部增加串联
        /// </summary>
        public void loadDataSetView()
        {
            LoadGCMDataHandler lgdh = new LoadGCMDataHandler(DataSourceHolder.GCMHolder, gcmView.getItemGridView(), gcmView.getRecordTree(), gcmView.getRecordList());
            DWorkMHub.callAsyncHandle(lgdh);
        }
        /// <summary>
        /// 激活GCM FrontFindDlg
        /// </summary>
        private void OnGCMViewFrontDataSearch(object sender, IDCMViewEventArgs e)
        {
            if (frontFindDlg == null || frontFindDlg.IsDisposed)
            {
                frontFindDlg = new GCMFrontFindDlg(gcmView.getItemGridView());
                frontFindDlg.setCellHit += new GCMFrontFindDlg.SetHit<DataGridViewCell>(DGVUtil.setDGVCellHit);
                frontFindDlg.cancelCellHit += new GCMFrontFindDlg.CancelHit<DataGridViewCell>(DGVUtil.cancelDGVCellHit);
            }
            frontFindDlg.Show();
            frontFindDlg.Visible = true;
            frontFindDlg.Activate();
        }

        private void OnGCMViewFrontSearchNext(object sender, IDCMViewEventArgs e)
        {
            frontFindDlg.findDown();
        }

        private void OnGCMViewFrontSearchPrev(object sender, IDCMViewEventArgs e)
        {
            frontFindDlg.findRev();
        }


        /// <summary>
        /// 模糊查找
        /// </summary>
        /// <param name="findTerm"></param>
        private void OnGCMViewQuickSearch(object sender, IDCMViewEventArgs e)
        {
            //e.values.length = 1 ### e.values[0] = this.toolStripTextBox_search.Text.Trim()
            DataGridViewCell ncell = datasetBuilder.quickSearch(e.values[0]);
            if (ncell != null)
                setDGVCellHit(ncell);
        }

        /// <summary>
        /// 定位到单元格
        /// </summary>
        /// <param name="cell"></param>
        private void setDGVCellHit(DataGridViewCell cell)
        {
            cell.DataGridView.EndEdit();
            int colCount = DGVUtil.getTextColumnCount(cell.DataGridView);
            DataGridViewCell rightCell = cell.DataGridView.Rows[cell.RowIndex].Cells[colCount - 1];
            cell.DataGridView.CurrentCell = rightCell;
            cell.DataGridView.CurrentCell = cell;
            cell.Selected = true;
            cell.DataGridView.BeginEdit(true);
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="etype"></param>
        /// <param name="fpath"></param>
        /// <param name="exportStrainTree"></param>
        private void OnGCMViewExportData(object sender, IDCMViewEventArgs e)
        {
            //e.values一共有三个成员，存放顺序：ExportTypeDlg.LastOptionValue, ExportTypeDlg.LastFilePath,ExportTypeDlg.ExportStainTree
            ExportType etype = e.values[0];
            string fpath = e.values[1];
            bool exportStrainTree = e.values[2];

            AbsHandler handler = null;
            DataGridViewSelectedRowCollection selectedRows = gcmView.getItemGridView().SelectedRows;
            if (selectedRows != null && selectedRows.Count > 0)
            {
                switch (etype)
                {
                    case ExportType.Excel:
                        handler = new GCMExcelExportHandler(fpath, exportStrainTree, selectedRows, DataSourceHolder.GCMHolder);
                        break;
                    case ExportType.JSONList:
                        handler = new GCMJSONExportHandler(fpath, exportStrainTree, selectedRows, DataSourceHolder.GCMHolder);
                        break;
                    case ExportType.TSV:
                        handler = new GCMTextExportHandler(fpath, exportStrainTree, selectedRows, "\t", DataSourceHolder.GCMHolder);
                        break;
                    case ExportType.CSV:
                        handler = new GCMTextExportHandler(fpath, exportStrainTree, selectedRows, ",", DataSourceHolder.GCMHolder);
                        break;
                    case ExportType.XML:
                        handler = new GCMXMLExportHandler(fpath, exportStrainTree, selectedRows, DataSourceHolder.GCMHolder);
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
                        handler = new GCMExcelExportHandler(fpath, exportStrainTree, datasetBuilder.DgvToTable(gcmView.getItemGridView()), DataSourceHolder.GCMHolder);
                        break;
                    case ExportType.JSONList:
                        handler = new GCMJSONExportHandler(fpath, exportStrainTree, datasetBuilder.DgvToTable(gcmView.getItemGridView()), DataSourceHolder.GCMHolder);
                        break;
                    case ExportType.TSV:
                        handler = new GCMTextExportHandler(fpath, exportStrainTree, datasetBuilder.DgvToTable(gcmView.getItemGridView()), "\t", DataSourceHolder.GCMHolder);
                        break;
                    case ExportType.CSV:
                        handler = new GCMTextExportHandler(fpath, exportStrainTree, datasetBuilder.DgvToTable(gcmView.getItemGridView()), ",", DataSourceHolder.GCMHolder);
                        break;
                    case ExportType.XML:
                        handler = new GCMXMLExportHandler(fpath, exportStrainTree, datasetBuilder.DgvToTable(gcmView.getItemGridView()), DataSourceHolder.GCMHolder);
                        break;
                    default:
                        MessageBox.Show("Unsupport export type!");
                        break;
                }
            }
            if (handler != null)
                DWorkMHub.callAsyncHandle(handler);
        }
        internal void exportData(ExportType etype, string fpath,bool exportStrainTree)
        {
            
        }

        //复制
        private void OnGCMViewCopyClipboard(object sender, IDCMViewEventArgs e)
        {
            DataObject d = gcmView.getItemGridView().GetClipboardContent();
            Clipboard.SetDataObject(d); 
        }

        /// <summary>
        /// 刷新DataGridView数据显示
        /// </summary>
        /// <param name="sids"></param>
        internal void OnUpdateGCMLinkStrains(params string[] sids)
        {
            if (sids == null)
            {
                LoadGCMDataHandler lgdh = new LoadGCMDataHandler(DataSourceHolder.GCMHolder, gcmView.getItemGridView(), gcmView.getRecordTree(), gcmView.getRecordList());
                DWorkMHub.callAsyncHandle(lgdh);
            }
            else
            {
                //有待改进以下实现
                LoadGCMDataHandler lgdh = new LoadGCMDataHandler(DataSourceHolder.GCMHolder, gcmView.getItemGridView(), gcmView.getRecordTree(), gcmView.getRecordList());
                DWorkMHub.callAsyncHandle(lgdh);
            }
        }
    }
}
