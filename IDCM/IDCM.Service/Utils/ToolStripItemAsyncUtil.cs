using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.Utils
{
    class ToolStripItemAsyncUtil
    {
        /// <summary>
        /// 异步调用具有特定名称的UI控件设置文本
        /// </summary>
        /// <param name="control"></param>
        /// <param name="data"></param>
        public static void SyncSetText(ToolStripItem tsitem, string data)
        {
            ControlAsyncUtil.SyncInvoke(tsitem.Owner, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                tsitem.Text = data;
            }));
        }
    }
}
