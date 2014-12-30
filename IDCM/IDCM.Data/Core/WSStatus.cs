using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Core
{
    /// <summary>
    /// 数据文档的用户工作空间WorkSpace的上层运营状态类型
    /// </summary>
    public class WSStatus
    {
        /// <summary>
        /// 空闲状态中
        /// </summary>
        public static readonly WSStatus Idle = new WSStatus("Idle", 0);
        /// <summary>
        /// 已建立数据文档的连接操作
        /// </summary>
        public static readonly WSStatus Connected = new WSStatus("Connected", 1);
        /// <summary>
        /// 正在准备运行时上下文数据环境
        /// </summary>
        public static readonly WSStatus Preparing = new WSStatus("Preparing", 2);
        /// <summary>
        /// 数据源连接保持中，正常工作状态
        /// </summary>
        public static readonly WSStatus InWorking = new WSStatus("InWorking", 3);
        /// <summary>
        /// 未能有效修复的异常状态
        /// </summary>
        public static readonly WSStatus FATAL = new WSStatus("FATAL", 4);

        /// <summary>
        /// For iterator 
        /// </summary>
        public static IEnumerable<WSStatus> Values
        {
            get
            {
                yield return Idle;
                yield return Connected;
                yield return Preparing;
                yield return InWorking;
                yield return FATAL;
            }
        }

        private readonly string name;
        private readonly double value;

        WSStatus(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name { get { return name; } }

        public double Value { get { return value; } }

        public override string ToString()
        {
            return name;
        }
    }
}
