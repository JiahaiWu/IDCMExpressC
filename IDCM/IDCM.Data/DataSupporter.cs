using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Core;
using IDCM.Data.DAM;
using IDCM.Data.Common;
using IDCM.Data.Base;
using System.Data;
/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.Data
{
    /// <summary>
    /// 数据源存储管理的应用支持类定义
    /// @author JiahaiWu 2014-12-27
    /// </summary>
    public class DataSupporter
    {
        /// <summary>
        /// 获取唯一序列生成ID值
        /// 1.可重入，可并入。
        /// 注意：
        /// 1.该序号的生成在同一数据库内部，由独占进程请求该方法时，保证生成序号值全局唯一。
        /// 2.该序号生成值会有规律地进行数据库同步写入操作，在进程重启后需调用loadBaseInfo更新目标生成序列起始值。
        /// </summary>
        /// <param name="wsm">工作空间管理器对象实例</param>
        /// <returns>新序列值</returns>
        public static long nextSeqID(WorkSpaceManager wsm)
        {
#if DEBUG
            bool res = WSStatus.InWorking.Equals(wsm.getStatus());
            System.Diagnostics.Debug.Assert(wsm != null && WSStatus.InWorking.Equals(wsm.getStatus()), "Ivalid params and status for get next sequence id.");
#endif
            return BaseInfoNoteDAM.nextSeqID(wsm.getConnection());
        }
        /// <summary>
        /// 执行SQL查询命令，返回查询结果集
        /// 说明：
        /// 1.可重入，可并入。
        /// </summary>
        /// <param name="wsm">工作空间管理器对象实例</param>
        /// <param name="sqlExpressions"></param>
        /// <returns></returns>
        public static IEnumerable<T>[] SQLQuery<T>(WorkSpaceManager wsm, params string[] sqlExpressions)
        {
            IEnumerable<T>[] res= wsm.SQLQuery<T>(sqlExpressions);
            return res;
        }
        /// <summary>
        /// 执行SQL查询命令，返回查询结果
        /// 说明：
        /// 1.可重入，可并入。
        /// </summary>
        /// <param name="wsm">工作空间管理器对象实例</param>
        /// <param name="sqlExpression"></param>
        /// <returns></returns>
        public static IEnumerable<T> SQLQuery<T>(WorkSpaceManager wsm, string sqlExpression)
        {
            IEnumerable<T>[] res = wsm.SQLQuery<T>(sqlExpression);
            if (res != null && res.Length > 0)
                return res[0];
            return null;
        }        
        /// <summary>
        /// 执行SQL查询命令，返回查询结果
        /// 说明：
        /// 1.可重入，可并入。
        /// </summary>
        /// <param name="wsm">工作空间管理器对象实例</param>
        /// <param name="sqlExpression"></param>
        /// <returns></returns>
        public static DataTable SQLQuery(WorkSpaceManager wsm, string sqlExpression)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(WSStatus.InWorking.Equals(wsm.getStatus()), "illegal status to Query Data! @getStatus()=" + wsm.getStatus());
            System.Diagnostics.Debug.Assert(sqlExpression != null, "sqlExpressions should not be null.");
#endif
            try
            {
                return DAMBase.DataTableSQLQuery(wsm.getConnection(), sqlExpression);
            }
            catch (Exception ex)
            {
                wsm.setLastError(new ErrorNote(ex));
            }
            return null;
        }
        /// <summary>
        /// 执行SQL查询命令，返回查询结果
        /// 说明：
        /// 1.可重入，可并入。
        /// </summary>
        /// <param name="wsm">工作空间管理器对象实例</param>
        /// <param name="sqlExpression"></param>
        /// <returns></returns>
        public static List<T> ListSQLQuery<T>(WorkSpaceManager wsm, string sqlExpression)
        {
            IEnumerable<T>[] res = wsm.SQLQuery<T>(sqlExpression);
            if (res != null && res.Length > 0)
                return res[0].ToList();
            return null;
        }
        /// <summary>
        /// 执行SQL查询命令，返回查询结果
        /// 说明：
        /// 1.可重入，可并入。
        /// </summary>
        /// <param name="wsm">工作空间管理器对象实例</param>
        /// <param name="sqlExpression"></param>
        /// <returns></returns>
        public static long CountSQLQuery(WorkSpaceManager wsm, string sqlExpression)
        {
            IEnumerable<long>[] res = wsm.SQLQuery<long>(sqlExpression);
            if (res != null && res.Length > 0)
                return res[0].FirstOrDefault();
            return 0;
        }

        /// <summary>
        /// 执行SQL非查询命令，返回执行结果
        /// 说明：
        /// 1.可重入，可并入。
        /// </summary>
        /// <param name="wsm">工作空间管理器对象实例</param>
        /// <param name="commands"></param>
        /// <returns>Array of Number of rows affected</returns>
        public static int[] executeSQL(WorkSpaceManager wsm, params string[] commands)
        {
            return wsm.executeSQL(commands);
        }
        /// <summary>
        /// 执行SQL非查询命令，返回执行结果
        /// 说明：
        /// 1.可重入，可并入。
        /// </summary>
        /// <param name="wsm">工作空间管理器对象实例</param>
        /// <param name="command"></param>
        /// <returns>Number of rows affected</returns>
        public static int executeSQL(WorkSpaceManager wsm, string command)
        {
            int[] res= wsm.executeSQL(command);
            if (res != null && res.Length>0)
                return res[0];
            return -2;
        }

        /// <summary>
        /// 获取存储字段序列值(如查找失败返回-1)
        /// 说明：
        /// 1.如果外部存在批量的字段映射匹配需要，首选getAttrDBMapping或getCustomAttrDBMapping方法进行外部缓冲。
        /// </summary>
        /// <param name="attr">数据存储字段名称</param>
        /// <returns></returns>
        public static int getDBOrder(WorkSpaceManager wsm, string attr, bool autoWrap = true)
        {
            return ColumnMappingHolder.getDBOrder(wsm.getConnection(), attr,autoWrap);
        }
        /// <summary>
        /// 获取预览字段位序值(如查找失败返回-1)
        /// 说明：
        /// 1.如果外部存在批量的字段映射匹配需要，首选getAttrViewMapping或getCustomAttrViewMapping方法进行外部缓冲。
        /// </summary>
        /// <param name="attr">数据存储字段名称</param>
        /// <returns></returns>
        public static int getViewOrder(WorkSpaceManager wsm, string attr)
        {
            return ColumnMappingHolder.getViewOrder(wsm.getConnection(), attr);
        }
        /// <summary>
        /// 更新预览字段位序值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="viewOrder"></param>
        public static void updateViewOrder(WorkSpaceManager wsm, string attr, int viewOrder, bool isRequired)
        {
            ColumnMappingHolder.updateViewOrder(wsm.getConnection(), attr, viewOrder, isRequired);
        }

        public static void noteDefaultColMap(WorkSpaceManager wsm, string attr, int dbOrder, int viewOrder)
        {
            ColumnMappingHolder.noteDefaultColMap(wsm.getConnection(), attr, dbOrder, viewOrder);
        }
        public static void noteDefaultColMap(WorkSpaceManager wsm,  List<string> noteCmds)
        {
            ColumnMappingHolder.noteDefaultColMap(wsm.getConnection(), noteCmds.ToArray());
        }

        public static void clearColMap(WorkSpaceManager wsm)
        {
            ColumnMappingHolder.clearColMap(wsm.getConnection());
        }
        /// <summary>
        /// 缓存数据字段映射关联关系
        /// </summary>
        public static void queryCacheAttrDBMap(WorkSpaceManager wsm)
        {
            ColumnMappingHolder.queryCacheAttrDBMap(wsm.getConnection());
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
        public static Dictionary<string, int> getCustomAttrDBMapping(WorkSpaceManager wsm)
        {
            return ColumnMappingHolder.getCustomAttrDBMapping(wsm.getConnection());
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
        public static Dictionary<string, int> getCustomAttrViewMapping(WorkSpaceManager wsm)
        {
            return ColumnMappingHolder.getCustomAttrViewMapping(wsm.getConnection());
        }

        /// <summary>
        /// 获取预览字段集序列
        /// </summary>
        /// <returns></returns>
        public static List<string> getViewAttrs(WorkSpaceManager wsm, bool withInnerField = true)
        {
            return ColumnMappingHolder.getViewAttrs(wsm.getConnection(), withInnerField);
        }
        /// <summary>
        /// 重写用户自定义数据表的字段集定义
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="ctcds"></param>
        /// <returns></returns>
        public static bool overwriteAllCustomTColDef(WorkSpaceManager wsm, List<CustomTColDef> ctcds)
        {
            return CustomTColDefDAM.overwriteAllCustomTColDef(wsm.getConnection(), ctcds);
        }

        /// <summary>
        /// 读取并创建默认的动态表单定义
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        public static bool buildDefaultSetting(WorkSpaceManager wsm)
        {
            return CustomTColDefDAM.buildDefaultSetting(wsm.getConnection());
        }
    }
}
