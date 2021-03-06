﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Common;
using IDCM.Data.Base;
using IDCM.Data.DAM;
using Dapper;
using IDCM.Data.DHCP;
using IDCM.Data.Base.Utils;

namespace IDCM.Data.Core
{
    /// <summary>
    /// 本类定义用于支持动态数据表的存储字段与显示字段之间的映射关系的动态缓存实现
    /// 说明：
    /// 1.基于ColumnMapping对象实例，记录各个数据字段名与[数据存储，预览界面]的映射关系
    /// 2.由用户自定义的动态数据表在前端预览的界面中，分为首选字段和备选字段两个部分,字段显示顺序要有稳定性。
    /// 3.首选字段的显示序号小于CustomTColMap.MaxMainViewCount时视为首选字段，大于CustomTColMap.MaxMainViewCount时视为备选字段。
    /// 4.动态数据表的数据存储的字段顺序相对固定，但字段名称区别于用户显示的数据字段名，用户自定义的数据字段名均有[前缀和]后缀。
    /// 5.对于无有[前缀和]后缀的数据字段一律视为内置的字段项，由创建程序内嵌生成与内部管理维护。
    /// 6.本类缓存实现旨在为用户提供虚拟字段与动态显示顺序设定特性下的快速定位数据库存储记录的字段表示的映射对照。
    /// </summary>
    class ColumnMappingHolder
    {
        /// <summary>
        /// 检视基本动态表单数据，依赖数据项载入请求方法
        /// 说明：
        /// 1.载入基础数据项，如自增长序号定位
        /// 2.确认动态定义的数据表CTDRecord有效生成状态
        /// 3.本方法通常用于初始化启动过程，可作为自动构建关系表的第一步检视过程。
        /// </summary>
        /// <param name="picker"></param>
        /// <returns>依赖数据项载入成功与否状态</returns>
        public static bool prepareForLoad(ConnLabel sconn)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(sconn != null);
#endif
            BaseInfoNoteDAM.loadBaseInfo(sconn);
            return checkTableSetting(sconn);
        }
        /// <summary>
        /// 检查数据表配置存在与否，如不存在则创建默认表属性设定
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        private static bool checkTableSetting(ConnLabel sconn)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(sconn != null);
#endif
            string cmd = "SELECT count(*) FROM " + typeof(CustomTColDef).Name;
            long ctcdCount = 0;
            using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
            {
                ctcdCount = picker.getConnection().Query<long>(cmd).Single<long>();
            }
            if (ctcdCount > 0)
            {
                queryCacheAttrDBMap(sconn);
                return true;
            }
            else if (CustomTColDefDAM.buildDefaultSetting(sconn))
            {
                using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
                {
                    ctcdCount = picker.getConnection().Query<long>(cmd).Single<long>();
                }
                if (ctcdCount > 0)
                {
                    CustomTColMapDAM.buildCustomTable(sconn);
                    queryCacheAttrDBMap(sconn);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 缓存数据字段映射关联关系
        /// 说明：
        /// 1.该方法用于存储及刷新CTDRecord相关的字段映射关系缓存
        /// </summary>
        internal static void queryCacheAttrDBMap(ConnLabel sconn)
        {
            List<CustomTColMap> ctcms = CustomTColMapDAM.findAllByOrder(sconn);
            attrMapping.Clear();
            foreach (CustomTColMap dr in ctcms)
            {
                ObjectPair<int, int> mapPair = new ObjectPair<int, int>(dr.MapOrder, dr.ViewOrder);
                attrMapping[dr.Attr]= mapPair;
            }
        }
        /// <summary>
        /// 获取已经被缓存的数据存储字段~数据库字段位序的映射关系。
        /// 说明：
        /// 1.本方法返回实际的[数据存储字段名称，数据存储字段位序]的映射关系。
        /// 2.数据库字段映射位序的值自0计数。
        /// 3.如果外部存在批量的字段映射匹配需要，需外部缓冲，重复请求该方法会重复创建对象资源。
        /// 4.返回的字典主键顺序以用户界面中字段列加载顺序为参照。
        /// </summary>
        /// <param name="sconn"></param>
        /// <returns></returns>
        public static Dictionary<string, int> getAttrDBMapping(ConnLabel sconn)
        {
            Dictionary<string, int> maps = new Dictionary<string, int>();
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap(sconn);
            foreach (KeyValuePair<String, ObjectPair<int, int>> kvpair in attrMapping)
            {
                maps[kvpair.Key] = kvpair.Value.Key;
            }
            return maps;
        }
        /// <summary>
        /// 获取已经被缓存的用户浏览字段~数据库字段位序的映射关系。
        /// 说明：
        /// 1.本方法返回可见的[用户浏览字段名，数据存储字段位序]的映射关系。
        /// 2.数据库字段映射位序的值自0计数。
        /// 3.如果外部存在批量的字段映射匹配需要，需外部缓冲，重复请求该方法会重复创建对象资源。
        /// 4.返回的字典主键顺序以用户界面中字段列加载顺序为参照。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> getCustomAttrDBMapping(ConnLabel sconn)
        {
            Dictionary<string, int> maps = ColumnMappingHolder.getAttrDBMapping(sconn);
            Dictionary<string, int> resmaps = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> mapEntry in maps)
            {
                if (CVNameConverter.isViewWrapName(mapEntry.Key))
                {
                    string key = CVNameConverter.toViewName(mapEntry.Key);
                    resmaps[key] = mapEntry.Value;
                }
            }
            return resmaps;
        }
        /// <summary>
        /// 获取已经被缓存的数据存储字段~预览界面位序的映射关系。
        /// 说明：
        /// 1.本方法返回实际的[数据存储字段名称，预览界面位序]的映射关系。
        /// 2.数据库字段映射位序的值自0计数。
        /// 3.如果外部存在批量的字段映射匹配需要，需外部缓冲，重复请求该方法会重复创建对象资源。
        /// 4.返回的字典主键顺序以用户界面中字段列加载顺序为参照。
        /// </summary>
        /// <param name="sconn"></param>
        /// <returns></returns>
        public static Dictionary<string, int> getAttrViewMapping(ConnLabel sconn)
        {
            Dictionary<string, int> maps = new Dictionary<string, int>();
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap(sconn);
            foreach (KeyValuePair<String, ObjectPair<int, int>> kvpair in attrMapping)
            {
                maps[kvpair.Key] = kvpair.Value.Val;
            }
            return maps;
        }
        /// <summary>
        /// 获取已经被缓存的用户浏览字段~预览界面位序的映射关系。
        /// 说明：
        /// 1.本方法返回可见的[用户浏览字段名，预览界面位序]的映射关系。
        /// 2.数据库字段映射位序的值自0计数。
        /// 3.如果外部存在批量的字段映射匹配需要，需外部缓冲，重复请求该方法会重复创建对象资源。
        /// 4.返回的字典主键顺序以用户界面中字段列加载顺序为参照。
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> getCustomAttrViewMapping(ConnLabel sconn)
        {
            Dictionary<string, int> maps = ColumnMappingHolder.getAttrDBMapping(sconn);
            Dictionary<string, int> resmaps = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> mapEntry in maps)
            {
                if (CVNameConverter.isViewWrapName(mapEntry.Key))
                {
                    string key = CVNameConverter.toViewName(mapEntry.Key);
                    resmaps[key] = mapEntry.Value;
                }
            }
            return resmaps;
        }

        /// <summary>
        /// 获取预览字段集序列
        /// 说明:
        /// 1.如果外部存在批量的字段映射匹配需要，需外部缓冲，重复请求该方法会重复创建对象资源。
        /// </summary>
        /// <returns></returns>
        public static List<string> getViewAttrs(ConnLabel sconn, bool withInnerField = true)
        {
            if (attrMapping.Count < 1)
                //作用是在attrMapping里存入，key属性名称，value<key value> key:数据库映射，字段显示
                queryCacheAttrDBMap(sconn);
            if (withInnerField)
                return attrMapping.Keys.ToList<string>();//第一次进来没参数，所以返回key的集合(属性名称)
            else
            {
                List<string> res = new List<string>();
                foreach (string key in attrMapping.Keys)
                {
                    if (CVNameConverter.isViewWrapName(key))//查看是否以[ 开头并以] 结尾
                        res.Add(key);
                }
                return res;
            }
        }
        /// <summary>
        /// 获取预览字段位序值(如查找失败返回-1)
        /// 说明：
        /// 1.如果外部存在批量的字段映射匹配需要，首选getAttrViewMapping或getCustomAttrViewMapping方法进行外部缓冲。
        /// </summary>
        /// <param name="attr">数据存储字段名称</param>
        /// <returns></returns>
        public static int getViewOrder(ConnLabel sconn, string attr)
        {
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap(sconn);
            ObjectPair<int, int> kvpair = null;
            attrMapping.TryGetValue(CVNameConverter.toDBName(attr), out kvpair);
            return kvpair == null ? -1 : kvpair.Val;
        }
        /// <summary>
        /// 获取存储字段序列值(如查找失败返回-1)
        /// 说明：
        /// 1.如果外部存在批量的字段映射匹配需要，首选getAttrDBMapping或getCustomAttrDBMapping方法进行外部缓冲。
        /// </summary>
        /// <param name="attr">数据存储字段名称</param>
        /// <returns></returns>
        public static int getDBOrder(ConnLabel sconn, string attr,bool autoWrap=true)
        {
            if (attrMapping.Count < 1)
                queryCacheAttrDBMap(sconn);
            ObjectPair<int, int> kvpair = null;
            if(autoWrap==true)
                attrMapping.TryGetValue(CVNameConverter.toDBName(attr), out kvpair);
            else
                attrMapping.TryGetValue(attr, out kvpair);
            return kvpair == null ? -1 : kvpair.Key;
        }

        /// <summary>
        /// 更新预览字段位序值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="viewOrder"></param>
        public static void updateViewOrder(ConnLabel sconn, string attr, int viewOrder, bool isRequired)
        {
            int vOrder = viewOrder;
            if (isRequired == false && viewOrder < CustomTColMap.MaxMainViewCount)
                vOrder = viewOrder + CustomTColMap.MaxMainViewCount;
            else if (viewOrder > CustomTColMap.MaxMainViewCount)
                vOrder = viewOrder - CustomTColMap.MaxMainViewCount;
            updateViewOrder(sconn, attr, vOrder);
        }
        /// <summary>
        /// 更新预览字段位序值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="viewOrder"></param>
        public static void updateViewOrder(ConnLabel sconn, string attr, int viewOrder)
        {
            int ic = CustomTColMapDAM.updateViewOrder(sconn, attr, viewOrder);
            if (ic > 0)
                attrMapping[attr] = new ObjectPair<int, int>(attrMapping[attr].Key, viewOrder);
        }
        public static void noteDefaultColMap(ConnLabel sconn, string attr, int dbOrder, int viewOrder)
        {
            CustomTColMapDAM.noteDefaultColMap(sconn, attr, dbOrder, viewOrder);
            queryCacheAttrDBMap(sconn);
        }
        public static void noteDefaultColMap(ConnLabel sconn, string[] noteCmds)
        {
            CustomTColMapDAM.noteDefaultColMap(sconn, noteCmds);
            queryCacheAttrDBMap(sconn);
        }
        public static void clearColMap(ConnLabel sconn)
        {
            CustomTColMapDAM.clearColMap(sconn);
            attrMapping.Clear();
        }
        /// <summary>
        /// 数据存储的字段名与[数据存储位序，预览界面位序]的双层映射关系存储集合对象
        /// </summary>
        protected static ColumnMapping attrMapping = new ColumnMapping();

    }
}
