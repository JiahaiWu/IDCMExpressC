using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Service.Common;

/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.Service.BGHandler
{
    public class UpdateTemplateHandler:AbsHandler
    {
        public UpdateTemplateHandler(DataSourceMHub datasource, LinkedList<CustomTColDef> customTCDList)
        {
            this.customTCDList = customTCDList;
            this.datasource = datasource;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            DWorkMHub.note(AsyncMessage.StartBackProgress);
            bool res = false;
            res=LocalRecordMHub.doUpdateProcess(datasource,customTCDList);
            return new object[] { res};
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override void complete(BackgroundWorker worker, bool canceled, Exception error, List<Object> args)
        {
            DWorkMHub.note(AsyncMessage.EndBackProgress);
            if (canceled)
                return;
            if (error != null)
            {
                MessageBox.Show("ERROR::" + error.Message + "\n" + error.StackTrace);
            }
            //重启数据源
            DWorkMHub.note(AsyncMessage.RetryDataPrepare);
        }

        private LinkedList<CustomTColDef> customTCDList = null;
        private DataSourceMHub datasource;
    }
}
