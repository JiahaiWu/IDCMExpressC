using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Service.Common.DAM;
using System.Data;

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
    }
}
