using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using Dapper;
using IDCM.Data.DHCP;

namespace IDCM.Data.DAM
{
    class BaseInfoNoteDAM
    {
        /// <summary>
        /// 获取唯一序列生成ID值
        /// 1.可重入，可并入。
        /// 注意：
        /// 1.该序号的生成在同一数据库内部，由独占进程请求该方法时，保证生成序号值全局唯一。
        /// 2.该序号生成值会有规律地进行数据库同步写入操作，在进程重启后需调用loadBaseInfo更新目标生成序列起始值。
        /// </summary>
        /// <param name="picker">数据库连接选择器实例</param>
        /// <returns>新序列值</returns>
        public static long nextSeqID(SQLiteConn sconn)
        {
            lock (IncrementKey_Lock)
            {
                ++autoIncrementNum;
                if (autoIncrementNum % 10 == 0)//如果是10的整数
                {
                    string cmd = "update BaseInfoNote set seqId=" + autoIncrementNum;//更新BaseInfoNote seqId
                    using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
                    {
                        SQLiteConnPicker.getConnection(picker).Execute(cmd);
                    }
                }
            }
            return autoIncrementNum;
        }
        /// <summary>
        /// 获取用于自增长的新的基础序号值
        /// 说明：
        /// 1.可重入，可并入。
        /// 参考：
        /// 1. BaseInfoNoteDAM.nextSeqID(...)
        /// </summary>
        public static void loadBaseInfo(SQLiteConn sconn)
        {
            using (SQLiteConnPicker picker = new SQLiteConnPicker(sconn))
            {
                List<long> seqIds = SQLiteConnPicker.getConnection(picker).Query<long>("SELECT seqId FROM BaseInfoNote").ToList<long>();
                if (seqIds.Count == 0)
                {
                    DBVersionNote dbvn = new DBVersionNote();
                    string icmd = "insert into BaseInfoNote(seqId) values(" + dbvn.StartNo + ");";
                    SQLiteConnPicker.getConnection(picker).Execute(icmd);
                    autoIncrementNum = dbvn.StartNo;
                }
                else
                {
                    autoIncrementNum = seqIds[0] + 10;
                    string cmd = "update BaseInfoNote set seqId=" + autoIncrementNum;
                    SQLiteConnPicker.getConnection(picker).Execute(cmd);
                }
            }
        }

        /// <summary>
        /// 自动增长ID计数值
        /// </summary>
        private static long autoIncrementNum = 0;
        /// <summary>
        /// 用于全局数据序号生成自增长对象锁
        /// </summary>
        public static object IncrementKey_Lock = new object();
    }
}
