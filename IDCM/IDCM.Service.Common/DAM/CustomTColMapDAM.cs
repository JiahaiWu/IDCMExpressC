using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using IDCM.Data.Base.Utils;
using IDCM.Data;


namespace IDCM.Service.Common.DAM
{
    class CustomTColMapDAM
    {
        /// <summary>
        /// 创建自定义表实例
        /// </summary>
        public static void buildCustomTable(WorkSpaceManager wsm)
        {
            List<CustomTColDef> ctcds = CustomTColDefDAM.loadAll(wsm);
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
                    log.Warn("duplicate column name:\r\n" + ctcd.Attr);
                    continue;
                }
                if (ctcd.Attr.Equals(CTDRecordA.CTD_RID))
                {
                    noteCmds.Add(ctcd.Attr, "insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + ctcd.Attr + "',0,-2)");
                    continue;
                }
                else if (ctcd.Attr.Equals(CTDRecordA.CTD_PLID) || ctcd.Attr.Equals(CTDRecordA.CTD_LID))
                {
                    noteCmds.Add(ctcd.Attr, "insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + ctcd.Attr + "'," + index + ",-1)");
                }
                else
                {
                    int viewOrder = 3 + (ctcd.IsRequire ? index : CustomTColMap.MaxMainViewCount + index);
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
            DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
            DataSupporter.noteDefaultColMap(wsm,noteCmds.Values.ToList<string>());
        }

        public static void alterCustomTable_add(WorkSpaceManager wsm,CustomTColDef ctcd)
        {
            StringBuilder cmdBuilder = new StringBuilder();
            cmdBuilder.Append("Alter Table " + CTDRecordA.table_name + " add column ");
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
            int viewOrder = (ctcd.IsRequire ? ctcd.Corder : CustomTColMap.MaxMainViewCount + ctcd.Corder);
            DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
            DataSupporter.noteDefaultColMap(wsm, ctcd.Attr, ctcd.Corder, viewOrder);
        }
        public static List<CustomTColMap> findAllByOrder(WorkSpaceManager wsm)
        {
            string cmd = "select * from " + typeof(CustomTColMap).Name + " order by viewOrder";
            return DataSupporter.ListSQLQuery<CustomTColMap>(wsm, cmd);
        }
        public static int updateViewOrder(WorkSpaceManager wsm,string attr, int viewOrder)
        {
            string cmd = "update " + typeof(CustomTColMap).Name + " set viewOrder=" + viewOrder + " where attr='" + attr + "'";
            return DataSupporter.executeSQL(wsm, cmd);
        }
        public static string renameCustomTColDefAll(WorkSpaceManager wsm)
        {
            string suffix = "_" + DataSupporter.nextSeqID(wsm);
            string cmd = "Alter Table " + CTDRecordA.table_name + " Rename to " + CTDRecordA.table_name + suffix;
            DataSupporter.executeSQL(wsm, cmd);
            DataSupporter.clearColMap(wsm);
            cmd = "Alter Table " + typeof(CustomTColDef).Name + " Rename to " + typeof(CustomTColDef).Name + suffix;
            DataSupporter.executeSQL(wsm, cmd);
            return suffix;
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        
    }
}
