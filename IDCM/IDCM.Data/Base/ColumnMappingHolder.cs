using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Common;
using IDCM.Data.POO;

namespace IDCM.Data.Base
{
    class ColumnMappingHolder
    {
        /// <summary>
        /// 检视基本动态表单数据
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        public static bool prepareForLoad(SQLiteConnPicker picker)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(picker != null);
#endif
            loadbaseinfo();
            checkTableSetting();
        }
        /// <summary>
        /// 存储表属性映射位序条件语句
        /// </summary>
        public static void noteDefaultColMap(List<string> noteCmds)
        {
            try
            {
                SQLiteHelper.ExecuteNonQuery(DAMBase.ConnectStr, CommandType.Text, noteCmds.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:\r\n" + ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            queryCacheAttrDBMap();
        }
        public static void noteDefaultColMap(string attr, int dbOrder, int viewOrder)
        {
            CustomTColMapDAM.noteDefaultColMap(attr, dbOrder, viewOrder);
            queryCacheAttrDBMap();
        }
        public static void clearColMap()
        {
            CustomTColMapDAM.clearColMap();
            attrMapping.Clear();
        }
        /// <summary>
        /// 缓存数据字段映射关联关系
        /// </summary>
        public static void queryCacheAttrDBMap()
        {
            //select * from CustomTColMap order by viewOrder
            List<CustomTColMap> ctcms = CustomTColMapDAM.findAllByOrder();
            lock (attrMapping)
            {
                attrMapping.Clear();
                foreach (CustomTColMap dr in ctcms)
                {
                    attrMapping.Add(dr.Attr, new ObjectPair<int, int>(dr.MapOrder, dr.ViewOrder));
                }
            }
        }
        /// <summary>
        /// 获取视图和数据库查询映射
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> getViewDBMapping()
        {
            Dictionary<string, int> maps = new Dictionary<string, int>();
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap();
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
        public static Dictionary<string, int> getCustomViewDBMapping()
        {
            Dictionary<string, int> maps = ColumnMappingHolder.getViewDBMapping();
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
        public static int getDBOrder(string attr)
        {
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap();
            ObjectPair<int, int> kvpair = null;
            attrMapping.TryGetValue(attr, out kvpair);
            return kvpair == null ? -1 : kvpair.Key;
        }

        /// <summary>
        /// 获取预览字段集序列
        /// </summary>
        /// <returns></returns>
        public static List<string> getViewAttrs(bool withInnerField = true)
        {
            if (attrMapping.Count < 1)
                //作用是在attrMapping里存入，key属性名称，value<key value> key:数据库映射，字段显示
                queryCacheAttrDBMap();
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
        public static int getViewOrder(string attr)
        {
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap();
            ObjectPair<int, int> kvpair = null;
            attrMapping.TryGetValue(attr, out kvpair);
            return kvpair == null ? -1 : kvpair.Val;
        }
        /// <summary>
        /// 更新预览字段位序值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="viewOrder"></param>
        public static void updateViewOrder(string attr, int viewOrder, bool isRequired)
        {
            int vOrder = viewOrder;
            if (isRequired == false && viewOrder < MaxMainViewCount)
                vOrder = viewOrder + MaxMainViewCount;
            else if (viewOrder > MaxMainViewCount)
                vOrder = viewOrder - MaxMainViewCount;
            updateViewOrder(attr, vOrder);
        }
        /// <summary>
        /// 更新预览字段位序值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="viewOrder"></param>
        public static void updateViewOrder(string attr, int viewOrder)
        {
            int ic = CustomTColMapDAM.updateViewOrder(attr, viewOrder);
            if (ic > 0)
                attrMapping[attr] = new ObjectPair<int, int>(attrMapping[attr].Key, viewOrder);
        }
        /// <summary>
        /// 数据字段名与[数据存储，预览界面]的映射关系
        /// </summary>
        protected static ColumnMapping attrMapping = new ColumnMapping();
        /// <summary>
        /// 主表域最大显示字段数
        /// </summary>
        public const int MaxMainViewCount = 1000;
    }
}
