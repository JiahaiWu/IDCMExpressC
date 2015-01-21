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

namespace IDCM.ViewManager
{
    public class GCMViewManager : ManagerA
    {
        #region 构造&析构
        public GCMViewManager()
        {
            gcmView = new GCMView();
            gcmView.Shown += OnGcmView_Shown;
            gcmView.getItemGridView().CellClick += OnGcmDataGridViewItems_CellClick;
            gcmView.getRecordTree().NodeMouseClick += OnGcmTreeViewRecord_NodeMouseClick;
            //frontFindDlg = new GCMFrontFindDlg(gcmView.getItemGridView());
            //frontFindDlg.setCellHit += new GCMFrontFindDlg.SetHit<DataGridViewCell>(DGVUtil.setDGVCellHit);
            //frontFindDlg.cancelCellHit += new GCMFrontFindDlg.CancelHit<DataGridViewCell>(DGVUtil.cancelDGVCellHit);
            //recBuilder = new GCMRecordBuilder(gcmView.getRecordTree(),gcmView.getRecordList());
            datasetBuilder = new GCMDataSetBuilder(gcmView.getItemGridView());
            //searchBuilder = new GCMSearchBuilder(gcmView.getSearchPanel(), gcmView.getSearchSpliter());
            BackProgressIndicator.addIndicatorBar(gcmView.getProgressBar());//有待完善
        }
        public static HomeViewManager getInstance()
        {
            ManagerI gcm = ViewManagerHolder.getManager(typeof(GCMViewManager));
            return gcm == null ? null : (gcm as HomeViewManager);
        }
        ~GCMViewManager()
        {
            dispose();
        }
        #endregion
        #region 实例对象保持部分

        
        //页面窗口实例
        private volatile GCMView gcmView = null;
        //private volatile GCMRecordBuilder recBuilder = null;
        private volatile GCMDataSetBuilder datasetBuilder = null;
        //private volatile GCMSearchBuilder searchBuilder = null;
        //private GCMFrontFindDlg frontFindDlg = null;
        private bool processing = false;
        
        #endregion
        #region 接口实例化部分
        public override void dispose()
        {
            _isDisposed = true;
            //if (recBuilder != null)
            //{
            //    recBuilder.Dispose();
            //    recBuilder = null;
            //}
            if (datasetBuilder != null)
            {
                datasetBuilder.Dispose();
                datasetBuilder = null;
            }
            //if (searchBuilder != null)
            //{
            //    searchBuilder.Dispose();
            //    searchBuilder = null;
            //}
            if (gcmView != null && !gcmView.IsDisposed)
            {
                BackProgressIndicator.removeIndicatorBar(gcmView.getProgressBar());
                gcmView.Close();
                gcmView.Dispose();
                gcmView = null;
            }
            //if (frontFindDlg != null && !frontFindDlg.IsDisposed)
            //{
            //    frontFindDlg.Close();
            //    frontFindDlg.Dispose();
            //    frontFindDlg = null;
            //}
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
                gcmView.Shown += OnGcmView_Shown;
                gcmView.getItemGridView().CellClick += OnGcmDataGridViewItems_CellClick;
                gcmView.getRecordTree().NodeMouseClick += OnGcmTreeViewRecord_NodeMouseClick;
                //recBuilder = new GCMRecordBuilder(gcmView.getRecordTree(), gcmView.getRecordList());
                datasetBuilder = new GCMDataSetBuilder(gcmView.getItemGridView());
                BackProgressIndicator.addIndicatorBar(gcmView.getProgressBar());
                //searchBuilder = new GCMSearchBuilder(gcmView.getSearchPanel(), gcmView.getSearchSpliter());
            }
            AuthInfo auth = DataSourceHolder.GCMHolder.getSignedAuthInfo();
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
        /// <summary>
        /// 载入数据显示
        /// </summary>
        public void loadDataSetView()
        {
            LoadGCMDataHandler lgdh = new LoadGCMDataHandler(DataSourceHolder.GCMHolder, gcmView.getItemGridView(), gcmView.getRecordTree(), gcmView.getRecordList(), datasetBuilder.getLoadedNoter());
            DWorkMHub.callAsyncHandle(lgdh);
            //datasetBuilder.loadDataSetView();
        }
        /// <summary>
        /// 更新具体记录的显示
        /// </summary>
        public void updateRecordView()
        {

        }

        /// <summary>
        /// activeHomeView
        /// </summary>
        /// <param name="refresh"></param>
        public void OnGcmView_Shown(object sender, EventArgs e)
        {
           //加载默认的数据报表展示
           loadDataSetView();
           updateRecordView();
           //resize for data view
           gcmView.getItemGridView().AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
           gcmView.getItemGridView().AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None);
           gcmView.getItemGridView().AllowUserToResizeColumns = true;
        }

        public void OnGcmDataGridViewItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                DataGridViewRow dgvRow = gcmView.getItemGridView().CurrentRow;
                String strainid = dgvRow.Cells["id"].FormattedValue.ToString();
                LoadGCMStrainViewHandler lsvh = new LoadGCMStrainViewHandler(DataSourceHolder.GCMHolder,strainid, gcmView.getRecordTree(), gcmView.getRecordList());
                DWorkMHub.callAsyncHandle(lsvh);
        }

        public void OnGcmTreeViewRecord_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            LoadGCMRecordNodeDetail.loadData(e.Node, gcmView.getRecordList());     
        }
    }
}
