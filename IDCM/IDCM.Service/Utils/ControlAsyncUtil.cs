using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace IDCM.Service.Utils
{
    public class ControlAsyncUtil
    {
        internal delegate void InvokeHandler();

        internal delegate void InvokeMethodDelegate(Control control, InvokeHandler handler);
        /// <summary>
        /// 为指定的UI控件执行异步操作更新的方法
        /// </summary>
        /// <param name="control"></param>
        /// <param name="recvTarget"></param>
        /// <param name="data"></param>
        internal static void SyncInvoke(Control control, InvokeHandler handler)
        {
            // InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread. 
            // If these threads are different, it returns true. 
            if (control.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!control.IsHandleCreated || control.RecreatingHandle) //如果当前控件没有与它关联的句柄或正在重绘中，则等待执行。
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (control.Disposing || control.IsDisposed)
                        return;
                    Thread.Sleep(1);
                }
                IAsyncResult result = control.BeginInvoke(new InvokeMethodDelegate(SyncInvoke), new object[] { control, handler });
                control.EndInvoke(result);//获取委托执行结果的返回值
            }
            else
            {
                IAsyncResult result2 = control.BeginInvoke(handler);
                control.EndInvoke(result2);
            }
        }
        /// <summary>
        /// 为指定的UI控件执行异步操作更新的方法
        /// </summary>
        /// <param name="control"></param>
        /// <param name="handler"></param>
        internal static void SyncInvokeNoWait(Control control, InvokeHandler handler)
        {
            if (control.InvokeRequired)
            {
                //解决窗体关闭时出现“访问已释放句柄“的异常
                if (control.Disposing || control.IsDisposed)
                    return;
                IAsyncResult result = control.BeginInvoke(new InvokeMethodDelegate(SyncInvoke), new object[] { control, handler });
                control.EndInvoke(result);//获取委托执行结果的返回值
            }
            else
            {
                IAsyncResult result2 = control.BeginInvoke(handler);
                control.EndInvoke(result2);
            }
        }

        /// <summary>
        /// 为指定的UI空间执行异步的赋值更新的方法
        /// </summary>
        /// <param name="control"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        public static void SyncInvokeData(Control control, MethodInfo method, Object[] data)
        {
            SyncInvoke(control, new InvokeHandler(delegate()
            {
                method.Invoke(control, data);
            }));
        }
        /// <summary>
        /// 异步调用具有特定方法名称的UI控件方法
        /// </summary>
        /// <param name="control"></param>
        /// <param name="methodName"></param>
        /// <param name="data"></param>
        public static void SyncInvokeData(Control control, String methodName, Object data)
        {
            SyncInvokeData(control, methodName, new Object[] { data });
        }
        /// <summary>
        /// 异步调用具有特定方法名称的UI控件方法
        /// </summary>
        /// <param name="control"></param>
        /// <param name="methodName"></param>
        /// <param name="data"></param>
        public static void SyncInvokeData(Control control, String methodName, Object[] data)
        {
            MethodInfo method = control.GetType().GetMethod(methodName);
            if (method != null)
                SyncInvokeData(control, method, data);
            else
                throw new NotSupportedException("Un Supported Method to invoke! The method named with " + methodName + " should be declare as a public function.");
        }
        /// <summary>
        /// 异步调用具有特定名称的UI控件设置文本
        /// </summary>
        /// <param name="control"></param>
        /// <param name="data"></param>
        public static void SyncSetText(Control control, string data)
        {
            SyncInvokeNoWait(control, new InvokeHandler(delegate()
            {
                control.Text = data;
            }));
        }
        /// <summary>
        /// 异步调用具有特定名称的UI控件设置是否可见
        /// </summary>
        /// <param name="control"></param>
        /// <param name="data"></param>
        public static void SyncSetVisible(Control control, bool data)
        {
            SyncInvokeNoWait(control, new InvokeHandler(delegate()
            {
                control.Visible = data;
            }));
        }
    }
}
