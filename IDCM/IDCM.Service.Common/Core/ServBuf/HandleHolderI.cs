using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Service.Common.POO;

namespace IDCM.Service.Common.Core.ServBuf
{
    interface HandleHolderI
    {
        /// <summary>
        /// 转换生成线程对象的存活状态快照
        /// </summary>
        /// <returns></returns>
        HandleRunInfo ToHandleRunInfo();
        /// <summary>
        /// 获取线程对象存活与否状态
        /// </summary>
        /// <returns></returns>
        bool isAlive();
        /// <summary>
        /// 获取内置句柄实例
        /// </summary>
        dynamic InnerUnit { get; }
    }
}
