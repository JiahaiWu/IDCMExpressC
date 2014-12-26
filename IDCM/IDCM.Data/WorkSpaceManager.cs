using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using Dapper;

namespace IDCM.Data
{
    public class WorkSpaceManager : WorkSpaceManagerI
    {
        public static bool connect()
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.Idle), "illegal status to connect DataSource! @getStatus()="+getStatus());
#endif
            //checkWorkSpaceSingleton
            //startDBInstance      
        }
        public static bool prepare()
        {
            //startInstance
            //verifyForLoad
        }
        public static bool disconnect()
        {
            //关闭用户工作空间
        }
        public static WSStatus getStatus()
        {
            return _status;
        }
        /// <summary>
        /// 执行SQL非查询命令，返回查询结果集
        /// </summary>
        /// <param name="sqlExpressions"></param>
        /// <returns></returns>
        public static dynamic[] SQLQuery(params string[] sqlExpressions)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.InWorking), "illegal status to Query Data! @getStatus()=" + getStatus());
            System.Diagnostics.Debug.Assert(sqlExpressions != null, "sqlExpressions should not be null.");
#endif
            List<dynamic> res = new List<dynamic>(sqlExpressions.Count());
            foreach (string sql in sqlExpressions)
            {
                using (SQLiteConnPicker picker = new SQLiteConnPicker(WorkSpaceHolder.ConnectStr))
                {
                    using (SQLiteTransaction transaction = picker.getConnection().BeginTransaction())
                    {
                        dynamic result = picker.getConnection().Query(sql);
                        res.Add(result);
                    }
                }
            }
            return res.ToArray();
        }
        /// <summary>
        /// 执行SQL查询命令，返回查询结果集
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public static int[] executeSQL(params string[] commands)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.InWorking), "illegal status to Execute SQL commands! @getStatus()=" + getStatus());
            System.Diagnostics.Debug.Assert(commands != null, "commands should not be null.");
#endif
            List<int> res = new List<int>(commands.Count());
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnPicker picker = new SQLiteConnPicker(WorkSpaceHolder.ConnectStr))
            {
                using (SQLiteTransaction transaction = picker.getConnection().BeginTransaction())
                {
                    foreach (string execmd in commands)
                    {
                        int result = picker.getConnection().Execute(execmd);
                        res.Add(result);
                    }
                    transaction.Commit();
                }
            }
            return res.ToArray();
        }

        #region 实例对象保持部分
        /// <summary>
        /// 用户工作空间运营状态标识
        /// </summary>
        private volatile static WSStatus _status = WSStatus.Idle;
        #endregion
    }
}
