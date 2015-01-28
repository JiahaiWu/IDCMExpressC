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

namespace IDCM.ViewManager
{
    public class GCMViewManager : ManagerA
    {
        #region 构造&析构
        public GCMViewManager()
        {
            gcmView = new GCMView();
            gcmView.setManager(this);

            gcmView.Shown += OnGcmView_Shown;
            gcmView.getItemGridView().CellClick += OnGcmDataGridViewItems_CellClick;
            gcmView.getRecordTree().NodeMouseClick += OnGcmTreeViewRecord_NodeMouseClick;

            frontFindDlg = new GCMFrontFindDlg(gcmView.getItemGridView());
            frontFindDlg.setCellHit += new GCMFrontFindDlg.SetHit<DataGridViewCell>(DGVUtil.setDGVCellHit);
            frontFindDlg.cancelCellHit += new GCMFrontFindDlg.CancelHit<DataGridViewCell>(DGVUtil.cancelDGVCellHit);
           
            datasetBuilder = new GCMDataSetBuilder(gcmView.getItemGridView());
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

                datasetBuilder = new GCMDataSetBuilder(gcmView.getItemGridView());
                BackProgressIndicator.addIndicatorBar(gcmView.getProgressBar());//有待完善
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
        public void OnGcmView_Shown(object sender, EventArgs e)
        {
           //加载默认的数据报表展示
           loadDataSetView();
           //resize for data view
           gcmView.getItemGridView().AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
           gcmView.getItemGridView().AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None);
           gcmView.getItemGridView().AllowUserToResizeColumns = true;
        }

        /// <summary>
        /// 查看帮助文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="hlpevent"></param>
        public void requestHelp()
        {
            HelpDocRequester.requestHelpDoc(HelpDocConstants.GCMViewTag);
        }

        /// <summary>
        /// 查看Strain明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnGcmDataGridViewItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                DataGridViewRow dgvRow = gcmView.getItemGridView().CurrentRow;
                String strainid = dgvRow.Cells["id"].FormattedValue.ToString();
                LoadGCMStrainViewHandler lsvh = new LoadGCMStrainViewHandler(DataSourceHolder.GCMHolder,strainid, gcmView.getRecordTree(), gcmView.getRecordList());
                DWorkMHub.callAsyncHandle(lsvh);
        }

        /// <summary>
        /// 查看GCM_TreeView节点明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnGcmTreeViewRecord_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            GCMNodeDetailLoader.loadData(e.Node, gcmView.getRecordList());     
        }

        /// <summary>
        /// 激活HomeView视图
        /// </summary>
        public void activeGCMView()
        {
            DWorkMHub.note(AsyncMessage.RetryQuickStartConnect);
        }
        /// <summary>
        /// 刷新数据展示
        /// </summary>
        public void gcmDataRefresh()
        {
            LoadGCMDataHandler lgdh = new LoadGCMDataHandler(DataSourceHolder.GCMHolder, gcmView.getItemGridView(), gcmView.getRecordTree(), gcmView.getRecordList(), datasetBuilder.getLoadedNoter());
            DWorkMHub.callAsyncHandle(lgdh);
        }
        #endregion

        /// <summary>
        /// 载入数据显示
        /// </summary>
        public void loadDataSetView()
        {
            LoadGCMDataHandler lgdh = new LoadGCMDataHandler(DataSourceHolder.GCMHolder, gcmView.getItemGridView(), gcmView.getRecordTree(), gcmView.getRecordList(), datasetBuilder.getLoadedNoter());
            DWorkMHub.callAsyncHandle(lgdh);
        }

        /// <summary>
        /// 激活GCM FrontFindDlg
        /// </summary>
        internal void frontDataSearch()
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

        internal void frontSearchNext()
        {
            frontFindDlg.findDown();
        }

        internal void frontSearchPrev()
        {
            frontFindDlg.findRev();
        }

        internal void quickSearch(string findTerm)
        {
            DataGridViewCell ncell = datasetBuilder.quickSearch(findTerm);
            if (ncell != null)
                setDGVCellHit(ncell);
        }

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

        internal void exportData(ExportType etype, string fpath)
        {
            KeyValuePair<string, int> lastQuery = LocalRecordMHub.getLastDGVRQuery();
            AbsHandler handler = null;
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
                default:
                    MessageBox.Show("Unsupport export type!");
                    break;
            }
        }

        internal void CopyClipboard()
        {
            DataObject d = gcmView.getItemGridView().GetClipboardContent();
            Clipboard.SetDataObject(d);
        }

        internal void PasteClipboard()
        {
            datasetBuilder.PasteClipboard();
        }
    }
}
