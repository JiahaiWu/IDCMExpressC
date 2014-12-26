using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.POO;
using System.IO;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using Dapper;

namespace IDCM.Data.Base
{
    class DAMBase
    {
        public static int rebuildCustomTColDef()
        {
            string cmd = @"drop table if exists " + typeof(CustomTColDef).Name + ";";
            cmd += "Create Table if Not Exists " + typeof(CustomTColDef).Name + "("
                + "Attr TEXT primary key,"
                + "AttrType TEXT default " + "string" + ","
                + "Comments TEXT,"
                + "Restrict TEXT,"
                + "IsUnique TEXT default '" + false.ToString() + "',"
                + "IsRequire TEXT default '" + false.ToString() + "',"
                + "DefaultVal TEXT default NULL,"
                + "Corder INTEGER default 0,"
                + "IsInter TEXT default '" + false.ToString() + "');";
            using (SQLiteConnPicker picker = ConnectPicker())
            {
                int res = picker.getConnection().Execute(cmd);
                return res;
            }
        }
        /// <summary>
        /// 启动数据库实例,返回数据库连接串，如失效则返回null
        /// </summary>
        /// <param name="dbFilePath"></param>
        /// <returns></returns>
        public static string startDBInstance()
        {
            string dbFilePath = IDCMEnvironment.CURRENT_WORKSPACE + "\\" + IDCMEnvironment.LUID;
            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
            try
            {
                SQLiteConnectionStringBuilder sqlCSB = new SQLiteConnectionStringBuilder();
                sqlCSB.DataSource = dbFilePath;
                sqlCSB.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
                connectStr = sqlCSB.ToString();
                using (SQLiteConnPicker picker = ConnectPicker())
                {
                    picker.getConnection().Execute("PRAGMA synchronous = OFF;");
                    picker.getConnection().Execute(getBasicTableCmds());
                    return connectStr;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + " \r\n[StackTrace]=" + ex.StackTrace);
                return null;
            }
        }
        /// <summary>
        /// 关闭数据库实例,返回数据库连接串，如失效则返回null
        /// </summary>
        /// <returns></returns>
        public static void stopDBInstance()
        {
            SQLiteConnPicker.closeAll();
            ////SQLiteConnection.ClearAllPools();
            ////SQLiteConnection.Shutdown(true, true);
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

        protected static SQLiteConnPicker ConnectPicker()
        {
            return new SQLiteConnPicker(connectStr);
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string connectStr;

        internal static string ConnectStr
        {
            get { return connectStr; }
        }
    }
}
