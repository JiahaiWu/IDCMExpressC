using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using IDCM.Data;

namespace IDCM.Service.Common.DAM
{
    class CatalogNodeDAM
    {
        /// <summary>
        /// 查询第一层节点集合
        /// </summary>
        /// <returns></returns>
        public static List<CatalogNode> findParentNodes(WorkSpaceManager wsm)
        {
            string cmd = "SELECT * FROM " + typeof(CatalogNode).Name + " where pid<0 order by lorder";
            return DataSupporter.ListSQLQuery<CatalogNode>(wsm, cmd);
        }
        /// <summary>
        /// 查询具有指定父节点ID编号的孩子节点集合
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static List<CatalogNode> findSubNodes(WorkSpaceManager wsm,long pid)
        {
            string cmd = "SELECT * FROM " + typeof(CatalogNode).Name + " where pid=" + pid + " order by lorder";
            return DataSupporter.ListSQLQuery<CatalogNode>(wsm, cmd);
        }
        /// <summary>
        /// 查询具有目标主键值的节点记录
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public static CatalogNode findCatalogNode(WorkSpaceManager wsm, long lid)
        {
            string cmd = "SELECT * FROM " + typeof(CatalogNode).Name + " where lid=" + lid;
            List<CatalogNode> res= DataSupporter.ListSQLQuery<CatalogNode>(wsm, cmd);
            return (res != null && res.Count > 0) ? res[0] : null;
        }
        /// <summary>
        /// 获取可检索分类目录映射索引对象
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> getSearchMap(WorkSpaceManager wsm)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            Dictionary<string, int> qdict;
            string cmd = "SELECT Name,Lid FROM " + typeof(CatalogNode).Name + " where pid>0 order by lorder";
            qdict = DataSupporter.SQLQuery<CatalogNode>(wsm, cmd).ToDictionary(rs => (string)rs.Name, rs => (int)rs.Lid);
            dict.Add("Whole Library",CatalogNode.REC_ALL);
            dict.Add("Unfiled Dataset", CatalogNode.REC_UNFILED);
            dict = dict.Concat(qdict).ToDictionary(x => x.Key, x => x.Value);
            return dict;
        }
        /// <summary>
        /// 保存新节点记录
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int saveCatalogNode(WorkSpaceManager wsm,CatalogNode instance)
        {
            int ic = -1;
            if (instance.Lid < 1)
            {
                instance.Lid = DataSupporter.nextSeqID(wsm);//update BaseInfoNote set seqId 返回更新的seqId
                string cmd = "insert into CatalogNode(lid,name,type,pid,lorder) values("
                    + instance.Lid + ",'" + instance.Name + "','" + instance.Type + "'," + (instance.Pid > 0 ? instance.Pid.ToString() : "-1") + "," + instance.Lorder + ");";
                ic = DataSupporter.executeSQL(wsm, cmd);
            }
#if DEBUG
            System.Diagnostics.Debug.Assert(instance.Lid > 0);
#endif
            return ic;
        }
        /// <summary>
        /// 保存新节点记录,包含同级后续节点位序值后移操作。
        /// @Note 请注意和saveCatalogNode区别使用，本方法主要用于特定节点插入情形，而批量归档节点插入模式。
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int insertCatalogNode(WorkSpaceManager wsm, CatalogNode instance)
        {
            int ic = -1;
            if (instance.Lid < 1)
            {
                instance.Lid = DataSupporter.nextSeqID(wsm);
                string cmd = "";
                if (instance.Lorder > -1)
                {
                    cmd += "update " + typeof(CatalogNode).Name + " set lorder=lorder+1 where lorder>=" + instance.Lorder + ";";
                }
                cmd += "insert into " + typeof(CatalogNode).Name + "(lid,name,type,pid,lorder) values("
                    + instance.Lid + ",'" + instance.Name + "','" + instance.Type + "'," + (instance.Pid > 0 ? instance.Pid.ToString() : "null") + "," + instance.Lorder + ");";
                ic = DataSupporter.executeSQL(wsm, cmd);
            }
#if DEBUG
            System.Diagnostics.Debug.Assert(instance.Lid > 0);
#endif
            return ic;
        }
        /// <summary>
        /// 级联删除目标节点以及其直接子节点操作
        /// </summary>
        /// <param name="referId"></param>
        /// <returns></returns>
        public static int delNodeCascaded(WorkSpaceManager wsm, long referId)
        {
            string cmd = "delete FROM " + typeof(CatalogNode).Name + " where pid=" + referId + " or lid=" + referId;
            return DataSupporter.executeSQL(wsm, cmd);
        }
        /// <summary>
        /// 更新特征条件下的记录的某个属性值
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="setName"></param>
        /// <param name="setVal"></param>
        /// <returns></returns>
        public static int updateCatalogNode(WorkSpaceManager wsm, long lid, string setName, object setVal)
        {
            return updateCatalogNode(wsm, lid.ToString(), setName, setVal);
        }
        /// <summary>
        /// 更新特征条件下的记录的某个属性值
        /// </summary>
        /// <param name="filterCond"></param>
        /// <param name="setName"></param>
        /// <param name="setVal"></param>
        /// <returns></returns>
        public static int updateCatalogNode(WorkSpaceManager wsm, string lids, string setName, object setVal)
        {
            string cmd = "update CatalogNode set "
                + setName + "=" + (setVal.GetType().Name.StartsWith("Int") ? setVal : "'" + setVal + "'")
                + " where Lid in (" + lids + ")";
            return DataSupporter.executeSQL(wsm, cmd);
        }
        /// <summary>
        /// 提取目标归档目录及同属节点ID的集合
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public static long[] extractToLids(WorkSpaceManager wsm, long lid)
        {
            if (lid > 0)
            {
                string cmd = "select lid from CatalogNode where lid=" + lid + " or pid=" + lid;
                List<CatalogNode> nodes = DataSupporter.ListSQLQuery<CatalogNode>(wsm, cmd);
                long[] lids = new long[nodes.Count];
                int idx = 0;
                foreach (CatalogNode node in nodes)
                {
                    lids[idx++] = node.Lid;
                }
                return lids;
            }
            return null;
        }
    }
}
