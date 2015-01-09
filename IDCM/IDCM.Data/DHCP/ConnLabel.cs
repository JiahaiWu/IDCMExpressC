using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace IDCM.Data.DHCP
{
    public class ConnLabel
    {
        public ConnLabel(SQLiteConnectionStringBuilder sqlCSB)
        {
            connectStr = sqlCSB.ToString();
        }
        public ConnLabel(string connectionString)
        {
            connectStr = connectionString;
        }
        internal readonly string connectStr = null;
    }
}
