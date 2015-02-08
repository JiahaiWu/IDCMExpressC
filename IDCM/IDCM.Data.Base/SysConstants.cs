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

        #region AppSettings
        public const string LastWorkSpace = "LWS";
        public const string LWSAsDefault = "LWS_As_Default";
        public const string LUID = "LUID";
        public const string LPWD = "LPWD";
        /// <summary>
        /// Default Table Setting File
        /// </summary>
        public const string CTableDef="CTableDef";
        /// <summary>
        /// Default System Setting File Templates
        /// </summary>
        public const string CTableTemplate="CTableTemplate";
        /// <summary>
        /// 帮助文档资源定位
        /// </summary>
        public const string HelpBase = "HelpBase";
        /// <summary>
        /// GCM用户登录请求资源地址
        /// </summary>
        public const string SignInUri = "SignInUri";
        /// <summary>
        /// GCM用户签出请求资源地址
        /// </summary>
        public const string SignOffUri = "SignOffUri";
        /// <summary>
        /// GCM菌种列表信息查询请求资源地址
        /// </summary>
        public const string StrainListUri = "StrainListUri";
        /// <summary>
        /// GCM菌种保藏记录详细信息请求资源地址
        /// </summary>
        public const string StrainViewUri = "StrainViewUri";

        #endregion
        /// <summary>
        /// GCM上传XML格式模板文档
        /// </summary>
        public const string GCMUploadTemplate = "GCMUploadTemplate";
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
