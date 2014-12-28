using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base
{
    /// <summary>
    /// 多线程共享锁预定义类
    /// </summary>
    internal class ShareSyncLockers
    {
        /// <summary>
        /// 用于保持串行获取数据库连接的共享锁对象
        /// </summary>
        public static object SQLiteConnPicker_Lock = new object();
        /// <summary>
        /// 用于全局数据序号生成自增长对象锁
        /// </summary>
        public static Object IncrementKey_Lock = new Object();
    }
}
