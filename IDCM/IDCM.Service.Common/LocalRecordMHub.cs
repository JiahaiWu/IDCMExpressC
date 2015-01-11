using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Service.Common.DAM;
using System.Data;
using IDCM.Service.Common.Core;

namespace IDCM.Service.Common
{
    public class LocalRecordMHub
    {
        public static Dictionary<string, int> getCustomViewDBMapping(DataSourceMHub datasource)
        {
            return CustomTColMapDAM.ColumnMappingHolder.getCustomViewDBMapping(datasource.WSM);
        }
        public static DataTable queryCTDRecordByHistSQL(DataSourceMHub datasource, string cmdstr, long lcount = 0, long offset = 0)
        {
            return CTDRecordDAM.queryCTDRecordByHistSQL(datasource.WSM, cmdstr, lcount, offset);
        }
        /// <summary>
        /// 最近的用户发起的数据表单查询条件及聚合结果数量的记录
        /// </summary>
        /// <param name="cmdstr"></param>
        /// <returns></returns>
        public static KeyValuePair<string, int> getLastDGVRQuery()
        {
            return QueryCmdCache.getLastDGVRQuery();
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        /// <returns></returns>
        public static DataTable queryCTDRecord(DataSourceMHub datasource, string nodeIds = null, string rids = null)
        {
            return CTDRecordDAM.queryCTDRecord(datasource.WSM, nodeIds, rids);
        }
    }
}
