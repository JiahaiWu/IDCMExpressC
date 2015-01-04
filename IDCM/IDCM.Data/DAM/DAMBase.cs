using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.POO;
using System.IO;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using Dapper;
using System.Threading;
using IDCM.Data.Base;
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
        public static string startDBInstance(string dbFilePath,string password)
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
                string connectStr = sqlCSB.ToString();
                if (!File.Exists(dbFilePath))
                {
                    SQLiteConnection.CreateFile(dbFilePath);
                    using (SQLiteConnPicker picker = new SQLiteConnPicker(connectStr))
                    {
                        picker.getConnection().Execute("PRAGMA application_id = " + IDCM_DATA_BIND_Code + ";"); //Refer:http://www.sqlite.org/src/artifact?ci=trunk&filename=magic.txt
                    }
                }
                using (SQLiteConnPicker picker = new SQLiteConnPicker(connectStr))
                {
                    int bindcode=picker.getConnection().Execute("PRAGMA application_id");
                    if (bindcode == IDCM_DATA_BIND_Code)
                    {
                        picker.getConnection().Execute("PRAGMA synchronous = OFF;"); //启用异步存储模式
                        //picker.getConnection().Execute("PRAGMA case_sensitive_like = 0;"); //设置Like查询大小写敏感与否设置
                        //picker.getConnection().Execute("PRAGMA auto_vacuum = INCREMENTAL;");  //设置索引增量存储模式
                        return connectStr;
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
        public static void stopDBInstance(SQLiteConnPicker picker)
        {
            picker.shutdown();
        }

       /// <summary>
        /// 结构化的静态数据表单初始化定义
        /// 说明：
        /// 1.可重入
        /// </summary>
        /// <returns>初始化操作完成与否</returns>
        public static bool prepareTables(SQLiteConnPicker picker)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(picker != null);
#endif 
            int rescode = -1;
            using (picker)
            {
                using (SQLiteTransaction transaction = picker.getConnection().BeginTransaction())
                {
                    rescode=picker.getConnection().Execute(getBasicTableCmds());
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
        public static dynamic[] SQLQuery(SQLiteConnPicker picker,params string[] sqlExpressions)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(picker != null);
            System.Diagnostics.Debug.Assert(sqlExpressions != null);
#endif 
            List<dynamic> res = new List<dynamic>(sqlExpressions.Count());
            foreach (string sql in sqlExpressions)
            {
                using (picker)
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
        /// <param name="picker"></param>
        /// <param name="commands"></param>
        /// <returns></returns>
        public static int[] executeSQL(SQLiteConnPicker picker,params string[] commands)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(picker != null);
            System.Diagnostics.Debug.Assert(commands != null);
#endif 
            List<int> res = new List<int>(commands.Count());
            SQLiteCommand cmd = new SQLiteCommand();
            using (picker)
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

        #region 基础数据库表定义
        protected static string getBasicTableCmds()
        {
            StringBuilder strbuilder = new StringBuilder();
            //创建目录库数据表定义
            string cmd = "Create Table if Not Exists " + typeof(LibraryNode).Name + "("
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
        /// IDCM.Data数据库文档特征标识码设定
        /// </summary>
        private const UInt32 IDCM_DATA_BIND_Code = 1415926535;
    }
}
