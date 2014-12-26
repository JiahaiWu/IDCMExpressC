using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data
{
    public class WorkSpaceManager : WorkSpaceManagerI
    {
        public static bool connect()
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(_status.Equals(WSStatus.Idle), "illegal status of work space to connect! @getStatus()="+getStatus());
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
        public static dynamic[] SQLQuery(params string[] sqlExpression)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(sqlExpression != null, "sqlExpression should not be null.");
#endif
            using (SQLiteConnPicker picker = new SQLiteConnPicker(WorkSpaceHolder.ConnectStr))
            {
                picker.getConnection().Execute(strBuilder.ToString());
            }
        }
        public static dynamic[] executeSQL(params string[] command)
        {
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                lastAuthInfo = picker.getConnection().Query<AuthInfo>(cmd).FirstOrDefault<AuthInfo>();
            }
        }

        #region 实例对象保持部分
        /// <summary>
        /// 用户工作空间运营状态标识
        /// </summary>
        private volatile static WSStatus _status = WSStatus.Idle;
        #endregion
    }
}
