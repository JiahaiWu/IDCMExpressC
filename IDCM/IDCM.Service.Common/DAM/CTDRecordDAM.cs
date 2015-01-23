using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using System.Data;
using IDCM.Data;
using IDCM.Data.Base.Utils;

namespace IDCM.Service.Common.DAM
{
    class CTDRecordDAM
    {
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        /// <returns></returns>
        public static DataTable queryCTDRecord(WorkSpaceManager wsm, string nodeIds = null, string rids = null)
        {
            string cmdstr = null;
            return queryCTDRecord(wsm,nodeIds, rids, out cmdstr);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        /// <returns></returns>
        public static DataTable queryCTDRecord(WorkSpaceManager wsm, string nodeIds, string rids, out string cmdstr)
        {
            StringBuilder cmdBuilder = new StringBuilder("SELECT * FROM " + CTDRecordA.table_name);
            if (nodeIds != null || rids != null)
            {
                cmdBuilder.Append(" where ");
                if (nodeIds != null)
                {
                    cmdBuilder.Append(CTDRecordA.CTD_LID);
                    if (nodeIds.StartsWith("-"))
                    {
                        if (nodeIds.Equals(CatalogNode.REC_ALL.ToString()))
                        {
                            cmdBuilder.Append("<>" + CatalogNode.REC_TRASH);
                        }
                        else if (nodeIds.Equals(CatalogNode.REC_TRASH.ToString()))
                        {
                            cmdBuilder.Append("=" + CatalogNode.REC_TRASH);
                        }
                        else if (nodeIds.Equals(CatalogNode.REC_TEMP.ToString()))
                        {
                            cmdBuilder.Append("=" + CatalogNode.REC_TEMP);
                        }
                        else if (nodeIds.Equals(CatalogNode.REC_UNFILED.ToString()))
                        {
                            cmdBuilder.Append("=" + CatalogNode.REC_UNFILED).Append(" or " + CTDRecordA.CTD_RID + " is NULL ");
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
                    cmdBuilder.Append(" ").Append(CTDRecordA.CTD_RID).Append(" in (").Append(rids).Append(") ");
            }
            cmdstr = cmdBuilder.ToString();
            return DataSupporter.SQLQuery(wsm, cmdstr); 
        }
        /// <summary>
        /// 根据历史记录SQL，再次查询结果集
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <returns></returns>
        public static DataTable queryCTDRecordByHistSQL(WorkSpaceManager wsm, string histCmd, long limit = 0, long offset = 0)
        {
            if (limit < 1)
                limit = Int16.MaxValue;
            string cmdstr = histCmd + " limit " + limit + " offset " + offset;
            return DataSupporter.SQLQuery(wsm, cmdstr);
        }
        /// <summary>
        /// 查询记录计数值
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <returns></returns>
        public static Dictionary<int, long> countCTDRecord(WorkSpaceManager wsm, string nodeIds = null)
        {
            Dictionary<int, long> dict = new Dictionary<int, long>();
            StringBuilder cmdBuilder = new StringBuilder("SELECT count(" + CTDRecordA.CTD_RID + ") as ccount," + CTDRecordA.CTD_LID + " as clid FROM " + CTDRecordA.table_name);

            if (nodeIds != null)
            {
                cmdBuilder.Append(" where ");
                cmdBuilder.Append(CTDRecordA.CTD_LID);
                if (nodeIds.StartsWith("-"))
                {
                    if (nodeIds.Equals(CatalogNode.REC_ALL.ToString()))
                    {
                        cmdBuilder.Append("<>" + CatalogNode.REC_TRASH);
                    }
                    else if (nodeIds.Equals(CatalogNode.REC_TRASH.ToString()))
                    {
                        cmdBuilder.Append("=" + CatalogNode.REC_TRASH);
                    }
                    else if (nodeIds.Equals(CatalogNode.REC_TEMP.ToString()))
                    {
                        cmdBuilder.Append("=" + CatalogNode.REC_TEMP);
                    }
                    else if (nodeIds.Equals(CatalogNode.REC_UNFILED.ToString()))
                    {
                        cmdBuilder.Append("=" + CatalogNode.REC_UNFILED).Append(" or " + CTDRecordA.CTD_RID + " is NULL ");
                    }
                    else
                    {
                        throw new NotSupportedException("Un Supported query condition using countCTDRecord Method! The condition named is " + nodeIds + ".");
                    }
                }
                else
                    cmdBuilder.Append(" in (").Append(nodeIds).Append(") ");
                long res = DataSupporter.SQLQuery<dynamic>(wsm, cmdBuilder.ToString()).Sum(rs => (long)(rs.ccount));
                dict[Convert.ToInt32(nodeIds)] = Convert.ToInt64(res);
            }
            else
            {
                cmdBuilder.Append(" group by ").Append(CTDRecordA.CTD_LID);
                long c_all = DataSupporter.SQLQuery<dynamic>(wsm, cmdBuilder.ToString())
                    .Sum(rs => (int)rs.clid == CatalogNode.REC_TRASH ? 0 : (long)rs.ccount);
                dict[CatalogNode.REC_ALL] = c_all;
            }
            return dict;
        }
        /// <summary>
        /// 更新目标数据的归档目录属性信息
        /// </summary>
        /// <param name="newlid"></param>
        /// <param name="newplid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public static int updateCTCRecordLid(WorkSpaceManager wsm,int newlid, int newplid = CatalogNode.REC_UNFILED, long rid = -1)
        {
            if (rid > 0)
            {
                string cmd = " update " + CTDRecordA.table_name + " set " + CTDRecordA.CTD_LID + "=" + newlid + "," + CTDRecordA.CTD_PLID + "=" + newplid
                    + " where " + CTDRecordA.CTD_RID + "=" + rid;
                return DataSupporter.executeSQL(wsm,cmd);
            }
            return -1;
        }
        /// <summary>
        /// 更新目标记录的归档目录属性信息
        /// </summary>
        /// <param name="newlid"></param>
        /// <param name="newplid"></param>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        public static int updateCTCRecordLid(WorkSpaceManager wsm, int newlid, int newplid = CatalogNode.REC_UNFILED, string nodeIds = null, string rids = null)
        {
            StringBuilder cmdBuilder = new StringBuilder("update " + CTDRecordA.table_name + " set " + CTDRecordA.CTD_LID + "=" + newlid + "," + CTDRecordA.CTD_PLID + "=" + newplid);
            if (nodeIds != null || rids != null)
            {
                cmdBuilder.Append(" where ");
                if (nodeIds != null)
                {
                    cmdBuilder.Append(CTDRecordA.CTD_LID);
                    if (nodeIds.StartsWith("-"))
                    {
                        if (nodeIds.Equals(CatalogNode.REC_ALL.ToString()))
                        {
                            cmdBuilder.Append("<>" + CatalogNode.REC_TRASH);
                        }
                        else if (nodeIds.Equals(CatalogNode.REC_TRASH.ToString()))
                        {
                            cmdBuilder.Append("=" + CatalogNode.REC_TRASH);
                        }
                        else if (nodeIds.Equals(CatalogNode.REC_TEMP.ToString()))
                        {
                            cmdBuilder.Append("=" + CatalogNode.REC_TEMP);
                        }
                        else if (nodeIds.Equals(CatalogNode.REC_UNFILED.ToString()))
                        {
                            cmdBuilder.Append("=" + CatalogNode.REC_UNFILED).Append(" or " + CTDRecordA.CTD_RID + " is NULL ");
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
                    cmdBuilder.Append(" ").Append(CTDRecordA.CTD_RID).Append(" in (").Append(rids).Append(") ");
            }
            return DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
        }
        /// <summary>
        /// 彻底删除目标归档目录的数据记录
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        public static void dropCTCRecordLid(WorkSpaceManager wsm, string nodeIds = null, string rids = null)
        {
            StringBuilder cmdBuilder = new StringBuilder("delete from " + CTDRecordA.table_name);
            if (nodeIds != null || rids != null)
            {
                cmdBuilder.Append(" where ");
                if (nodeIds != null)
                {
                    cmdBuilder.Append(CTDRecordA.CTD_LID);
                    if (nodeIds.StartsWith("-"))
                    {
                        if (nodeIds.Equals(CatalogNode.REC_ALL.ToString()))
                        {
                            cmdBuilder.Append("<>" + CatalogNode.REC_TRASH);
                        }
                        else if (nodeIds.Equals(CatalogNode.REC_TRASH.ToString()))
                        {
                            cmdBuilder.Append("=" + CatalogNode.REC_TRASH);
                        }
                        else if (nodeIds.Equals(CatalogNode.REC_TEMP.ToString()))
                        {
                            cmdBuilder.Append("=" + CatalogNode.REC_TEMP);
                        }
                        else if (nodeIds.Equals(CatalogNode.REC_UNFILED.ToString()))
                        {
                            cmdBuilder.Append("=" + CatalogNode.REC_UNFILED).Append(" or " + CTDRecordA.CTD_RID + " is NULL ");
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
                    cmdBuilder.Append(" ").Append(CTDRecordA.CTD_RID).Append(" in (").Append(rids).Append(") ");
            }
            DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
        }

        /// <summary>
        /// 更新数据记录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cellVal"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public static int updateAttrVal(WorkSpaceManager wsm, string rid, string cellVal, string attrName)
        {
            StringBuilder cmdBuilder = new StringBuilder();
            string attr = attrName;
            cmdBuilder.Append("update ").Append(CTDRecordA.table_name).Append(" set ").Append(attr).Append("=");
            if (AttrTypeConverter.getSQLiteType(attrName).Equals("Text", StringComparison.CurrentCultureIgnoreCase))
            {
                cmdBuilder.Append("'").Append(cellVal).Append("'");
            }
            else
            {
                cmdBuilder.Append(cellVal);
            }
            cmdBuilder.Append(" where " + CTDRecordA.CTD_RID + "=" + rid);
            return DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
        }
        /// <summary>
        /// 添加新数据记录
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="plid"></param>
        /// <returns></returns>
        public static long addNewRecord(WorkSpaceManager wsm, long lid = CatalogNode.REC_UNFILED, long plid = CatalogNode.REC_ALL)
        {
            StringBuilder cmdBuilder = new StringBuilder();
            long rid = DataSupporter.nextSeqID(wsm);
            cmdBuilder.Append("insert into " + CTDRecordA.table_name + "(" + CTDRecordA.CTD_RID + "," + CTDRecordA.CTD_LID + "," + CTDRecordA.CTD_PLID + ") values(" + rid + "," + lid + "," + plid + ")");
            DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
            return rid;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="uid"></param>
        public static int deleteRec(WorkSpaceManager wsm, long rid)
        {
            string cmd = "delete from " + CTDRecordA.table_name + " where " + CTDRecordA.CTD_RID + "=" + rid;
            return DataSupporter.executeSQL(wsm, cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapValues"></param>
        /// <returns></returns>
        public static long mergeRecord(WorkSpaceManager wsm, Dictionary<string, string> mapValues)
        {
            string rid = null;
            mapValues.TryGetValue(CTDRecordA.CTD_RID, out rid);
            if (rid == null)
            {
                rid = DataSupporter.nextSeqID(wsm).ToString();
            }
            else
                mapValues.Remove(CTDRecordA.CTD_RID);
            if (!mapValues.ContainsKey(CTDRecordA.CTD_LID))
                mapValues.Add(CTDRecordA.CTD_LID, CatalogNode.REC_UNFILED.ToString());
            if (!mapValues.ContainsKey(CTDRecordA.CTD_PLID))
                mapValues.Add(CTDRecordA.CTD_PLID, CatalogNode.REC_ALL.ToString());
            StringBuilder cmdBuilder = new StringBuilder();
            cmdBuilder.Append("replace into " + CTDRecordA.table_name + "(" + CTDRecordA.CTD_RID);
            foreach (string key in mapValues.Keys)
            {
                cmdBuilder.Append(",").Append(key);
            }
            cmdBuilder.Append(") values (" + rid);
            foreach (KeyValuePair<string, string> kvpair in mapValues)
            {
                CustomTColDef ctcd = CustomTColDefDAM.getCustomTColDef(wsm,kvpair.Key);
                string dbtype = AttrTypeConverter.getSQLiteType(ctcd.AttrType);
                if (dbtype.Equals("Text"))
                {
                    cmdBuilder.Append(",").Append("'" + kvpair.Value + "'");
                }
                else
                    cmdBuilder.Append(",").Append(kvpair.Value);
            }
            cmdBuilder.Append(")");
            DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
            return Convert.ToInt64(rid);
        }
    }
}
