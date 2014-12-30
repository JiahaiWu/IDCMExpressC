using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using Dapper;
using IDCM.Data.Core;
using IDCM.Data.Common;
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
    /// 基于特定数据存储引擎的工作空间管理器的具体实现类
    /// @author JiahaiWu 2014-12-26
    /// </summary>
    public class WorkSpaceManager : WorkSpaceManagerA
    {
        /// <summary>
        /// 工作空间管理器构造方法，要求指定标准格式的连接句柄
        /// </summary>
        /// <param name="dbPath">数据存档文件路径</param>
        /// <param name="password">数据存档加密字符串</param>
        public WorkSpaceManager(string dbPath, string password = null): base(dbPath, password)
        {
        }
        /// <summary>
        /// 请求数据源连接操作
        /// </summary>
        /// <returns>连接成功与否状态</returns>
        public override bool connect()
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.Idle), "illegal status to connect DataSource! @getStatus()="+getStatus());
#endif
            try
            {
                if (WorkSpaceHelper.isWorkSpaceAccessible(DBPath))
                {
                    if (WorkSpaceHelper.isProcessDuplicate())
                    {
                        _connectStr = DAMBase.startDBInstance(DBPath, password);
                        if (_connectStr != null)
                        {
                            _status = WSStatus.Connected;
                            return true;
                        }
                    }
                    else
                    {
                        _lastError = new ErrorNote(typeof(WorkSpaceManager), "There is already another process instance with the same location and name, is should not be started again.");
                    }
                }
                else
                {
                    _lastError = new ErrorNote(typeof(WorkSpaceManager), "Database path is not valid or cannot be exclusively locked. @DBPath=" + DBPath);
                }
            }
            catch (Exception ex)
            {
                _lastError=new ErrorNote(ex);
            }
            return false;    
        }
        /// <summary>
        /// 预备启动前准备请求，依赖数据项载入请求方法
        /// 说明：
        /// 1.主要包含数据源初始结构化及结构完整性校验
        /// </summary>
        /// <returns>预备启动成功与否状态</returns>
        public override bool prepare()
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.Connected), "illegal status to connect DataSource! @getStatus()=" + getStatus());
#endif
            _status = WSStatus.Preparing;
            if (DAMBase.prepareTables(picker)) ////定义静态表结构
            {
                if (ColumnMappingHolder.prepareForLoad(picker)) //检视基本动态表单数据
                {
                    _status = WSStatus.InWorking;
                    return true;
                }
                else
                {
                    _lastError = new ErrorNote(typeof(WorkSpaceManager), "The target data source load failed at ColumnMappingHolder.verifyForLoad()!");
                }
            }
            else
            {
                _lastError = new ErrorNote(typeof(WorkSpaceManager), "Prepare Tables for database init failed!");
            }
            _status = WSStatus.FATAL;
            return false;
        }
        /// <summary>
        /// 断开数据库连接，释放访问连接池资源占用。
        /// 说明：
        /// 1.可重入，可并入。
        /// 2.断开数据库连接后，任何后续的数据访问请求都必须重新建立。
        /// </summary>
        /// <returns>断开连接成功与否</returns>
        public override bool disconnect()
        {
            //关闭用户工作空间
            if (!_status.Equals(WSStatus.Idle))
            {
                picker.shutdown();
                _status = WSStatus.Idle;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 执行SQL非查询命令，返回查询结果集
        /// 说明：
        /// 1.可重入，可并入。
        /// </summary>
        /// <param name="sqlExpressions"></param>
        /// <returns></returns>
        public override dynamic[] SQLQuery(params string[] sqlExpressions)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.InWorking), "illegal status to Query Data! @getStatus()=" + getStatus());
            System.Diagnostics.Debug.Assert(sqlExpressions != null, "sqlExpressions should not be null.");
#endif
            try
            {
                return DAMBase.SQLQuery(picker, sqlExpressions);
            }
            catch (Exception ex)
            {
                _lastError = new ErrorNote(ex);
            }
            return null;
        }
        /// <summary>
        /// 执行SQL查询命令，返回查询结果集
        /// 说明：
        /// 1.可重入，可并入。
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public override int[] executeSQL(params string[] commands)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.InWorking), "illegal status to Execute SQL commands! @getStatus()=" + getStatus());
            System.Diagnostics.Debug.Assert(commands != null, "commands should not be null.");
#endif
            try
            {
                return DAMBase.executeSQL(picker, commands);
            }
            catch (Exception ex)
            {
                _lastError = new ErrorNote(ex);
            }
            return null;
        }
       
    }
}
