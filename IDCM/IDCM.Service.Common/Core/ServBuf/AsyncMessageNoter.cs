using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace IDCM.Service.Common.Core.ServBuf
{
    public class AsyncMessageNoter
    {
        /// <summary>
        /// 尝试移除并返回位于 msgQueue<T> 开始处的对象。
        /// </summary>
        /// <returns></returns>
        public static AsyncMessage pop()
        {
            AsyncMessage msg = null;
            msgQueue.TryDequeue(out msg);
            return msg;
        }

        /// <summary>
        /// 将对象添加到 msgQueue<T> 的结尾处。
        /// </summary>
        /// <param name="msg"></param>
        public static void push(AsyncMessage msg)
        {
            msgQueue.Enqueue(msg);
        }

        /// <summary>
        /// 异步转发消息缓冲池
        /// @author JiahaiWu 2014-11-07
        /// </summary>
        private static ConcurrentQueue<AsyncMessage> msgQueue = new ConcurrentQueue<AsyncMessage>();
    }
}
