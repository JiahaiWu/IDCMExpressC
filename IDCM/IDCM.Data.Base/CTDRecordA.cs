using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base
{
    public class CTDRecordA
    {
        /// <summary>
        /// 动态记录表名预定义
        /// </summary>
        public const string table_name = "CTDRecord";
        /// <summary>
        /// Record ID 固定列名设定
        /// </summary>
        public const string CTD_RID = "ctd_rid";
        /// <summary>
        /// Record 从属父级目录Library ID的列名设定，没有从属特定节点时CatalogNodeDA.REC_UNFILED
        /// </summary>
        public const string CTD_PLID = "ctd_plid";
        /// <summary>
        /// Record 从属目录的Library ID的列名设定，没有从属特定节点时为CatalogNodeDA.REC_UNFILED
        /// </summary>
        public const string CTD_LID = "ctd_lid";
    }
}
