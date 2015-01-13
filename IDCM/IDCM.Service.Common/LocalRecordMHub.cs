using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Service.Common.DAM;
using System.Data;
using IDCM.Service.Common.Core;
using IDCM.Data;
using IDCM.Data.Base;

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
        /// 查询记录计数值
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <returns></returns>
        public static Dictionary<int, long> countCTDRecord(DataSourceMHub datasource, string nodeIds = null)
        {
            return CTDRecordDAM.countCTDRecord(datasource.WSM, nodeIds);
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
        /// 数据表单查询条件语句缓存
        /// </summary>
        /// <param name="cmdstr"></param>
        public static void cacheDGVQuery(string cmdstr, int tcount)
        {
            QueryCmdCache.cacheDGVQuery(cmdstr, tcount);
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
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        /// <returns></returns>
        public static DataTable queryCTDRecord(DataSourceMHub datasource, string nodeIds, string rids, out string cmdstr)
        {
            return CTDRecordDAM.queryCTDRecord(datasource.WSM, nodeIds, rids,out cmdstr);
        }
        /// <summary>
        /// 获取预览字段集序列
        /// </summary>
        /// <returns></returns>
        public static List<string> getViewAttrs(DataSourceMHub datasource, bool withInnerField = true)
        {
            return DataSupporter.getViewAttrs(datasource.WSM,withInnerField);
        }
        /// <summary>
        /// 提取目标归档目录及同属节点ID的集合
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public static long[] extractToLids(DataSourceMHub datasource, long lid)
        {
            return CatalogNodeDAM.extractToLids(datasource.WSM, lid);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="uid"></param>
        public static int deleteRec(DataSourceMHub datasource, long rid)
        {
            return CTDRecordDAM.deleteRec(datasource.WSM, rid);
        }
        /// <summary>
        /// 更新目标记录的归档目录属性信息
        /// </summary>
        /// <param name="newlid"></param>
        /// <param name="newplid"></param>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        public static int updateCTCRecordLid(DataSourceMHub datasource,int newlid, int newplid = CatalogNode.REC_UNFILED, string nodeIds = null, string rids = null)
        {
            return CTDRecordDAM.updateCTCRecordLid(datasource.WSM, newlid, newplid, nodeIds, rids);
        }
        /// <summary>
        /// 更新目标数据的归档目录属性信息
        /// </summary>
        /// <param name="newlid"></param>
        /// <param name="newplid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        public static int updateCTCRecordLid(DataSourceMHub datasource, int newlid, int newplid = CatalogNode.REC_UNFILED, long rid = -1)
        {
            return CTDRecordDAM.updateCTCRecordLid(datasource.WSM, newlid, newplid, rid);
        }
        /// <summary>
        /// 彻底删除目标归档目录的数据记录
        /// </summary>
        /// <param name="nodeIds"></param>
        /// <param name="rids"></param>
        public static void dropCTCRecordLid(DataSourceMHub datasource, string nodeIds = null, string rids = null)
        {
            CTDRecordDAM.dropCTCRecordLid(datasource.WSM, nodeIds, rids);
        }
        /// <summary>
        /// 获取存储字段序列值(如查找失败返回-1)
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static int getDBOrder(DataSourceMHub datasource, string attr)
        {
            return DataSupporter.getDBOrder(datasource.WSM, attr);
        }
        /// <summary>
        /// 获取预览字段位序值(如查找失败返回-1)
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static int getViewOrder(DataSourceMHub datasource, string attr)
        {
            return DataSupporter.getViewOrder(datasource.WSM, attr);
        }
        /// <summary>
        /// 添加新数据记录
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="plid"></param>
        /// <returns></returns>
        public static long addNewRecord(DataSourceMHub datasource,long lid = CatalogNode.REC_UNFILED, long plid= CatalogNode.REC_ALL)
        {
            return CTDRecordDAM.addNewRecord(datasource.WSM, lid, plid);
        }
        /// <summary>
        /// 更新数据记录
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="cellVal"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public static int updateAttrVal(DataSourceMHub datasource,string rid, string cellVal, string attrName)
        {
            return CTDRecordDAM.updateAttrVal(datasource.WSM, rid, cellVal,attrName);
        }
        /// <summary>
        /// 获取用户自定义表字段声明
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static CustomTColDef getCustomTColDef(DataSourceMHub datasource, string attr)
        {
            return CustomTColDefDAM.getCustomTColDef(datasource.WSM, attr);
        }
        /// <summary>
        /// 更新预览字段位序值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="viewOrder"></param>
        public static void updateViewOrder(DataSourceMHub datasource,string attr, int viewOrder, bool isRequired=true)
        {
            DataSupporter.updateViewOrder(datasource.WSM, attr, viewOrder, isRequired);
        }
        /// <summary>
        /// 查询第一层节点集合
        /// </summary>
        /// <returns></returns>
        public static List<CatalogNode> findParentNodes(DataSourceMHub datasource)
        {
            return CatalogNodeDAM.findParentNodes(datasource.WSM);
        }
        /// <summary>
        /// 保存新节点记录
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int saveLibraryNode(DataSourceMHub datasource,CatalogNode instance)
        {
            return CatalogNodeDAM.saveLibraryNode(datasource.WSM,instance);
        }
        /// <summary>
        /// 查询具有指定父节点ID编号的孩子节点集合
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static List<CatalogNode> findSubNodes(DataSourceMHub datasource, long pid)
        {
            return CatalogNodeDAM.findSubNodes(datasource.WSM, pid);
        }
        /// <summary>
        /// 级联删除目标节点以及其直接子节点操作
        /// </summary>
        /// <param name="referId"></param>
        /// <returns></returns>
        public static int delNodeCascaded(DataSourceMHub datasource, long referId)
        {
            return CatalogNodeDAM.delNodeCascaded(datasource.WSM, referId);
        }
        /// <summary>
        /// 保存新节点记录,包含同级后续节点位序值后移操作。
        /// @Note 请注意和saveLibraryNode区别使用，本方法主要用于特定节点插入情形，而批量归档节点插入模式。
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static int insertLibraryNode(DataSourceMHub datasource, CatalogNode instance)
        {
            return CatalogNodeDAM.insertLibraryNode(datasource.WSM, instance);
        }
        /// <summary>
        /// 查询具有目标主键值的节点记录
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public static CatalogNode findLibraryNode(DataSourceMHub datasource, long lid)
        {
            return CatalogNodeDAM.findLibraryNode(datasource.WSM, lid);
        }
        /// <summary>
        /// 更新特征条件下的记录的某个属性值
        /// </summary>
        /// <param name="filterCond"></param>
        /// <param name="setName"></param>
        /// <param name="setVal"></param>
        /// <returns></returns>
        public static int updateCatalogNode(DataSourceMHub datasource, string lids, string setName, object setVal)
        {
            return CatalogNodeDAM.updateCatalogNode(datasource.WSM, lids, setName, setVal);
        }
        /// <summary>
        /// 获取可检索分类目录映射索引对象
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> getSearchMap(DataSourceMHub datasource)
        {
            return CatalogNodeDAM.getSearchMap(datasource.WSM);
        }
        public static bool doUpdateProcess(DataSourceMHub datasource, LinkedList<CustomTColDef> newCtcds)
        {
            return TemplateUpdater.doUpdateProcess(datasource.WSM, newCtcds);
        }
    }
}
