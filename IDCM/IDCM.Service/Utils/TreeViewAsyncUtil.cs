using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.Utils
{
    class TreeViewAsyncUtil
    {

        public static void syncAddNode(TreeView tree, TreeNode node)
        {
            ControlAsyncUtil.SyncInvoke(tree, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (node != null)
                    tree.Nodes.Add(node);
            }));
        }
        public static void syncClearNodes(TreeView tree)
        {
            ControlAsyncUtil.SyncInvoke(tree, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (tree != null)
                    tree.Nodes.Clear();
            }));
        }
        public static void syncAddNodes(TreeView to, TreeView from)
        {
            ControlAsyncUtil.SyncInvoke(to, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (to != null)
                {
                    foreach (TreeNode node in from.Nodes)
                    {
                        TreeNode newNode = node.Clone() as TreeNode;
                        to.Nodes.Add(newNode);
                    }
                }
            }));
        }
    }
}
