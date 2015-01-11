using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using CSharpTest.Net.Collections;

namespace IDCM.Service.Common.Core
{
    /// <summary>
    /// 特征查询条件缓存类
    /// @author JiahaiWu
    /// </summary>
    class QueryCmdCache
    {
        /// <summary>
        /// 数据表单查询条件语句缓存
        /// </summary>
        /// <param name="cmdstr"></param>
        internal static void cacheDGVQuery(string cmdstr, int tcount)
        {
            int idx = cmdstr.IndexOf("Limit", 0, StringComparison.OrdinalIgnoreCase);
            lastUserDGVRQuery = idx > 0 ? cmdstr.Substring(0, idx) : cmdstr;
            lastUserDGVRQueryCount = tcount;
        }
        /// <summary>
        /// 最近的用户发起的数据表单查询条件语句
        /// </summary>
        /// <param name="cmdstr"></param>
        /// <returns></returns>
        public static KeyValuePair<string, int> getLastDGVRQuery()
        {
            return new KeyValuePair<string, int>(lastUserDGVRQuery, lastUserDGVRQueryCount);
        }
        /// <summary>
        /// 聚合查询数值结果缓存
        /// </summary>
        /// <param name="cmdstr"></param>
        /// <param name="values"></param>
        internal static void cacheAggregateQuery(string cmdstr, params long[] values)
        {
            aggregateQueryStack.AddOrUpdate(cmdstr, values, (key, oldValue) => values);
        }
        /// <summary>
        /// 获取聚合查询数值结果
        /// </summary>
        /// <param name="cmdstr"></param>
        /// <returns></returns>
        public static long[] getAggregateValues(string cmdstr)
        {
            long[] vals = null;
            aggregateQueryStack.TryGetValue(cmdstr, out vals);
            return vals;
        }
        /// <summary>
        /// 最近的用户发起的数据表单查询条件语句
        /// </summary>
        public static string lastUserDGVRQuery = null;

        public static int lastUserDGVRQueryCount = 0;
        /// <summary>
        /// 聚合查询数值结果缓存池大小限定
        /// </summary>
        public static int MaxAggregateQueryStackSize = 256;
        /// <summary>
        /// 最近的聚合查询数值结果缓存池，目前仅支持先进先出缓存原则。
        /// </summary>
        public static LurchTable<string, long[]> aggregateQueryStack = new LurchTable<string, long[]>(LurchTableOrder.Insertion, MaxAggregateQueryStackSize);
    }
}
