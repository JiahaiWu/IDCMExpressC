using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Core;
using IDCM.Data.DAM;
using IDCM.Data.Common;
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
            System.Diagnostics.Debug.Assert(wsm!=null && wsm.getStatus().Equals(WSStatus.InWorking),"Ivalid params and status for get next sequence id.");
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
            System.Diagnostics.Debug.Assert(wsm.getStatus().Equals(WSStatus.InWorking), "illegal status to Query Data! @getStatus()=" + wsm.getStatus());
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
    }
}
