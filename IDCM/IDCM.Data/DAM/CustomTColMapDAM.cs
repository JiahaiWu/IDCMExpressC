using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.POO;
using IDCM.Data.Common;
using Dapper;
using IDCM.Data.DHCP;

namespace IDCM.Data.DAM
{
    class CustomTColMapDAM
    {
        /// <summary>
        /// 创建自定义表实例
        /// </summary>
        public static void buildCustomTable(SQLiteConnPicker picker)
        {
            List<CustomTColDef> ctcds = CustomTColDefDAM.loadAll(picker);
            StringBuilder cmdBuilder = new StringBuilder();
            cmdBuilder.Append("Create Table if Not Exists " + CTDRecordDAM.table_name + " (" + CTDRecordDAM.CTD_RID + " Integer unique primary key ");
            int index = 1;
            Dictionary<string, string> noteCmds = new Dictionary<string, string>();
            foreach (CustomTColDef ctcd in ctcds)
            {
                string cmdstr = null;
                noteCmds.TryGetValue(ctcd.Attr, out cmdstr);
                if (cmdstr != null)
                {
                    log.Warn("Duplicate column name: " + ctcd.Attr);
                    continue;
                }
                if (ctcd.Attr.Equals(CTDRecordDAM.CTD_RID))
                {
                    noteCmds.Add(ctcd.Attr, "insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + ctcd.Attr + "',0,-1)");
                    continue;
                }
                else if (ctcd.Attr.Equals(CTDRecordDAM.CTD_PLID) || ctcd.Attr.Equals(CTDRecordDAM.CTD_LID))
                {
                    noteCmds.Add(ctcd.Attr, "insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + ctcd.Attr + "'," + index + ",-1)");
                }
                else
                {
                    int viewOrder = 2 + (ctcd.IsRequire ? index : MaxMainViewCount + index);
                    noteCmds.Add(ctcd.Attr, "insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + ctcd.Attr + "'," + index + "," + viewOrder + ")");
                }
                //////////////////////////////////////////////
                cmdBuilder.Append(",");
                string sqliteType = AttrTypeConverter.getSQLiteType(ctcd.AttrType);
                cmdBuilder.Append(ctcd.Attr).Append(" ").Append(sqliteType);
                if (ctcd.IsUnique)
                {
                    cmdBuilder.Append(" UNIQUE ");
                }
                if (ctcd.DefaultVal != null && ctcd.DefaultVal.Length > 0)
                {
                    if (sqliteType.Equals("Text", StringComparison.CurrentCultureIgnoreCase))
                    {
                        cmdBuilder.Append(" Default '").Append(ctcd.DefaultVal).Append("'");
                    }
                    else
                    {
                        cmdBuilder.Append(" Default ").Append(ctcd.DefaultVal);
                    }
                }
                ++index;
            }
            cmdBuilder.Append(")");
            DAMBase.executeSQL(picker, cmdBuilder.ToString());
            DAMBase.executeSQL(picker, noteCmds.Values.ToArray()); //noteDefaultColMap
        }

        
        /// <summary>
        /// find All CustomTColMap By viewOrder
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        public static List<CustomTColMap> findAllByOrder(SQLiteConnPicker picker)
        {
            string cmd = "select * from " + typeof(CustomTColMap).Name + " order by viewOrder";
            using (picker)
            {
                return picker.getConnection().Query<CustomTColMap>(cmd).ToList<CustomTColMap>();
            }
        }

        public static int updateViewOrder(SQLiteConnPicker picker,string attr, int viewOrder)
        {
            string cmd = "update " + typeof(CustomTColMap).Name + " set viewOrder=" + viewOrder + " where attr='" + attr + "'";
            using (picker)
            {
                return picker.getConnection().Execute(cmd);
            }
        }
      
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 主表域最大显示字段数
        /// </summary>
        public const int MaxMainViewCount = 1000;
    }
}
