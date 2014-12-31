using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using IDCM.Service.Common.Core;
using IDCM.Service.Common.Core.ServBuf;
/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.Service.Common
{
    /// <summary>
    /// 托管运行任务集线器类
    /// @author JiahaiWu
    /// </summary>
    public class DWorkMHub
    {
        /// <summary>
        /// 记录实例化对象
        /// </summary>
        /// <param name="formType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Form note(Form formView)
        {
            return LongTermHandleNoter.note(formView);
        }

        /// <summary>
        /// 记录带参线程句柄对象
        /// </summary>
        /// <param name="start"></param>
        /// <param name="tname"></param>
        /// <param name="maxStackSize"></param>
        /// <returns></returns>
        public static Thread note(Thread thread)
        {
            return LongTermHandleNoter.note(thread);
        }

        /// <summary>
        /// 向指定的对象实例和关联方法传递参数，并请求执行
        /// </summary>
        /// <param name="servInstance"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool callAsyncHandle(AbsHandler hanldeInstance, MethodInfo method = null,object[] args = null)
        {
            if (method != null)
                method.Invoke(hanldeInstance, args);
            BGWorkerPool.pushHandler(hanldeInstance);
            return true;
        }

        /// <summary>
        /// 获取特定类型的Form实例化对象
        /// </summary>
        /// <param name="formType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Form[] getActiveForm(Type formType)
        {
            return LongTermHandleNoter.getActiveForm(formType);
        }

        /// <summary>
        /// 查询句柄记录集，验证当前空闲状态
        /// </summary>
        /// <returns></returns>
        public static bool checkForIdle()
        {
            return LongTermHandleNoter.checkForIdle();
        }

        /// <summary>
        /// 向指定的对象实例和关联方法传递参数，并中断执行
        /// </summary>
        /// <param name="servInstance"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool killAsyncHandle(AbsHandler servInstance, MethodInfo method = null, Object[] args = null)
        {
            if (method != null)
                method.Invoke(servInstance, args);
            BGWorkerPool.abortHandlerByType(servInstance.GetType());
            return true;
        }
    }
}
