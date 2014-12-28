using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.DAM
{
    class CustomTColMapDAM
    {
        /// <summary>
        /// 创建自定义表实例
        /// </summary>
        public static void buildCustomTable()
        {
            List<CustomTColDef> ctcds = CustomTColDefDAM.loadAll();
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
                    MessageBox.Show("WARN:duplicate column name:\r\n" + ctcd.Attr);
                    Console.WriteLine("WARN:duplicate column name:\r\n" + ctcd.Attr);
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
                    int viewOrder = 2 + (ctcd.IsRequire ? index : ColumnMappingHolder.MaxMainViewCount + index);
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
            SQLiteHelper.ExecuteNonQuery(ConnectStr, cmdBuilder.ToString());
            ColumnMappingHolder.noteDefaultColMap(noteCmds.Values.ToList<string>());
        }

        public static void alterCustomTable_add(CustomTColDef ctcd)
        {
            StringBuilder cmdBuilder = new StringBuilder();
            cmdBuilder.Append("Alter Table " + CTDRecordDAM.table_name + " add column ");
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
            int viewOrder = (ctcd.IsRequire ? ctcd.Corder : ColumnMappingHolder.MaxMainViewCount + ctcd.Corder);
            SQLiteHelper.ExecuteNonQuery(ConnectStr, cmdBuilder.ToString());
            ColumnMappingHolder.noteDefaultColMap(ctcd.Attr, ctcd.Corder, viewOrder);
        }
        public static int noteDefaultColMap(string attr, int dbOrder, int viewOrder)
        {
            string cmd = "Replace or insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + attr + "'," + dbOrder + "," + viewOrder + ")";
            return SQLiteHelper.ExecuteNonQuery(DAMBase.ConnectStr, cmd);
        }
        public static int clearColMap()
        {
            string cmd = "delete from " + typeof(CustomTColMap).Name + "";
            return SQLiteHelper.ExecuteNonQuery(DAMBase.ConnectStr, cmd);
        }
        public static List<CustomTColMap> findAllByOrder()
        {
            string cmd = "select * from " + typeof(CustomTColMap).Name + " order by viewOrder";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                return picker.getConnection().Query<CustomTColMap>(cmd).ToList<CustomTColMap>();
            }
        }
        public static int updateViewOrder(string attr, int viewOrder)
        {
            string cmd = "update " + typeof(CustomTColMap).Name + " set viewOrder=" + viewOrder + " where attr='" + attr + "'";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                return picker.getConnection().Execute(cmd);
            }
        }
        public static string renameCustomTColDefAll()
        {
            string suffix = "_" + BaseInfoNoteDAM.nextSeqID();
            string cmd = "Alter Table " + CTDRecordDAM.table_name + " Rename to " + CTDRecordDAM.table_name + suffix;
            SQLiteHelper.ExecuteNonQuery(ConnectStr, cmd);
            ColumnMappingHolder.clearColMap();
            cmd = "Alter Table " + typeof(CustomTColDef).Name + " Rename to " + typeof(CustomTColDef).Name + suffix;
            SQLiteHelper.ExecuteNonQuery(ConnectStr, cmd);
            return suffix;
        }
    }
}
