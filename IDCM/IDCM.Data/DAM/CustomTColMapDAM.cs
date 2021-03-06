﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using IDCM.Data.Common;
using Dapper;
using System.Data.SQLite;
using IDCM.Data.DHCP;
using IDCM.Data.Base.Utils;

namespace IDCM.Data.DAM
{
    class CustomTColMapDAM
    {
        /// <summary>
        /// 创建自定义表实例
        /// </summary>
        public static void buildCustomTable(ConnLabel sconn)
        {
            List<CustomTColDef> ctcds = CustomTColDefDAM.loadAll(sconn);
            StringBuilder cmdBuilder = new StringBuilder();
            cmdBuilder.Append("Create Table if Not Exists " + CTDRecordA.table_name + " (" + CTDRecordA.CTD_RID + " Integer unique primary key ");
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
                if (ctcd.Attr.Equals(CTDRecordA.CTD_RID))
                {
                    noteCmds.Add(ctcd.Attr, "insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + ctcd.Attr + "',0,-1)");
                    continue;
                }
                else if (ctcd.Attr.Equals(CTDRecordA.CTD_PLID) || ctcd.Attr.Equals(CTDRecordA.CTD_LID))
                {
                    noteCmds.Add(ctcd.Attr, "insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + ctcd.Attr + "'," + index + ",-1)");
                }
                else
                {
                    int viewOrder = 2 + (ctcd.IsRequire ? index : CustomTColMap.MaxMainViewCount + index);
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
            DAMBase.executeSQL(sconn, cmdBuilder.ToString());
            DAMBase.executeSQL(sconn, noteCmds.Values.ToArray()); //noteDefaultColMap
        }

        
        /// <summary>
        /// find All CustomTColMap By viewOrder
        /// 说明：
        /// 1.查询条件为select * from CustomTColMap order by viewOrder
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        public static List<CustomTColMap> findAllByOrder(ConnLabel sconn)
        {
            string cmd = "select * from " + typeof(CustomTColMap).Name + " order by viewOrder";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
            {
                return picker.getConnection().Query<CustomTColMap>(cmd).ToList<CustomTColMap>();
            }
        }

        public static int updateViewOrder(ConnLabel sconn, string attr, int viewOrder)
        {
            string cmd = "update " + typeof(CustomTColMap).Name + " set viewOrder=" + viewOrder + " where attr='" + attr + "'";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
            {
                return picker.getConnection().Execute(cmd);
            }
        }

        public static int noteDefaultColMap(ConnLabel sconn, string attr, int dbOrder, int viewOrder)
        {
            string cmd = "Replace or insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + attr + "'," + dbOrder + "," + viewOrder + ")";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
            {
                return picker.getConnection().Execute(cmd);
            }
        }
        public static int noteDefaultColMap(ConnLabel sconn, string[] noteCmds)
        {
            if (noteCmds == null || noteCmds.Length < 1)
                return 0;
            int[] res= DAMBase.executeSQL(sconn, noteCmds);
            return res.Length;
        }
        

        public static int clearColMap(ConnLabel sconn)
        {
            string cmd = "delete from " + typeof(CustomTColMap).Name + "";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
            {
                return picker.getConnection().Execute(cmd);
            }
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        
    }
}
