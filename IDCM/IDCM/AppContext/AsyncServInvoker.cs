using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ViewManager;
using IDCM.Data.Base;

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
            switch (msg.MsgType)
            {
                case MsgType.DataPrepared:
                    OnDataPrepared(this, new IDCMAsyncEventArgs(msg.MsgTag, msg.Parameters));
                    break;
                case MsgType.RetryQuickStartConnect:
                    OnRetryQuickStartConnect(this,new IDCMAsyncEventArgs(msg.MsgTag,msg.Parameters));
                    break;
                case MsgType.RequestHomeView:
                    OnRequestHomeView(this, new IDCMAsyncEventArgs(msg.MsgTag, msg.Parameters));
                    break;
                case MsgType.RequestGCMView:
                    OnRequestGCMView(this, new IDCMAsyncEventArgs(msg.MsgTag, msg.Parameters));
                    break;
                case MsgType.RetryDataPrepare:
                    OnRetryDataPrepare(this, new IDCMAsyncEventArgs(msg.MsgTag, msg.Parameters));
                    break;
                case MsgType.UpdateGCMSignTip:
                    OnUpdateGCMSignTip(this, new IDCMAsyncEventArgs(msg.MsgTag, msg.Parameters));
                    break;
                default:
                    log.Warn("Unhandled asynchronous message.  @msgTag=" + msg.MsgTag);
                    break;
            }
        }

        //定义数据源加载完成事件
        internal event IDCMAsyncRequest OnRequestHomeView;
        internal event IDCMAsyncRequest OnRequestGCMView;
        internal event IDCMAsyncRequest OnDataPrepared;
        internal event IDCMAsyncRequest OnRetryQuickStartConnect;
        internal event IDCMAsyncRequest OnRetryDataPrepare;
        internal event IDCMAsyncRequest OnUpdateGCMSignTip;
        #endregion

        //异步消息事件委托形式化声明
        public delegate void IDCMAsyncRequest(object sender, IDCMAsyncEventArgs e);
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
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
