using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Core
{   
    /// <summary>
    /// 多线程共享锁预定义类
    /// </summary>
    class ShareSyncLockers
    {
        /// <summary>
        /// 本地表单数据视图控件的独占保持的共享锁对象
        /// </summary>
        public static object LocalDataGridView_Lock = new object();
    }
}
