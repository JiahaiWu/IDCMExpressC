using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.UIM
{
    /// <summary>
    /// 发送网络请求，获取strain信息，构建GCM
    /// </summary>
    public class GCMItemsLoader
    {
        /// <summary>
        /// 分多次发送网络请求，获取strain信息，将strain数据添加到DataGridView中
        /// 说明：
        /// 1：分为多次网络请求，每次请求获取1页数据，获取到最后一页时请求结束
        /// 2：每次获取数据后调用showDataItems将strain数据添加到DataGridView
        /// </summary>
        /// <param name="gcmSite"></param>
        /// <param name="itemDGV"></param>
        /// <param name="loadedNoter"></param>
        /// <param name="recordTree"></param>
        /// <param name="recordList"></param>
        /// <returns></returns>
        public static bool loadData(GCMSiteMHub gcmSite, DataGridView itemDGV, Dictionary<string, int> loadedNoter, TreeView recordTree, ListView recordList)
        {
            GCMDataMHub gcmDataHub = new GCMDataMHub();
            int curPage = 1;
            StrainListPage slp = gcmDataHub.strainListQuery(gcmSite, curPage);
            showDataItems(slp, itemDGV, loadedNoter);
            while (hasNextPage(slp, curPage))
            {
                curPage++;
                slp = gcmDataHub.strainListQuery(gcmSite, curPage);
                showDataItems(slp, itemDGV, loadedNoter);
            }
            if (loadedNoter.Count > 0)
            {
                TreeView treeNode = GCMStrainTreeLoader.loadData(gcmSite, loadedNoter.First().Key, recordList);
                if (treeNode == null) return true;
                TreeViewAsyncUtil.syncClearNodes(recordTree);
                TreeViewAsyncUtil.syncAddNodes(recordTree,treeNode);
            }
            return true;
        }

        private static void showDataItems(StrainListPage slp, DataGridView itemDGV, Dictionary<string, int> loadedNoter)
        {
            if (slp == null || slp.list == null)
                return;
            if (!itemDGV.Columns.Contains("id"))
            {
                DataGridViewTextBoxColumn dgvtbc = new DataGridViewTextBoxColumn();
                dgvtbc.Name = "id";
                dgvtbc.HeaderText = "id";
                dgvtbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                DGVAsyncUtil.syncAddCol(itemDGV, dgvtbc);
            }
            foreach (Dictionary<string, string> valMap in slp.list)
            {
                //add valMap note Tag into loadedNoter Map
                int dgvrIdx = -1;
                if (!loadedNoter.TryGetValue(valMap["id"], out dgvrIdx))
                {
                    dgvrIdx = itemDGV.RowCount;
                    DGVAsyncUtil.syncAddRow(itemDGV, null, dgvrIdx);
                    loadedNoter.Add(valMap["id"], dgvrIdx);
                }
                foreach (KeyValuePair<string, string> entry in valMap)
                {
                    //if itemDGV not contains Column of entry.key
                    //   add Column named with entry.key
                    //then merge data into itemDGV View.
                    //(if this valMap has exist in loadedNoter Map use Update Method else is append Method.) 
                    if (!itemDGV.Columns.Contains(entry.Key))
                    {
                        DataGridViewTextBoxColumn dgvtbc = new DataGridViewTextBoxColumn();
                        dgvtbc.Name = entry.Key;
                        dgvtbc.HeaderText = entry.Key;
                        dgvtbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        DGVAsyncUtil.syncAddCol(itemDGV, dgvtbc); 
                    }
                    DataGridViewCell dgvc = itemDGV.Rows[dgvrIdx].Cells[entry.Key];
                    if (dgvc != null)
                    {
                        DGVAsyncUtil.syncValue(itemDGV, dgvc, entry.Value);
                    }
                }
            }
        }
        /// <summary>
        /// 判断分页请求是否存在下一页内容
        /// </summary>
        /// <param name="slp"></param>
        /// <param name="reqPage"></param>
        /// <returns></returns>
        private static bool hasNextPage(StrainListPage slp, int reqPage)
        {
            if (slp != null && slp.totalpage > slp.pageNumber && slp.totalpage > reqPage)
            {
                return true;
            }
            return false;
        }
    }
}
