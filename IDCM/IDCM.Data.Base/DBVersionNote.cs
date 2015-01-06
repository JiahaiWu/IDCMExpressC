using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base
{
    /// <summary>
    /// 用户标记当前数据库记录版本标识类定义
    /// 该类实例应当唯一确定，不接受运行时变更标识信息。
    /// </summary>
    public class DBVersionNote
    {
        private string dbType = "SQLite";

        public string DbType
        {
            get { return dbType; }
            set { }
        }
        private long startNo = 1;

        public long StartNo
        {
            get { return startNo; }
            set { }
        }

        private string appType = "IDCMExpress";

        public string AppType
        {
            get { return appType; }
            set { }
        }
        private double appVercode = 0.1;

        public double AppVercode
        {
            get { return appVercode; }
            set { }
        }
    }
}
