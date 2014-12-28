using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using IDCM.Data.Base;
using System.IO;
using Dapper;

namespace IDCM.Data
{
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
        public bool connect()
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
        /// 数据源初始结构化及结构完整性校验
        /// </summary>
        /// <returns></returns>
        public bool prepare()
        {
            if (DAMBase.prepareTables(picker)) ////定义静态表结构
            {
                if (ColumnMappingHolder.prepareForLoad(picker)) //检视基本动态表单数据
                {
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
            return false;
        }

        public bool disconnect()
        {
            //关闭用户工作空间
            WorkSpaceHolder.close();
        }

        /// <summary>
        /// 执行SQL非查询命令，返回查询结果集
        /// </summary>
        /// <param name="sqlExpressions"></param>
        /// <returns></returns>
        public dynamic[] SQLQuery(params string[] sqlExpressions)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.InWorking), "illegal status to Query Data! @getStatus()=" + getStatus());
            System.Diagnostics.Debug.Assert(sqlExpressions != null, "sqlExpressions should not be null.");
#endif
            try
            {
                return DAMBase.SQLQuery(_connectStr, sqlExpressions);
            }
            catch (Exception ex)
            {
                _lastError = new ErrorNote(ex);
            }
            return null;
        }
        /// <summary>
        /// 执行SQL查询命令，返回查询结果集
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public int[] executeSQL(params string[] commands)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.InWorking), "illegal status to Execute SQL commands! @getStatus()=" + getStatus());
            System.Diagnostics.Debug.Assert(commands != null, "commands should not be null.");
#endif
            try
            {
                return DAMBase.executeSQL(_connectStr, commands);
            }
            catch (Exception ex)
            {
                _lastError = new ErrorNote(ex);
            }
            return null;
        }
       
    }
}
