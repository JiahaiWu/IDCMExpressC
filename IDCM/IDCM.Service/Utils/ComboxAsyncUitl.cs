using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace IDCM.Service.Utils
{
    public class ComboxAsyncUitl
    {
        /// <summary>
        /// 异步调用具有特定名称的UI控件设置文本
        /// </summary>
        /// <param name="control"></param>
        /// <param name="data"></param>
        public static void SyncSetText(ComboBox control, string data)
        {
            ControlAsyncUtil.SyncInvokeNoWait(control, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                control.FormatString = data;
            }));
        }
    }
}
