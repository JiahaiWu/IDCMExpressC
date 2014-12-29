using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.DAM
{
    class CTDRecordDAM
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
        /// Record 从属父级目录Library ID的列名设定，没有从属特定节点时LibraryNodeDA.REC_UNFILED
        /// </summary>
        public const string CTD_PLID = "ctd_plid";
        /// <summary>
        /// Record 从属目录的Library ID的列名设定，没有从属特定节点时为LibraryNodeDA.REC_UNFILED
        /// </summary>
        public const string CTD_LID = "ctd_lid";
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        /// <returns></returns>
        public static DataTable queryCTDRecord(string nodeIds = null, string rids = null)
        {
            string cmdstr = null;
            return queryCTDRecord(nodeIds, rids, out cmdstr);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        /// <returns></returns>
        public static DataTable queryCTDRecord(string nodeIds, string rids, out string cmdstr)
        {
            StringBuilder cmdBuilder = new StringBuilder("SELECT * FROM " + table_name);
            if (nodeIds != null || rids != null)
            {
                cmdBuilder.Append(" where ");
                if (nodeIds != null)
                {
                    cmdBuilder.Append(CTDRecordDAM.CTD_LID);
                    if (nodeIds.StartsWith("-"))
                    {
                        if (nodeIds.Equals(LibraryNodeDAM.REC_ALL.ToString()))
                        {
                            cmdBuilder.Append("<>" + LibraryNodeDAM.REC_TRASH);
                        }
                        else if (nodeIds.Equals(LibraryNodeDAM.REC_TRASH.ToString()))
                        {
                            cmdBuilder.Append("=" + LibraryNodeDAM.REC_TRASH);
                        }
                        else if (nodeIds.Equals(LibraryNodeDAM.REC_TEMP.ToString()))
                        {
                            cmdBuilder.Append("=" + LibraryNodeDAM.REC_TEMP);
                        }
                        else if (nodeIds.Equals(LibraryNodeDAM.REC_UNFILED.ToString()))
                        {
                            cmdBuilder.Append("=" + LibraryNodeDAM.REC_UNFILED).Append(" or " + CTDRecordDAM.CTD_RID + " is NULL ");
                        }
                        else
                        {
                            throw new NotSupportedException("Un Supported query condition using queryCTDRecord Method! The condition named is " + nodeIds + ".");
                        }
                    }
                    else
                        cmdBuilder.Append(" in (").Append(nodeIds).Append(") ");
                }
                if (rids != null)
                    cmdBuilder.Append(" ").Append(CTD_RID).Append(" in (").Append(rids).Append(") ");
            }
            cmdstr = cmdBuilder.ToString();
            DataTable table = SQLiteHelper.ExecuteDataTable(ConnectStr, cmdstr);
            return table;
        }
        /// <summary>
        /// 根据历史记录SQL，再次查询结果集
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <returns></returns>
        public static DataTable queryCTDRecordByHistSQL(string histCmd, long limit = 0, long offset = 0)
        {
            if (limit < 1)
                limit = Int16.MaxValue;
            string cmdstr = histCmd + " limit " + limit + " offset " + offset;
            DataTable table = SQLiteHelper.ExecuteDataTable(ConnectStr, cmdstr);
            return table;
        }
        /// <summary>
        /// 查询记录计数值
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <returns></returns>
        public static Dictionary<int, long> countCTDRecord(string nodeIds = null)
        {
            Dictionary<int, long> dict = new Dictionary<int, long>();
            StringBuilder cmdBuilder = new StringBuilder("SELECT count(" + CTDRecordDAM.CTD_RID + ")," + CTDRecordDAM.CTD_LID + " FROM " + table_name);

            if (nodeIds != null)
            {
                cmdBuilder.Append(" where ");
                cmdBuilder.Append(CTDRecordDAM.CTD_LID);
                if (nodeIds.StartsWith("-"))
                {
                    if (nodeIds.Equals(LibraryNodeDAM.REC_ALL.ToString()))
                    {
                        cmdBuilder.Append("<>" + LibraryNodeDAM.REC_TRASH);
                    }
                    else if (nodeIds.Equals(LibraryNodeDAM.REC_TRASH.ToString()))
                    {
                        cmdBuilder.Append("=" + LibraryNodeDAM.REC_TRASH);
                    }
                    else if (nodeIds.Equals(LibraryNodeDAM.REC_TEMP.ToString()))
                    {
                        cmdBuilder.Append("=" + LibraryNodeDAM.REC_TEMP);
                    }
                    else if (nodeIds.Equals(LibraryNodeDAM.REC_UNFILED.ToString()))
                    {
                        cmdBuilder.Append("=" + LibraryNodeDAM.REC_UNFILED).Append(" or " + CTDRecordDAM.CTD_RID + " is NULL ");
                    }
                    else
                    {
                        throw new NotSupportedException("Un Supported query condition using countCTDRecord Method! The condition named is " + nodeIds + ".");
                    }
                }
                else
                    cmdBuilder.Append(" in (").Append(nodeIds).Append(") ");
                object res = SQLiteHelper.ExecuteScalar(ConnectStr, cmdBuilder.ToString());
                dict[Convert.ToInt32(nodeIds)] = Convert.ToInt64(res);
            }
            else
            {
                cmdBuilder.Append(" group by ").Append(CTDRecordDAM.CTD_LID);
                DataTable table = SQLiteHelper.ExecuteDataTable(ConnectStr, cmdBuilder.ToString());
                long c_all = 0;
                foreach (DataRow dr in table.Rows)
                {
                    long cc = Convert.ToInt64(dr[0]);
                    int lid = Convert.ToInt32(dr[1]);
                    if (lid != LibraryNodeDAM.REC_TRASH)
                        c_all += cc;
                    dict[lid] = cc;
                }
                dict[LibraryNodeDAM.REC_ALL] = c_all;
            }
            return dict;
        }
        /// <summary>
        /// 更新目标记录的归档目录属性信息
        /// </summary>
        /// <param name="newlid"></param>
        /// <param name="newplid"></param>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        public static void updateCTCRecordLid(int newlid, int newplid = LibraryNodeDAM.REC_UNFILED, string nodeIds = null, string rids = null)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            StringBuilder cmdBuilder = new StringBuilder("update " + table_name + " set " + CTD_LID + "=" + newlid + "," + CTD_PLID + "=" + newplid);
            if (nodeIds != null || rids != null)
            {
                cmdBuilder.Append(" where ");
                if (nodeIds != null)
                {
                    cmdBuilder.Append(CTDRecordDAM.CTD_LID);
                    if (nodeIds.StartsWith("-"))
                    {
                        if (nodeIds.Equals(LibraryNodeDAM.REC_ALL.ToString()))
                        {
                            cmdBuilder.Append("<>" + LibraryNodeDAM.REC_TRASH);
                        }
                        else if (nodeIds.Equals(LibraryNodeDAM.REC_TRASH.ToString()))
                        {
                            cmdBuilder.Append("=" + LibraryNodeDAM.REC_TRASH);
                        }
                        else if (nodeIds.Equals(LibraryNodeDAM.REC_TEMP.ToString()))
                        {
                            cmdBuilder.Append("=" + LibraryNodeDAM.REC_TEMP);
                        }
                        else if (nodeIds.Equals(LibraryNodeDAM.REC_UNFILED.ToString()))
                        {
                            cmdBuilder.Append("=" + LibraryNodeDAM.REC_UNFILED).Append(" or " + CTDRecordDAM.CTD_RID + " is NULL ");
                        }
                        else
                        {
                            throw new NotSupportedException("Un Supported query condition using updateCTCRecordLid Method! The condition named is " + nodeIds + ".");
                        }
                    }
                    else
                        cmdBuilder.Append(" in (").Append(nodeIds).Append(") ");
                }
                if (rids != null)
                    cmdBuilder.Append(" ").Append(CTD_RID).Append(" in (").Append(rids).Append(") ");
            }
            SQLiteHelper.ExecuteNonQuery(ConnectStr, cmdBuilder.ToString());
        }
        /// <summary>
        /// 彻底删除目标归档目录的数据记录
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        public static void dropCTCRecordLid(string nodeIds = null, string rids = null)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            StringBuilder cmdBuilder = new StringBuilder("delete from " + table_name);
            if (nodeIds != null || rids != null)
            {
                cmdBuilder.Append(" where ");
                if (nodeIds != null)
                {
                    cmdBuilder.Append(CTDRecordDAM.CTD_LID);
                    if (nodeIds.StartsWith("-"))
                    {
                        if (nodeIds.Equals(LibraryNodeDAM.REC_ALL.ToString()))
                        {
                            cmdBuilder.Append("<>" + LibraryNodeDAM.REC_TRASH);
                        }
                        else if (nodeIds.Equals(LibraryNodeDAM.REC_TRASH.ToString()))
                        {
                            cmdBuilder.Append("=" + LibraryNodeDAM.REC_TRASH);
                        }
                        else if (nodeIds.Equals(LibraryNodeDAM.REC_TEMP.ToString()))
                        {
                            cmdBuilder.Append("=" + LibraryNodeDAM.REC_TEMP);
                        }
                        else if (nodeIds.Equals(LibraryNodeDAM.REC_UNFILED.ToString()))
                        {
                            cmdBuilder.Append("=" + LibraryNodeDAM.REC_UNFILED).Append(" or " + CTDRecordDAM.CTD_RID + " is NULL ");
                        }
                        else
                        {
                            throw new NotSupportedException("Un Supported query condition using dropCTCRecordLid Method! The condition named is " + nodeIds + ".");
                        }
                    }
                    else
                        cmdBuilder.Append(" in (").Append(nodeIds).Append(") ");
                }
                if (rids != null)
                    cmdBuilder.Append(" ").Append(CTD_RID).Append(" in (").Append(rids).Append(") ");
            }
            SQLiteHelper.ExecuteNonQuery(ConnectStr, cmdBuilder.ToString());
        }

        /// <summary>
        /// 更新数据记录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cellVal"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public static int updateAttrVal(string rid, string cellVal, string attrName)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            StringBuilder cmdBuilder = new StringBuilder();
            string attr = attrName;
            cmdBuilder.Append("update ").Append(table_name).Append(" set ").Append(attr).Append("=");
            if (AttrTypeConverter.getSQLiteType(attrName).Equals("Text", StringComparison.CurrentCultureIgnoreCase))
            {
                cmdBuilder.Append("'").Append(cellVal).Append("'");
            }
            else
            {
                cmdBuilder.Append(cellVal);
            }
            cmdBuilder.Append(" where " + CTD_RID + "=" + rid);
            return SQLiteHelper.ExecuteNonQuery(ConnectStr, cmdBuilder.ToString());
        }
        /// <summary>
        /// 添加新数据记录
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="plid"></param>
        /// <returns></returns>
        public static long addNewRecord(long lid = LibraryNodeDAM.REC_UNFILED, long plid = LibraryNodeDAM.REC_ALL)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            StringBuilder cmdBuilder = new StringBuilder();
            long rid = BaseInfoNoteDAM.nextSeqID();
            cmdBuilder.Append("insert into " + table_name + "(" + CTD_RID + "," + CTD_LID + "," + CTD_PLID + ") values(" + rid + "," + lid + "," + plid + ")");
            SQLiteHelper.ExecuteNonQuery(ConnectStr, cmdBuilder.ToString());
            return rid;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="uid"></param>
        public static int deleteRec(long rid)
        {
            string cmd = "delete from " + table_name + " where " + CTD_RID + "=" + rid;
            return SQLiteHelper.ExecuteNonQuery(ConnectStr, cmd);
        }
        /// <summary>
        /// 更新目标数据的归档目录属性信息
        /// </summary>
        /// <param name="newlid"></param>
        /// <param name="newplid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public static int updateCTCRecordLid(int newlid, int newplid = LibraryNodeDAM.REC_UNFILED, long rid = -1)
        {
            if (rid > 0)
            {
                string cmd = " update " + table_name + " set " + CTD_LID + "=" + newlid + "," + CTD_PLID + "=" + newplid
                    + " where " + CTD_RID + "=" + rid;
                return SQLiteHelper.ExecuteNonQuery(ConnectStr, cmd);
            }
            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapValues"></param>
        /// <returns></returns>
        public static long mergeRecord(Dictionary<string, string> mapValues)
        {
            string rid = null;
            mapValues.TryGetValue(CTD_RID, out rid);
            if (rid == null)
            {
                rid = BaseInfoNoteDAM.nextSeqID().ToString();
            }
            else
                mapValues.Remove(CTD_RID);
            if (!mapValues.ContainsKey(CTD_LID))
                mapValues.Add(CTD_LID, LibraryNodeDAM.REC_UNFILED.ToString());
            if (!mapValues.ContainsKey(CTD_PLID))
                mapValues.Add(CTD_PLID, LibraryNodeDAM.REC_ALL.ToString());
            SQLiteCommand cmd = new SQLiteCommand();
            StringBuilder cmdBuilder = new StringBuilder();
            cmdBuilder.Append("replace into " + table_name + "(" + CTD_RID);
            foreach (string key in mapValues.Keys)
            {
                cmdBuilder.Append(",").Append(key);
            }
            cmdBuilder.Append(") values (" + rid);
            foreach (KeyValuePair<string, string> kvpair in mapValues)
            {
                CustomTColDef ctcd = CustomTColDefDAM.getCustomTColDef(kvpair.Key);
                string dbtype = AttrTypeConverter.getSQLiteType(ctcd.AttrType);
                if (dbtype.Equals("Text"))
                {
                    cmdBuilder.Append(",").Append("'" + kvpair.Value + "'");
                }
                else
                    cmdBuilder.Append(",").Append(kvpair.Value);
            }
            cmdBuilder.Append(")");
            SQLiteHelper.ExecuteNonQuery(ConnectStr, cmdBuilder.ToString());
            return Convert.ToInt64(rid);
        }
    }
}
