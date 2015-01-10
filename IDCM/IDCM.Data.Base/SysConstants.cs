using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base
{
    public class SysConstants
    {
        public const string SOCK_SATUTS = "status";
        public const string SOCK_LUID = "luid";
        public const string SOCK_LICENSE = "license";
        public const string APP_Assembly = "IDCM";
        public const string DB_SUFFIX = ".mrc";
        public const int Max_Attr_Count = 1000;
        public const int EXPORT_PAGING_COUNT = 1000;
        /// <summary>
        /// 最长等待毫秒数
        /// </summary>
        public static int MAX_DB_REQUEST_TIME_OUT = 10000;
        /// <summary>
        /// 最大数据库连接池连接数
        /// </summary>
        public static int MAX_DB_REQUEST_POOL_NUM = 4;
    }
}
