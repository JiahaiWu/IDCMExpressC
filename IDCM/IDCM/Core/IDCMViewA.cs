using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IDCM.Core
{
    //异步消息事件委托形式化声明
    [Serializable]
    [ComVisible(true)]
    public delegate void IDCMViewEventHandler(object sender, IDCMViewEventArgs e);
}
