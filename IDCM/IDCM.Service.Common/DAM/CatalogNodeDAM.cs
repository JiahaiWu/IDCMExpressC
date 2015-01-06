using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;

namespace IDCM.Service.Common.DAM
{
    class CatalogNodeDAM
    {
        public const int REC_ALL = -1;
        public const int REC_UNFILED = -2;
        public const int REC_TRASH = -4;
        public const int REC_TEMP = -8;

        public enum LibraryNodeType { GroupSet = 0, Group = 1, SmartGroup = 2 };
        /// <summary>
        /// 查询第一层节点集合
        /// </summary>
        /// <returns></returns>
        public static List<CatalogNode> findParentNodes()
        {
            string cmd = "SELECT * FROM " + typeof(CatalogNode).Name + " where pid<0 order by lorder";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                return picker.getConnection().Query<LibraryNode>(cmd).ToList<LibraryNode>();
            }
        }
        /// <summary>
        /// 查询具有指定父节点ID编号的孩子节点集合
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static List<CatalogNode> findSubNodes(long pid)
        {
            string cmd = "SELECT * FROM " + typeof(CatalogNode).Name + " where pid=" + pid + " order by lorder";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                return picker.getConnection().Query<CatalogNode>(cmd).ToList<CatalogNode>();
            }
        }
        /// <summary>
        /// 查询具有目标主键值的节点记录
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public static CatalogNode findLibraryNode(long lid)
        {
            string cmd = "SELECT * FROM " + typeof(CatalogNode).Name + " where lid=" + lid;
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                return picker.getConnection().Query<CatalogNode>(cmd).First();
            }
        }
        /// <summary>
        /// 获取可检索分类目录映射索引对象
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> getSearchMap()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            Dictionary<string, int> qdict;
            string cmd = "SELECT Name,Lid FROM " + typeof(CatalogNode).Name + " where pid>0 order by lorder";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                qdict = picker.getConnection().Query(cmd).ToDictionary(rs => (string)rs.Name, rs => (int)rs.Lid);
            }
            dict.Add("Whole Library", REC_ALL);
            dict.Add("Unfiled Dataset", REC_UNFILED);
            dict = dict.Concat(qdict).ToDictionary(x => x.Key, x => x.Value);
            return dict;
        }
        /// <summary>
        /// 保存新节点记录
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int saveLibraryNode(CatalogNode instance)
        {
            int ic = -1;
            if (instance.Lid < 1)
            {
                instance.Lid = BaseInfoNoteDAM.nextSeqID();//update BaseInfoNote set seqId 返回更新的seqId
                string cmd = "insert into LibraryNode(lid,name,type,pid,lorder) values("
                    + instance.Lid + ",'" + instance.Name + "','" + instance.Type + "'," + (instance.Pid > 0 ? instance.Pid.ToString() : "-1") + "," + instance.Lorder + ");";
                ic = SQLiteHelper.ExecuteNonQuery(ConnectStr, cmd);
            }
#if DEBUG
            System.Diagnostics.Debug.Assert(instance.Lid > 0);
#endif
            return ic;
        }
        /// <summary>
        /// 保存新节点记录,包含同级后续节点位序值后移操作。
        /// @Note 请注意和saveLibraryNode区别使用，本方法主要用于特定节点插入情形，而批量归档节点插入模式。
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int insertLibraryNode(CatalogNode instance)
        {
            int ic = -1;
            if (instance.Lid < 1)
            {
                instance.Lid = BaseInfoNoteDAM.nextSeqID();
                string cmd = "";
                if (instance.Lorder > -1)
                {
                    cmd += "update " + typeof(CatalogNode).Name + " set lorder=lorder+1 where lorder>=" + instance.Lorder + ";";
                }
                cmd += "insert into " + typeof(LibraryNode).Name + "(lid,name,type,pid,lorder) values("
                    + instance.Lid + ",'" + instance.Name + "','" + instance.Type + "'," + (instance.Pid > 0 ? instance.Pid.ToString() : "null") + "," + instance.Lorder + ");";
                ic = SQLiteHelper.ExecuteNonQuery(ConnectStr, cmd);
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
        public static int delNodeCascaded(long referId)
        {
            string cmd = "delete FROM " + typeof(CatalogNode).Name + " where pid=" + referId + " or lid=" + referId;
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                return picker.getConnection().Execute(cmd);
            }
        }
        /// <summary>
        /// 更新特征条件下的记录的某个属性值
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="setName"></param>
        /// <param name="setVal"></param>
        /// <returns></returns>
        public static int updateLibraryNode(long lid, string setName, object setVal)
        {
            return updateLibraryNode(lid.ToString(), setName, setVal);
        }
        /// <summary>
        /// 更新特征条件下的记录的某个属性值
        /// </summary>
        /// <param name="filterCond"></param>
        /// <param name="setName"></param>
        /// <param name="setVal"></param>
        /// <returns></returns>
        public static int updateLibraryNode(string lids, string setName, object setVal)
        {
            string cmd = "update LibraryNode set "
                + setName + "=" + (setVal.GetType().Name.StartsWith("Int") ? setVal : "'" + setVal + "'")
                + " where Lid in (" + lids + ")";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                return picker.getConnection().Execute(cmd);
            }
        }
        /// <summary>
        /// 提取目标归档目录及同属节点ID的集合
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public static long[] extractToLids(long lid)
        {
            if (lid > 0)
            {
                string cmd = "select lid from LibraryNode where lid=" + lid + " or pid=" + lid;
                DataTable table = SQLiteHelper.ExecuteDataTable(ConnectStr, cmd);
                long[] lids = new long[table.Rows.Count];
                int idx = 0;
                foreach (DataRow dr in table.Rows)
                {
                    lids[idx] = Convert.ToInt64(dr[0]);
                    ++idx;
                }
                return lids;
            }
            return null;
        }
    }
}
