using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Service.Common.Core.ServBuf
{
    /// <summary>
    /// 异步消息类型及附属参数的封装类
    /// </summary>
    public class AsyncMessage
    {
        public static readonly AsyncMessage DataPrepared = new AsyncMessage(MsgType.DataPrepared, "Data Prepared");

        /// <summary>
        /// For iterator 
        /// </summary>
        public static IEnumerable<AsyncMessage> Values
        {
            get
            {
                yield return DataPrepared;
            }
        }
        private readonly string msgTag;
        private readonly MsgType msgType;
        private readonly object[] parameters;

        AsyncMessage(MsgType msgType, string msgTag, object[] parameters = null)
        {
            this.msgTag = msgTag;
            this.msgType = msgType;
            this.parameters = parameters;
        }

        public string MsgTag { get { return msgTag; } }

        public MsgType MsgType { get { return msgType; } }

        public object[] Parameters { get { return parameters; } }

        public override string ToString()
        {
            return msgType+":"+msgTag;
        }
    }
    /// <summary>
    /// 预定义的消息类型
    /// </summary>
    public class MsgType
    {
        public static readonly MsgType DataPrepared = new MsgType("DataPrepared", 0);

        /// <summary>
        /// For iterator 
        /// </summary>
        public static IEnumerable<MsgType> Values
        {
            get
            {
                yield return DataPrepared;
            }
        }
        private readonly string name;
        private readonly int value;

        MsgType(string name,int value)
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
