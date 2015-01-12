﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Data;
using System.Drawing;
using IDCM.AppContext;
using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Core;

namespace IDCM.Modules
{
    class LocalCatBuilder
    {
        #region 构造&析构
        /// <summary>
        /// 初始化个人资源目录树
        /// </summary>
        /// <param name="libTree"></param>
        public LocalCatBuilder(TreeView _baseTree, TreeView _libTree)
        {
            baseTree = _baseTree;
            libTree = _libTree;
            //reset（baseTree & customTree）
            baseTree.Nodes.Clear();
            libTree.Nodes.Clear();
        }
        ~LocalCatBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            baseTree = null;
            libTree = null;
        }
        #endregion
        #region 实例对象保持部分
        private TreeNode rootNode_all = null;

        public TreeNode RootNode_all
        {
            get { return rootNode_all; }
        }
        private TreeNode rootNode_unfiled = null;

        public TreeNode RootNode_unfiled
        {
            get { return rootNode_unfiled; }
        }
        private TreeNode rootNode_trash = null;

        public TreeNode RootNode_trash
        {
            get { return rootNode_trash; }
        }
        private TreeNode selectedNode_Current = null;
        public TreeNode SelectedNode_Current
        {
            get { return selectedNode_Current; }
        }
        private TreeView baseTree;

        public TreeView BaseTree
        {
            get { return baseTree; }
        }
        private TreeView libTree;

        public TreeView LibTree
        {
            get { return libTree; }
        }
        #endregion

        /// <summary>
        /// 根据当前焦点的TreeNode筛选可用的右键菜单显示列表项
        /// </summary>
        /// <param name="cms"></param>
        /// <param name="snode"></param>
        public static void filterContextMenuItems(ContextMenuStrip cms, TreeNode snode)
        {
            foreach (ToolStripItem tsItem in cms.Items)
            {
                if (tsItem is ToolStripSeparator)
                    continue;
                tsItem.Visible = false;
            }
            if (snode == null)
            {
                cms.Items["CreateGroupSet"].Visible = true;
                cms.Items["CreateGroup"].Visible = true;
                cms.Items["CreateSmartGroup"].Visible = true;
            }
            else
            {
                if (snode.Parent == null)
                {
                    cms.Items["CreateGroupSet"].Visible = true;
                    cms.Items["RenameGroupSet"].Visible = true;
                    cms.Items["DeleteGroupSet"].Visible = true;
                    cms.Items["CreateGroup"].Visible = true;
                }
                else
                {
                    cms.Items["CreateGroup"].Visible = true;
                    cms.Items["RenameGroup"].Visible = true;
                    cms.Items["DeleteGroup"].Visible = true;
                    cms.Items["CreateSmartGroup"].Visible = true;
                }
            }
        }
        /// <summary>
        /// 根据指定焦点的TreeNode获取归档数据集
        /// </summary>
        public void loadTreeSet()
        {
            // 添加默认的三个根节点（All Strains，Unfiled，Trash）其Name应为负数值的字符串表示
            rootNode_all = new TreeNode("All Strains", 0, 0);
            rootNode_all.Name = CatalogNode.REC_ALL.ToString();
            rootNode_unfiled = new TreeNode("Unfiled", 0, 0);
            rootNode_unfiled.Name = CatalogNode.REC_UNFILED.ToString();
            rootNode_trash = new TreeNode("Trash", 0, 0);
            rootNode_trash.Name = CatalogNode.REC_TRASH.ToString();
            baseTree.Nodes.Add(rootNode_all);
            baseTree.Nodes.Add(rootNode_unfiled);
            baseTree.Nodes.Add(rootNode_trash);
            /////////////////////////////////////////////////////////////////
            List<CatalogNode> pnodes = LocalRecordMHub.findParentNodes(DataSourceHolder.DataSource);//SELECT * FROM LibraryNode

            //如果为空，则创建一个分组为空的节点，更新到数据库
            if (pnodes == null || pnodes.Count == 0)
            {
                CatalogNode node = new CatalogNode("My Group Set (Temp)", "GroupSet", "My Group Set (Temp)", -1);
                LocalRecordMHub.saveLibraryNode(DataSourceHolder.DataSource,node);
                long newlid = node.Lid;
                pnodes = LocalRecordMHub.findParentNodes(DataSourceHolder.DataSource);
            }

            //如果节点不为空，遍历节点，创建TreeNode
            foreach (CatalogNode dr in pnodes)
            {
                TreeNode gsNode = new TreeNode(dr.Name, 1, 1);
                gsNode.Name = dr.Lid.ToString();//把节点名称设置为节点ID
                libTree.Nodes.Add(gsNode);
            }


            foreach(TreeNode lnode in libTree.Nodes)
            {
                long pid=Convert.ToInt64(lnode.Name);//获取节点名称(ID)

                //根据ID 查LibraryNode 根据分组 order by lorder
                List<CatalogNode> subnodes = LocalRecordMHub.findSubNodes(DataSourceHolder.DataSource,pid);

                //如果节点下有子节点，把子几点添加到父节点上
                if (subnodes != null)
                {
                    foreach (CatalogNode node in subnodes)
                    {
                        TreeNode gsubNode = new TreeNode(node.Name, 2, 2);
                        gsubNode.Name = node.Lid.ToString();
                        lnode.Nodes.Add(gsubNode);
                    }
                }
            }
        }
        /// <summary>
        /// 删除节点操作
        /// </summary>
        /// <param name="treeNode"></param>
        internal void deleteNode(TreeNode treeNode)
        {
            long referNameId = Convert.ToInt64(treeNode.Name);
            int res = LocalRecordMHub.delNodeCascaded(DataSourceHolder.DataSource, referNameId);
            treeNode.Remove();
        }
        /// <summary>
        /// addGroupSet
        /// </summary>
        /// <param name="treeView_library"></param>
        internal void addGroupSet(TreeNode treeNode)
        {
            CatalogNode lnode = new CatalogNode("New Group Set", "GroupSet");
            TreeNode gsNode = new TreeNode(lnode.Name, 1, 1);
            TreeNode tpnode = treeNode != null ? treeNode : selectedNode_Current;
#if DEBUG
            System.Diagnostics.Debug.Assert(tpnode != null);
#endif
            int insertIndex = tpnode == null ? 0 : tpnode.Index + 1;
            if (tpnode != null && tpnode.Level > 0)
            {
                insertIndex = tpnode.Parent.Index + 1;
            }
            libTree.Nodes.Insert(insertIndex, gsNode);
            gsNode.EnsureVisible();

            lnode.Pid=-1;
            lnode.Lorder=insertIndex;
            LocalRecordMHub.insertLibraryNode(DataSourceHolder.DataSource, lnode);
            gsNode.Name = lnode.Lid.ToString();
            ///////////////////////////
            libTree.LabelEdit = true;
            gsNode.BeginEdit();
        }
        /// <summary>
        /// addGroup
        /// </summary>
        /// <param name="treeView_library"></param>
        internal void addGroup(TreeNode treeNode)
        {
            TreeNode tpnode = treeNode != null ? treeNode : selectedNode_Current;
#if DEBUG
            System.Diagnostics.Debug.Assert(tpnode != null);
#endif
            int insetIndex = 0;
            if (tpnode != null && tpnode.Level > 0)
            {
                tpnode = tpnode.Parent;
                insetIndex = tpnode.Index + 1;
            }
            long referNameId = Convert.ToInt64(tpnode.Name);
            CatalogNode pnode = LocalRecordMHub.findLibraryNode(DataSourceHolder.DataSource, referNameId);
            if (pnode != null)
            {
                CatalogNode lnode = new CatalogNode("New Group", "Group");
                lnode.Pid = pnode.Lid;
                TreeNode gsNode = new TreeNode(lnode.Name, 2, 2);
                if(insetIndex>=0)
                    tpnode.Nodes.Insert(insetIndex, gsNode);
                else
                    tpnode.Nodes.Add(gsNode);
                gsNode.EnsureVisible();
                lnode.Lorder = insetIndex;
                LocalRecordMHub.insertLibraryNode(DataSourceHolder.DataSource, lnode);
                gsNode.Name = lnode.Lid.ToString();
                libTree.LabelEdit = true;
                gsNode.BeginEdit();
            }
        }
        /// <summary>
        /// 节点重命名提交操作
        /// </summary>
        /// <param name="treeNode"></param>
        internal void renameNode(TreeNode treeNode,string label)
        {
            if (label == null || label.Length < 1)
                treeNode.EndEdit(true);
            else
            {
                LocalRecordMHub.updateCatalogNode(DataSourceHolder.DataSource, treeNode.Name, "Name", label);
            }
        }
        
        /// <summary>
        /// 选中节点标记处理方法，
        /// 对baseTree和libTree共享一个目标节点，标记选中节点是需要清除既有的节点状态标记。
        /// 如果目标节点和历史焦点节点不一致，即返回状态为真，表示需要更新数据记录状态。
        /// </summary>
        /// <param name="snode"></param>
        /// <param name="baseTree"></param>
        /// <param name="libTree"></param>
        public bool noteCurSelectedNode(TreeNode snode)
        {
            if (snode == null)
                return false;
            TreeNode lastNode = selectedNode_Current;
            selectedNode_Current = snode;
            bool needUpdateData = snode != lastNode;
            TreeView lastTree = null;
            if (lastNode != null)
            {
                lastTree = lastNode.TreeView;
            }
            if (lastTree != null && !snode.TreeView.Equals(lastTree))
            {
                lastTree.SelectedNode = null;
                lastTree.HideSelection = false;
                lastTree.Refresh();
            }
            snode.TreeView.SelectedNode = snode;
            snode.TreeView.Refresh();
            return needUpdateData;
        }
    }
}