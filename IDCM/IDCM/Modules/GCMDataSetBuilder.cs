using System;
using System.Collections.Generic;
using System.Configuration;
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
            this.loadedNoter = new Dictionary<string, int>();
        }
        ~GCMDataSetBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            itemDGV = null;
            if (loadedNoter != null)
            {
                loadedNoter.Clear();
        }
            loadedNoter = null;
        }
        #endregion
        #region 实例对象保持部分
        private DataGridView itemDGV=null;
        private Dictionary<string, int> loadedNoter = null;
        public Dictionary<string, int> getLoadedNoter()
        {
            return loadedNoter;
        }
        #endregion


        ///// <summary>
        ///// @Deprecated
        ///// 通过网络请求，载入数据显示
        ///// </summary>
        //public void loadDataSetView()
        //{
        //    int curPage=1;
        //    StrainListPage slp = StrainListQueryExecutor.strainListQuery(curPage);
        //    showDataItems(slp);
        //    while (hasNextPage(slp,curPage))
        //    {
        //        curPage++;
        //        slp = StrainListQueryExecutor.strainListQuery(curPage);
        //        showDataItems(slp);
        //    }
        //}
        ///// <summary>
        ///// 将网络获取数据显示到itemDGV，并予以标记缓存
        ///// </summary>
        ///// <param name="slp"></param>
        //private void showDataItems(StrainListPage slp)
        //{
        //    if (slp == null || slp.list==null)
        //        return;
        //    foreach (Dictionary<string, string> valMap in slp.list)
        //    {
        //        //add valMap note Tag into loadedNoter Map
        //        int dgvrIdx=-1;
        //        loadedNoter.TryGetValue(valMap["id"],out dgvrIdx);
        //        if (dgvrIdx < 0)
        //        {
        //            dgvrIdx=itemDGV.Rows.Add();
        //        }
        //        foreach (KeyValuePair<string, string> entry in valMap)
        //        {
        //            //if itemDGV not contains Column of entry.key
        //            //   add Column named with entry.key
        //            //then merge data into itemDGV View.
        //            //(if this valMap has exist in loadedNoter Map use Update Method else is append Method.) 
        //            if (!itemDGV.Columns.Contains(entry.Key))
        //            {
        //                DataGridViewTextBoxColumn dgvtbc = new DataGridViewTextBoxColumn();
        //                dgvtbc.Name = entry.Key;
        //                dgvtbc.HeaderText = entry.Key;
        //                dgvtbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
        //                itemDGV.Columns.Add(dgvtbc);
        //            }
        //            DataGridViewCell dgvc = itemDGV.Rows[dgvrIdx].Cells[entry.Key];
        //            if(dgvc!=null)
        //            {
        //                dgvc.Value = entry.Value;
        //            }
        //        }
        //    }
        //}
        ///// <summary>
        ///// 判断分页请求是否存在下一页内容
        ///// </summary>
        ///// <param name="slp"></param>
        ///// <param name="reqPage"></param>
        ///// <returns></returns>
        //private bool hasNextPage(StrainListPage slp,int reqPage)
        //{
        //    if(slp!=null && slp.totalpage>slp.pageNumber && slp.totalpage>reqPage)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
