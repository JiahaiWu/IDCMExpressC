using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Core
{
    /// <summary>
    /// 异步消息事件消息细节参数类定义
    /// </summary>
    public class IDCMViewEventArgs : EventArgs
    {
        public readonly dynamic[] values;

        public IDCMViewEventArgs(dynamic[] vals)
        {
            this.values = vals;
        }
    }
}
