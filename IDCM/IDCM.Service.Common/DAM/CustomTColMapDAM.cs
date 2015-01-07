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
                    noteCmds.Add(ctcd.Attr, "insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + ctcd.Attr + "',0,-1)");
                    continue;
                }
                else if (ctcd.Attr.Equals(CTDRecordA.CTD_PLID) || ctcd.Attr.Equals(CTDRecordA.CTD_LID))
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
            DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
            ColumnMappingHolder.noteDefaultColMap(wsm,noteCmds.Values.ToList<string>());
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
            int viewOrder = (ctcd.IsRequire ? ctcd.Corder : ColumnMappingHolder.MaxMainViewCount + ctcd.Corder);
            DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
            ColumnMappingHolder.noteDefaultColMap(wsm,ctcd.Attr, ctcd.Corder, viewOrder);
        }
        public static int noteDefaultColMap(WorkSpaceManager wsm,string attr, int dbOrder, int viewOrder)
        {
            string cmd = "Replace or insert into " + typeof(CustomTColMap).Name + "(attr,mapOrder,viewOrder) values('" + attr + "'," + dbOrder + "," + viewOrder + ")";
            return DataSupporter.executeSQL(wsm, cmd);
        }
        public static int clearColMap(WorkSpaceManager wsm)
        {
            string cmd = "delete from " + typeof(CustomTColMap).Name + "";
            return DataSupporter.executeSQL(wsm, cmd);
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
            ColumnMappingHolder.clearColMap(wsm);
            cmd = "Alter Table " + typeof(CustomTColDef).Name + " Rename to " + typeof(CustomTColDef).Name + suffix;
            DataSupporter.executeSQL(wsm, cmd);
            return suffix;
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        internal static class ColumnMappingHolder
        {
            //public static ColumnMapping getBaseMapping()
            //{
            //    return attrMapping.Clone() as ColumnMapping;
            //}
            /// <summary>
            /// 存储表属性映射位序条件语句
            /// </summary>
            public static void noteDefaultColMap(WorkSpaceManager wsm, List<string> noteCmds)
            {
                DataSupporter.executeSQL(wsm, noteCmds.ToArray());
                queryCacheAttrDBMap(wsm);
            }
            public static void noteDefaultColMap(WorkSpaceManager wsm, string attr, int dbOrder, int viewOrder)
            {
                CustomTColMapDAM.noteDefaultColMap(wsm,attr, dbOrder, viewOrder);
                queryCacheAttrDBMap(wsm);
            }
            public static void clearColMap(WorkSpaceManager wsm)
            {
                CustomTColMapDAM.clearColMap(wsm);
                attrMapping.Clear();
            }
            /// <summary>
            /// 缓存数据字段映射关联关系
            /// </summary>
            public static void queryCacheAttrDBMap(WorkSpaceManager wsm)
            {
                //select * from CustomTColMap order by viewOrder
                List<CustomTColMap> ctcms = CustomTColMapDAM.findAllByOrder(wsm);
                lock (attrMapping)
                {
                    attrMapping.Clear();
                    foreach (CustomTColMap dr in ctcms)
                    {
                        attrMapping[dr.Attr]= new ObjectPair<int, int>(dr.MapOrder, dr.ViewOrder);
                    }
                }
            }
            /// <summary>
            /// 获取视图和数据库查询映射
            /// </summary>
            /// <returns></returns>
            public static Dictionary<string, int> getViewDBMapping(WorkSpaceManager wsm)
            {
                Dictionary<string, int> maps = new Dictionary<string, int>();
                if (attrMapping.Count < 1)
                    queryCacheAttrDBMap(wsm);
                foreach (KeyValuePair<String, ObjectPair<int, int>> kvpair in attrMapping)
                {
                    maps[kvpair.Key] = kvpair.Value.Val;
                }
                return maps;
            }
            /// <summary>
            /// 获取已经被缓存的用户浏览字段~数据库字段位序的映射关系。
            /// @author JiahaiWu
            /// 字段名对于数据库存储名,亦即包装过的表单列名。
            /// 数据库字段映射位序的值自0计数。
            /// </summary>
            /// <returns></returns>
            public static Dictionary<string, int> getCustomViewDBMapping(WorkSpaceManager wsm)
            {
                Dictionary<string, int> maps = ColumnMappingHolder.getViewDBMapping(wsm);
                //填写表头
                List<string> excludes = new List<string>();
                foreach (string attr in maps.Keys)
                {
                    if (!CVNameConverter.isViewWrapName(attr))
                    {
                        excludes.Add(CVNameConverter.toViewName(attr));
                    }
                }
                foreach (string attr in excludes)
                {
                    maps.Remove(attr);
                }
                return maps;
            }
            /// <summary>
            /// 获取存储字段序列值(如查找失败返回-1)
            /// </summary>
            /// <param name="attr"></param>
            /// <returns></returns>
            public static int getDBOrder(WorkSpaceManager wsm, string attr)
            {
                if (attrMapping.Count < 1)
                    queryCacheAttrDBMap(wsm);
                ObjectPair<int, int> kvpair = null;
                attrMapping.TryGetValue(attr, out kvpair);
                return kvpair == null ? -1 : kvpair.Key;
            }

            /// <summary>
            /// 获取预览字段集序列
            /// </summary>
            /// <returns></returns>
            public static List<string> getViewAttrs(WorkSpaceManager wsm, bool withInnerField = true)
            {
                if (attrMapping.Count < 1)
                    //作用是在attrMapping里存入，key属性名称，value<key value> key:数据库映射，字段显示
                    queryCacheAttrDBMap(wsm);
                if (withInnerField)
                    return attrMapping.Keys.ToList<string>();//第一次进来没参数，所以返回key的集合(属性名称)
                else
                {
                    List<string> res = new List<string>();
                    foreach (string key in attrMapping.Keys)
                    {
                        if (CVNameConverter.isViewWrapName(key))//查看是否以[ 开头，以] 结尾
                            res.Add(key);
                    }
                    return res;
                }
            }
            /// <summary>
            /// 获取预览字段位序值(如查找失败返回-1)
            /// </summary>
            /// <param name="attr"></param>
            /// <returns></returns>
            public static int getViewOrder(WorkSpaceManager wsm, string attr)
            {
                if (attrMapping.Count < 1)
                    queryCacheAttrDBMap(wsm);
                ObjectPair<int, int> kvpair = null;
                attrMapping.TryGetValue(attr, out kvpair);
                return kvpair == null ? -1 : kvpair.Val;
            }
            /// <summary>
            /// 更新预览字段位序值
            /// </summary>
            /// <param name="attr"></param>
            /// <param name="viewOrder"></param>
            public static void updateViewOrder(WorkSpaceManager wsm, string attr, int viewOrder, bool isRequired)
            {
                int vOrder = viewOrder;
                if (isRequired == false && viewOrder < MaxMainViewCount)
                    vOrder = viewOrder + MaxMainViewCount;
                else if (viewOrder > MaxMainViewCount)
                    vOrder = viewOrder - MaxMainViewCount;
                updateViewOrder(wsm,attr, vOrder);
            }
            public static void updateViewOrder(WorkSpaceManager wsm, string attr, int viewOrder)
            {
                int ic = CustomTColMapDAM.updateViewOrder(wsm,attr, viewOrder);
                if (ic > 0)
                    attrMapping[attr] = new ObjectPair<int, int>(attrMapping[attr].Key, viewOrder);
            }
            /// <summary>
            /// 数据字段名与[数据存储，预览界面]的映射关系
            /// </summary>
            internal static ColumnMapping attrMapping = new ColumnMapping();
            /// <summary>
            /// 主表域最大显示字段数
            /// </summary>
            public const int MaxMainViewCount = 1000;
        }
    }
}
