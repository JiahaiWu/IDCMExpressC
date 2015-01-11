using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IDCM.Data.Base
{
    [Serializable]
    public class IDCMServException:IDCMException,ISerializable
    {
        // 摘要:
        //     初始化 IDCM.Data.Base.IDCMServException 类的新实例。
        protected IDCMServException()
            : base()
        {
        }
        //
        // 摘要:
        //     使用指定的错误消息初始化 IDCM.Data.Base.IDCMServException 类的新实例。
        //
        // 参数:
        //   message:
        //     为此异常显示的消息。
        public IDCMServException(string message):base(message)
        {
        }
        //
        // 摘要:
        //     用指定的序列化信息和上下文初始化 IDCM.Data.Base.IDCMServException 类的新实例。
        //
        // 参数:
        //   info:
        //     System.Runtime.Serialization.SerializationInfo，它存有有关所引发异常的序列化的对象数据。
        //
        //   context:
        //     System.Runtime.Serialization.StreamingContext，它包含有关源或目标的上下文信息。
        public IDCMServException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        //
        // 摘要:
        //     使用指定的错误消息和对导致此异常的内部异常的引用初始化 IDCM.Data.Base.IDCMServException 类的新实例。
        //
        // 参数:
        //   message:
        //     错误消息字符串。
        //
        //   innerException:
        //     内部异常引用。
        public IDCMServException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        //
        // 摘要:
        //     使用指定的错误消息和错误代码初始化 IDCM.Data.Base.IDCMServException 类的新实例。
        //
        // 参数:
        //   message:
        //     解释异常原因的错误消息。
        //
        //   errorCode:
        //     异常的错误代码。
        public IDCMServException(string message, int errorCode)
            : base(message, errorCode)
        {
        }

        // 摘要:
        //     Adds extra information to the serialized object data specific to this class
        //     type. This is only used for serialization.
        //
        // 参数:
        //   info:
        //     Holds the serialized object data about the exception being thrown.
        //
        //   context:
        //     Contains contextual information about the source or destination.
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
