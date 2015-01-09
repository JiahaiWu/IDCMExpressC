using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using System.IO;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using Dapper;
using System.Data;
using System.Threading;
using IDCM.Data.DHCP;

namespace IDCM.Data.DAM
{
    class DAMBase
    {
        /// <summary>
        /// 启动数据库实例,返回数据库连接串，如失效则返回null.
        /// 异常:
        /// System.Data.Exception  文件创建或访问异常
        /// System.Data.SQLite.SQLiteException  文件创建或访问异常
        /// </summary>
        /// <param name="dbFilePath"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static SQLiteConn startDBInstance(string dbFilePath, string password)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(dbFilePath != null);
            System.Diagnostics.Debug.Assert(password != null);
#endif 
            try
            {
                SQLiteConnectionStringBuilder sqlCSB = new SQLiteConnectionStringBuilder();
                sqlCSB.DataSource = dbFilePath;
                sqlCSB.Password = password;//设置密码
                sqlCSB.SyncMode = SynchronizationModes.Off;//启用异步存储模式
                sqlCSB.Pooling = true;
                sqlCSB.DefaultTimeout = 5000;
                //sqlCSB.Pooling = true;
                ////////////////////////////////////////////////////////////////
                sqlCSB.Add("PRAGMA application_id", IDCM_DATA_BIND_Code);
                //sqlCSB.Add("PRAGMA case_sensitive_like", 0);//设置Like查询大小写敏感与否设置
                //sqlCSB.Add("PRAGMA auto_vacuum", "INCREMENTAL");//设置Like查询大小写敏感与否设置
                //sqlCSB.ToFullPath = true;
                ////////////////////////////////////////////////////////////////
                //有关PRAGMA的参数设置及读取未能有效执行，有待解决的问题 @Date 2015-01-08
                ///////////////////////////////////////////////////////////////
                SQLiteConn sconn = new SQLiteConn(sqlCSB);
                if (!File.Exists(dbFilePath))
                {
                    SQLiteConnection.CreateFile(dbFilePath);
                    sqlCSB.Add("PRAGMA application_id", IDCM_DATA_BIND_Code);//设置Like查询大小写敏感与否设置
                    sconn = new SQLiteConn(sqlCSB);
                }
                using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
                {
                    SQLiteConnPicker.getConnection(picker).Execute("PRAGMA application_id(" + IDCM_DATA_BIND_Code + ");");
                    int bindcode = SQLiteConnPicker.getConnection(picker).ExecuteScalar<int>("PRAGMA application_id");
                    if (bindcode == IDCM_DATA_BIND_Code)
                    {
                        return sconn;
                    }
                    else
                    {
                        throw new System.Data.DataException("Invalid database file for IDCM.Data Module!");
                    }
                }
            }
            catch (SQLiteException ex)
            {
                log.Error("Error in IDCM.Data.startDBInstance(): " + ex.Message,ex);
                throw ex;
            }
        }
        /// <summary>
        /// 关闭目标数据库，停止所有未完成的连接过程
        /// 说明：
        /// 1.Passes a shutdown request to the SQLite core library. Does not throw an exception if the shutdown request fails.
        /// </summary>
        /// <returns></returns>
        public static void stopDBInstance(SQLiteConn sconn)
        {
            SQLiteConnPicker.shutdown(sconn);
        }

       /// <summary>
        /// 结构化的静态数据表单初始化定义
        /// 说明：
        /// 1.可重入
        /// </summary>
        /// <returns>初始化操作完成与否</returns>
        public static bool prepareTables(SQLiteConn sconn)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(sconn != null);
#endif 
            int rescode = -1;
            using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
            {
                using (SQLiteTransaction transaction = SQLiteConnPicker.getConnection(picker).BeginTransaction())
                {
                    rescode = SQLiteConnPicker.getConnection(picker).Execute(getBasicTableCmds());
                }
            }
            return rescode > -1;
        }
        /// <summary>
        /// 执行SQL非查询命令，返回查询结果集
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="sqlExpressions"></param>
        /// <returns></returns>
        public static IEnumerable<T>[] SQLQuery<T>(SQLiteConn sconn, params string[] sqlExpressions)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(sconn != null);
            System.Diagnostics.Debug.Assert(sqlExpressions != null);
#endif 
            List<IEnumerable<T>> res = new List<IEnumerable<T>>(sqlExpressions.Count());
            foreach (string sql in sqlExpressions)
            {
                using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
                {
                    ////////////////////////////////////////////////
                    //using (SQLiteTransaction transaction = picker.getConnection().BeginTransaction())
                    ///////////////////////////////////////////////
                    //For Query default without Transaction
                    {
                        IEnumerable<T> result = SQLiteConnPicker.getConnection(picker).Query<T>(sql);
                        res.Add(result);
                    }
                }
            }
            return res.ToArray();
        }
        /// <summary>
        /// 执行SQL查询命令，返回查询结果集
        /// </summary>
        /// <param name="picker">连接句柄</param>
        /// <param name="commands"></param>
        /// <returns></returns>
        public static int[] executeSQL(SQLiteConn sconn, params string[] commands)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(sconn != null);
            System.Diagnostics.Debug.Assert(commands != null);
#endif 
            List<int> res = new List<int>(commands.Count());
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
            {
                using (SQLiteTransaction transaction = SQLiteConnPicker.getConnection(picker).BeginTransaction())
                {
                    foreach (string execmd in commands)
                    {
                        int result = SQLiteConnPicker.getConnection(picker).Execute(execmd);
                        res.Add(result);
                    }
                    transaction.Commit();
                }
            }
            return res.ToArray();
        }
        #region ExecuteDataTable
        /// <summary>
        /// 执行数据库查询，返回DataTable对象
        /// </summary>
        /// <param name="picker">连接句柄</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>DataTable对象</returns>
        public static DataTable DataTableSQLQuery(SQLiteConn sconn, string commandText, CommandType commandType = CommandType.Text)
        {
            return DataTableSQLQuery(sconn, commandText, commandType, null);
        }

        /// <summary>
        /// 执行数据库查询，返回DataTable对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>DataTable对象</returns>
        public static DataTable DataTableSQLQuery(SQLiteConn sconn, string commandText, CommandType commandType, params SQLiteParameter[] cmdParms)
        {
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataSet ds = new DataSet();
            using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
            {
                ////////////////////////////////////////////////
                //using (SQLiteTransaction transaction = picker.getConnection().BeginTransaction())
                ///////////////////////////////////////////////
                //For Query default without Transaction
                {
                    SQLiteConnection conn = SQLiteConnPicker.getConnection(picker);
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;
                    if (cmdParms != null)
                    {
                        foreach (SQLiteParameter parm in cmdParms)
                            cmd.Parameters.Add(parm);
                    }
                    try
                    {
#if DEBUG
                        log.Debug("DataTableSQLQuery Info: @CommandText=" + cmd.CommandText);
#endif
                        SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                        sda.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                        log.Error("DataTableSQLQuery Error.", ex);
                    }
                }
            }
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        #endregion
        #region 基础数据库表定义
        protected static string getBasicTableCmds()
        {
            StringBuilder strbuilder = new StringBuilder();
            //创建目录库数据表定义
            string cmd = "Create Table if Not Exists " + typeof(CatalogNode).Name + "("
                + "Lid INTEGER primary key,"
                + "Name TEXT,"
                + "Type TEXT,"
                + "Desc TEXT,"
                + "Lorder TEXT,"
                + "Pid INTEGER default -1,"
                + "UpdateTime TEXT);";
            strbuilder.Append(cmd).Append("\n");
            DBVersionNote dbvn = new DBVersionNote();
            //创建基础自增长序列及版本记录数据表
            cmd = "Create Table if Not Exists BaseInfoNote("
            + "SeqId INTEGER DEFAULT " + dbvn.StartNo + ","
            + "DbType Text default '" + dbvn.DbType + "', "
            + "AppType Text default '" + dbvn.AppType + "',"
            + "AppVercode Real default " + dbvn.AppVercode + ");";
            strbuilder.Append(cmd).Append("\n");
            //创建CustomTColDef数据表定义
            cmd = "Create Table if Not Exists " + typeof(CustomTColDef).Name + "("
                + "Attr TEXT primary key,"
                + "AttrType TEXT default " + "string" + ","
                + "Comments TEXT,"
                + "Restrict TEXT,"
                + "IsUnique TEXT default '" + false.ToString() + "',"
                + "IsRequire TEXT default '" + false.ToString() + "',"
                + "DefaultVal TEXT default NULL,"
                + "Corder INTEGER default 0,"
                + "IsInter TEXT default '" + false.ToString() + "');";
            strbuilder.Append(cmd).Append("\n");
            //创建CustomTColMap数据表定义
            cmd = "Create Table if Not Exists " + typeof(CustomTColMap).Name + "("
                + "Attr TEXT primary key,"
                + "MapOrder INTEGER default 0,"
                + "ViewOrder INTEGER default 0);";
            strbuilder.Append(cmd).Append("\n");
            //创建内置事务存档数据表记录
            cmd = "Create Table if Not Exists " + typeof(DelayWorkNote).Name + "("
                + "Nid INTEGER primary key,"
                + "JobType TEXT not null,"
                + "JobSerialInfo TEXT,"
                + "JobLevel INTEGER default 1,"
                + "CreateTime INTEGER,"
                + "StartCount INTEGER default 0,"
                + "LastResult TEXT);";
            strbuilder.Append(cmd).Append("\n");
            //创建用户身份认证信息
            cmd = "Create Table if Not Exists " + typeof(AuthInfo).Name + "("
                + "username primary key,"
                + "password TEXT,"
                + "jsessionid TEXT,"
                + "loginFlag INTEGER default 0,"
                + "autoLogin INTEGER default 0,"
                + "timestamp INTEGER default 0);";
            strbuilder.Append(cmd).Append("\n");
            return strbuilder.ToString();
        }
        #endregion

        

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// IDCM.Data数据库文档特征标识码设定(设定为32bit长度)
        /// </summary>
        private const UInt32 IDCM_DATA_BIND_Code = 1415926535;
    }
}
