using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data
{
    public interface WorkSpaceManagerI
    {
        public static bool connect();
        public static bool prepare();
        public static bool disconnect();
        public static WSStatus getStatus();
        public static dynamic SQLQuery(string sqlExpression);
        public static dynamic executeSQL(string command);
    }
}
