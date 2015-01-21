using IDCM.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.UIM
{
    public class LoadGCMRecordNodeDetailed
    {
        /// <summary>
        /// 根据传入的TreeNode，读取Tag内容，用Tag内容构建一个ListView
        /// 说明：
        /// 1：ListView分为两列(Name，Data)显示
        /// 注意：
        /// 1：TreeNode.Tag大概分为三种情况(string,Dictionary<string, dynamic>,null)
        ///     如果不是这三种存储方式，ListView将无法构建(不会报错，但是Tag中的数据显示不出来)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="listView"></param>
        public static void loadData(TreeNode node, ListView listView)
        {
            ListViewAsyncUtil.syncClearListView(listView);
            if (listView == null || node == null)
                return;
            ColumnHeader cZh = new ColumnHeader();
            cZh.Text = "Name";
            cZh.Width = 205;
            ColumnHeader cZh1 = new ColumnHeader();
            cZh1.Text = "Data";
            cZh1.Width = 205;
            ListViewAsyncUtil.syncAddColumn(listView, cZh);
            ListViewAsyncUtil.syncAddColumn(listView, cZh1);
            if (node.Tag is string)
            {
                string tag = node.Tag as string;
                ListViewItem lv = new ListViewItem(node.Name);
                lv.SubItems.AddRange(new string[] { tag });
                ListViewAsyncUtil.syncAddItem(listView, lv);
            }
            else if (node.Tag is Dictionary<string, dynamic>)
            {
                Dictionary<string, dynamic> dictionary = node.Tag as Dictionary<string, dynamic>;
                foreach (KeyValuePair<string, dynamic> subEntry in dictionary)
                {
                    ListViewItem lv = new ListViewItem(subEntry.Key);
                    String value = Convert.ToString(subEntry.Value);
                    if (value == null) value = "";
                    lv.SubItems.AddRange(new string[] { value });
                    ListViewAsyncUtil.syncAddItem(listView, lv);
                }
            }
            else if (node.Tag == null)
            {
                ListViewItem lv = new ListViewItem(node.Name);
                ListViewAsyncUtil.syncAddItem(listView, lv);
            }
        }
    }
}
