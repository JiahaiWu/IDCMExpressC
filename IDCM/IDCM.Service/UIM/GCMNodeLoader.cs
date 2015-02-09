using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.DataTransfer;
using IDCM.Service.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IDCM.Service.UIM
{
    class GCMNodeLoader
    {
        public GCMNodeLoader(GCMSiteMHub gcmSiteHolder, string strainid, ListView recordView)
        {
            this.gcmSiteHolder = gcmSiteHolder;
            this.strainid = strainid;
            this.recordView = recordView;
        }

        public GCMNodeLoader(GCMSiteMHub gcmSiteHolder)
        {
            this.gcmSiteHolder = gcmSiteHolder;
        }

        public GCMNodeLoader()
        { }
        /// <summary>
        /// 根据strain_id显示strain详细信息(以TreeView的方式)
        /// 注意：
        /// 1：如果strain没有详细信息，将返回NULL
        /// </summary>
        /// <param name="gcmSite"></param>
        /// <param name="strainid"></param>
        /// <param name="recordView"></param>
        /// <returns></returns>
        internal TreeView loadData()
        {
            StrainView sv = GCMDataMHub.strainViewQuery(gcmSiteHolder, strainid);
            TreeView treeView = buildTreeView(sv);
            GCMNodeDetailLoader.loadData(treeView.Nodes[0], recordView);
            return treeView;
        }

        
        /// <summary>
        /// 构建TreeNode，将StrainView信息添加到TreeNode.Tag中
        /// </summary>
        /// <param name="sv"></param>
        /// <returns></returns>
        private TreeView buildTreeView(StrainView sv)
        {
            TreeView treeView = new TreeView();
            if (sv == null)
                return null;
            foreach (KeyValuePair<string, object> svEntry in sv.ToDictionary())
            {
                TreeNode node = new TreeNode(svEntry.Key);
                node.Name = svEntry.Key;
                if (svEntry.Value is string)
                {
                    node.Tag = svEntry.Value;
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    ///////////////////////////////////////////////////////////////
                    //时间2014-10-23
                    //说明：
                    //1:如果有子节点，将子节点存入tag中，构建ListView时从tag中取子节点
                    node.Tag = svEntry.Value;
                    /////////////////////////////////////////////////////////////
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        TreeNode subNode = new TreeNode(subEntry.Key);
                        subNode.Name = subEntry.Key;
                        subNode.Tag = Convert.ToString(subEntry.Value);
                        node.Nodes.Add(subNode);
                    }
                }
                //TreeViewAsyncUtil.syncAddNode(recordTree, node);
                treeView.Nodes.Add(node);
            }    
            return treeView;
        }
        private GCMSiteMHub gcmSiteHolder;
        private string strainid;
        private ListView recordView;
    }
}
