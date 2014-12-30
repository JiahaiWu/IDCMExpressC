using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Core;
using IDCM.Data.DAM;
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
            return BaseInfoNoteDAM.nextSeqID(wsm.getConnectPicker());
        }
        //////////////////////////////////////////////////////////////////////////////////////
        ///// <summary>
        ///// 执行SQL非查询命令，返回查询结果集
        ///// 说明：
        ///// 1.可重入，可并入。
        ///// </summary>
        ///// <param name="wsm">工作空间管理器对象实例</param>
        ///// <param name="sqlExpressions"></param>
        ///// <returns></returns>
        //public override dynamic[] SQLQuery(WorkSpaceManager wsm,params string[] sqlExpressions)
        //{
        //    return wsm.SQLQuery(sqlExpressions);
        //}
        ///// <summary>
        ///// 执行SQL查询命令，返回查询结果集
        ///// 说明：
        ///// 1.可重入，可并入。
        ///// </summary>
        ///// <param name="wsm">工作空间管理器对象实例</param>
        ///// <param name="commands"></param>
        ///// <returns></returns>
        //public override int[] executeSQL(WorkSpaceManager wsm,params string[] commands)
        //{
        //    return wsm.executeSQL(commands);
        //}
        //////////////////////////////////////////////////////////////////////////////////////
        //保留未用
    }
}
