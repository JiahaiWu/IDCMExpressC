using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data
{
    public abstract class WorkSpaceManagerA
    {
        public WorkSpaceManagerA(string connectStr)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(connectStr!=null && connectStr.Length>0,"connectStr should not be null or empty char sequence.");
#endif
            this.ConnectStr = connectStr;
        }
        public virtual bool connect()
        {
            return false;
        }
        public virtual bool prepare()
        {
            return false;
        }
        public virtual bool disconnect()
        {
            return false;
        }
        public virtual WSStatus getStatus()
        {
            return WSStatus.Idle;
        }
        public virtual dynamic[] SQLQuery(params string[] sqlExpressions)
        {
            return null;
        }
        public virtual int[] executeSQL(params string[] commands)
        {
            return null;
        }

        public readonly string ConnectStr;
    }
}
