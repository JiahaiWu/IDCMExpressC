using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base
{
    /// <summary>
    /// 异步消息类型及附属参数的封装类
    /// </summary>
    public class AsyncMessage
    {
        public static readonly AsyncMessage DataPrepared = new AsyncMessage(MsgType.DataPrepared, "Data Prepared");
        public static readonly AsyncMessage RetryQuickStartConnect = new AsyncMessage(MsgType.RetryQuickStartConnect, "Retry Quick Start Connect");
        public static readonly AsyncMessage RequestHomeView = new AsyncMessage(MsgType.RequestHomeView, "Request Home View");
        public static readonly AsyncMessage RequestGCMView = new AsyncMessage(MsgType.RequestGCMView, "Request GCM View");
        /// <summary>
        /// 重启数据源事件消息
        /// </summary>
        public static readonly AsyncMessage RetryDataPrepare = new AsyncMessage(MsgType.RetryDataPrepare, "Retry Data Prepare Operation");
        public static readonly AsyncMessage UpdateGCMSignTip = new AsyncMessage(MsgType.UpdateGCMSignTip, "Update GCM Sign Tip");
        public static readonly AsyncMessage StartBackProgress = new AsyncMessage(MsgType.StartBackProgress, "Start Back Progress");
        public static readonly AsyncMessage EndBackProgress = new AsyncMessage(MsgType.EndBackProgress, "End Back Progress");
        /// <summary>
        /// 更新GCM获取记录信息
        /// </summary>
        public static readonly AsyncMessage UpdateGCMLinkStrains = new AsyncMessage(MsgType.UpdateGCMLinkStrains, "Update GCM Link Strains");
        /// <summary>
        /// 更新Local软连接记录信息
        /// </summary>
        public static readonly AsyncMessage UpdateLocalLinkTags = new AsyncMessage(MsgType.UpdateLocalLinkTags, "Update Local Link Tags");
        
        /// <summary>
        /// For iterator 
        /// </summary>
        public static IEnumerable<AsyncMessage> Values
        {
            get
            {
                yield return DataPrepared;
                yield return RetryQuickStartConnect;
                yield return RequestHomeView;
                yield return RequestGCMView;
                yield return RetryDataPrepare;
                yield return UpdateGCMSignTip;
                yield return StartBackProgress;
                yield return EndBackProgress;
                yield return UpdateGCMLinkStrains;
                yield return UpdateLocalLinkTags;
            }
        }
        private readonly string msgTag;
        private readonly MsgType msgType;
        private readonly object[] parameters;

        public AsyncMessage(AsyncMessage amsg, params object[] parameters)
        {
            this.msgTag = amsg.msgTag;
            this.msgType = amsg.msgType;
            this.parameters = parameters;
        }

        protected AsyncMessage(MsgType msgType, string msgTag, object[] parameters = null)
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
    public enum MsgType
    {
        DataPrepared = 0,
        RetryQuickStartConnect = 1,
        RequestHomeView=2,
        RequestGCMView=3,
        RetryDataPrepare=4,
        UpdateGCMSignTip=5,
        StartBackProgress=6,
        EndBackProgress = 7,
        UpdateGCMLinkStrains = 8,
        UpdateLocalLinkTags = 9
    }
}
