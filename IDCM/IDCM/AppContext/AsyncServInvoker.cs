using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ViewManager;
using IDCM.Service.Common.Core;

namespace IDCM.AppContext
{
    internal class AsyncServInvoker
    {
        #region IDCM全局异步消息事务分发处理
        /// <summary>
        /// 异步消息事务分发处理
        /// </summary>
        /// <param name="msg"></param>
        internal void dispatchMessage(AsyncMessage msg)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(msg != null);
#endif
            if (msg.MsgType.Equals(MsgType.DataPrepared))
            {
                OnDataPrepared(this, new IDCMAsyncEventArgs(msg.MsgTag, msg.Parameters));
            }
        }

        //定义数据源加载完成事件
        public event IDCMAsyncRequest OnDataPrepared;

        #endregion

        //异步消息事件委托形式化声明
        public delegate void IDCMAsyncRequest(object sender, IDCMAsyncEventArgs e);
        
    }
    /// <summary>
    /// 异步消息事件消息细节参数类定义
    /// </summary>
    internal class IDCMAsyncEventArgs : EventArgs
    {
        public readonly string msgTag;
        public readonly object[] values;

        public IDCMAsyncEventArgs(string msgTag, object[] vals)
        {
            this.msgTag = msgTag;
            this.values = vals;
        }
    }
}
