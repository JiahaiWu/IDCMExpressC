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
        /// 最长等待毫秒数（默认为5000ms）
        /// </summary>
        public static int MAX_DB_REQUEST_TIME_OUT = 5000;
    }
}
