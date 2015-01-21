using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 1.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.Service.Utils
{
    /// <summary>
    /// 本实例主要作用是为指定的ListView执行插入行，列，调用方法等操作
    /// 本类的所有方法实现均依靠IDCM.ControlMBL.AsyncInvoker.ControlAsyncUtil类
    /// 操作本类中方法的原因主要是，创建ListView的线程和调用ListView的线程不是同一线程
    /// 本类中的所有方法每次调用均会通过委托检测传入的ListView是否可用，创建与调用方是否为同一线程，而后通过第二层委托指定需要的操作
    /// @author 丁辉 2014-12-23
    /// </summary>
    public class ListViewAsyncUtil
    {
        /// <summary>
        /// 同步操作，将ColumnHeader加入到ListView
        /// 说明：
        /// 1：此方法有两个委托实现
        /// 2：第一个委托会检测ListView是否能正常使用，创建与调用方是否为同一线程
        /// 3：第二个委托会使用BeginInvoke来执行
        /// </summary>
        /// <param name="listView">需要加入到的ListView</param>
        /// <param name="zch">待加入的ColumnHeader</param>
        public static void syncAddColumn(ListView listView,ColumnHeader zch)
        {
            ControlAsyncUtil.SyncInvoke(listView, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (listView != null)
                    listView.Columns.Add(zch);
            }));
        }
        /// <summary>
        /// 同步操作，将ListViewItem加入到ListView
        /// 说明：
        /// 1：此方法有两个委托
        /// 2：第一个委托会检测ListView是否能正常使用，创建与调用方是否为同一线程
        /// 3：第二个委托会使用BeginInvoke来执行
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="item"></param>
        public static void syncAddItem(ListView listView, ListViewItem item)
        {
            ControlAsyncUtil.SyncInvoke(listView, new ControlAsyncUtil.InvokeHandler(delegate()
             {
                 if (listView != null)
                     listView.Items.Add(item);
             }));
        }
        /// <summary>
        /// 同步操作，调用ListView中具有指定方法名的方法
        /// 说明：
        /// 1：此方法首先会通过反射得到传入的方法名(方法名是string类型)
        /// 2：而后会检测控件是否可用，创建与调用方是否为同一线程
        /// 3：最后通过Invoke来执行方法
        /// 注意：
        /// 1：如果在控件中没有搜索到参数中的指定的方法会抛出NotSupportedException异常
        /// </summary>
        /// <param name="listView">需要调用方法的控件</param>
        /// <param name="method">需要调用的方法</param>
        /// <param name="data">方法参数(可选)</param>
        public static void SyncInvokeMethod(ListView listView, string method,Object[] data)
        {
            ControlAsyncUtil.SyncInvokeData(listView,method,data);
        }

        internal static void syncClearListView(ListView listView)
        {
            ControlAsyncUtil.SyncInvoke(listView, new ControlAsyncUtil.InvokeHandler(delegate()
            {
                if (listView != null)
                {
                    listView.Columns.Clear();
                    listView.Items.Clear();
                }
            }));
        }
    }
}
