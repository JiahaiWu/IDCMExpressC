using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace IDCM.Data.DHCP
{
    public class SQLiteConn
    {
        public SQLiteConn(SQLiteConnectionStringBuilder sqlCSB)
        {
            connectStr = sqlCSB.ToString();
        }
        public SQLiteConn(string connectionString)
        {
            connectStr = connectionString;
        }
        internal readonly string connectStr = null;
    }
}
