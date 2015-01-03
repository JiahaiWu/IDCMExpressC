using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Service.Common.Core.ServBuf
{
    public class AsyncMessage
    {
        public static readonly AsyncMessage Prepared = new AsyncMessage("Prepared", MsgType.DataLoading);

        /// <summary>
        /// For iterator 
        /// </summary>
        public static IEnumerable<AsyncMessage> Values
        {
            get
            {
                yield return Prepared;
            }
        }
        private readonly string message;
        private readonly MsgType msgType;
        private readonly object[] parameters;

        AsyncMessage(string message, MsgType msgType, object[] parameters = null)
        {
            this.message = message;
            this.msgType = msgType;
            this.parameters = parameters;
        }

        public string Message { get { return message; } }

        public MsgType MsgType { get { return msgType; } }
        public object[] Parameters { get { return parameters; } }

        public override string ToString()
        {
            return message + "/" + msgType;
        }
    }
    public class MsgType
    {
        public static readonly MsgType DataLoading = new MsgType("DataLoading",0);
        public static readonly MsgType FormActiving = new MsgType("FormActiving", 0);

        /// <summary>
        /// For iterator 
        /// </summary>
        public static IEnumerable<MsgType> Values
        {
            get
            {
                yield return DataLoading;
                yield return FormActiving;
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
