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
    }
}
