using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Common;
using IDCM.Data.Base;
using IDCM.Data.DAM;
using Dapper;
using IDCM.Data.DHCP;

namespace IDCM.Data.Core
{
    class ColumnMappingHolder
    {
        /// <summary>
        /// 检视基本动态表单数据，依赖数据项载入请求方法
        /// 说明：
        /// 1.载入基础数据项，如自增长序号定位
        /// 2.确认动态定义的数据表CTDRecord有效生成状态
        /// </summary>
        /// <param name="picker"></param>
        /// <returns>依赖数据项载入成功与否状态</returns>
        public static bool prepareForLoad(SQLiteConnPicker picker)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(picker != null);
#endif
            BaseInfoNoteDAM.loadBaseInfo(picker);
            return checkTableSetting(picker);
        }
        /// <summary>
        /// 检查数据表配置存在与否，如不存在则创建默认表属性设定
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        public static bool checkTableSetting(SQLiteConnPicker picker)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(picker != null);
#endif
            string cmd = "SELECT count(*) FROM " + typeof(CustomTColDef).Name;
            using (picker)
            {
                long ctcdCount = picker.getConnection().Query<long>(cmd).Single<long>();
                if (ctcdCount > 0)
                {
                    queryCacheAttrDBMap(picker);
                    return true;
                }
                else if (CustomTColDefDAM.buildDefaultSetting(picker))
                {
                    ctcdCount = picker.getConnection().Query<long>(cmd).Single<long>();
                    if (ctcdCount > 0)
                    {
                        CustomTColMapDAM.buildCustomTable(picker);
                        queryCacheAttrDBMap(picker);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 缓存数据字段映射关联关系
        /// </summary>
        public static void queryCacheAttrDBMap(SQLiteConnPicker picker)
        {
            //select * from CustomTColMap order by viewOrder
            List<CustomTColMap> ctcms = CustomTColMapDAM.findAllByOrder(picker);
            attrMapping.Clear();
            foreach (CustomTColMap dr in ctcms)
            {
                attrMapping[dr.Attr]= new ObjectPair<int, int>(dr.MapOrder, dr.ViewOrder);
            }
        }
        /// <summary>
        /// 获取视图和数据库查询映射
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> getViewDBMapping(SQLiteConnPicker picker)
        {
            Dictionary<string, int> maps = new Dictionary<string, int>();
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap(picker);
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
        public static Dictionary<string, int> getCustomViewDBMapping(SQLiteConnPicker picker)
        {
            Dictionary<string, int> maps = ColumnMappingHolder.getViewDBMapping(picker);
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
        public static int getDBOrder(SQLiteConnPicker picker, string attr)
        {
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap(picker);
            ObjectPair<int, int> kvpair = null;
            attrMapping.TryGetValue(attr, out kvpair);
            return kvpair == null ? -1 : kvpair.Key;
        }

        /// <summary>
        /// 获取预览字段集序列
        /// </summary>
        /// <returns></returns>
        public static List<string> getViewAttrs(SQLiteConnPicker picker,bool withInnerField = true)
        {
            if (attrMapping.Count < 1)
                //作用是在attrMapping里存入，key属性名称，value<key value> key:数据库映射，字段显示
                queryCacheAttrDBMap(picker);
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
        public static int getViewOrder(SQLiteConnPicker picker,string attr)
        {
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap(picker);
            ObjectPair<int, int> kvpair = null;
            attrMapping.TryGetValue(attr, out kvpair);
            return kvpair == null ? -1 : kvpair.Val;
        }
        /// <summary>
        /// 更新预览字段位序值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="viewOrder"></param>
        public static void updateViewOrder(SQLiteConnPicker picker, string attr, int viewOrder, bool isRequired)
        {
            int vOrder = viewOrder;
            if (isRequired == false && viewOrder < CustomTColMapDAM.MaxMainViewCount)
                vOrder = viewOrder + CustomTColMapDAM.MaxMainViewCount;
            else if (viewOrder > CustomTColMapDAM.MaxMainViewCount)
                vOrder = viewOrder - CustomTColMapDAM.MaxMainViewCount;
            updateViewOrder(picker,attr, vOrder);
        }
        /// <summary>
        /// 更新预览字段位序值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="viewOrder"></param>
        public static void updateViewOrder(SQLiteConnPicker picker, string attr, int viewOrder)
        {
            int ic = CustomTColMapDAM.updateViewOrder(picker,attr, viewOrder);
            if (ic > 0)
                attrMapping[attr] = new ObjectPair<int, int>(attrMapping[attr].Key, viewOrder);
        }
        /// <summary>
        /// 数据字段名与[数据存储，预览界面]的映射关系
        /// </summary>
        protected static ColumnMapping attrMapping = new ColumnMapping();

    }
}
