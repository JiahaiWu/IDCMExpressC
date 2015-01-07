using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Service.Utils;

/********************************
 * Individual Data Center of Microbial resources (IDCM)
 * A desktop software package for microbial resources researchers.
 * 
 * Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
 * 
 * @Contact NO.1 Beichen West Road, Chaoyang District, Beijing 100101, Email: office@im.ac.cn
 */
namespace IDCM.ServiceBL.Handle
{
    class UpdateTemplateHandler:AbsHandler
    {
        public UpdateTemplateHandler(LinkedList<CustomTColDef> customTCDList)
        {
            this.customTCDList = customTCDList;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res = false;
            TemplateUpdater updater = new TemplateUpdater();
            res=updater.doUpdateProcess(customTCDList);
            return new object[] { res};
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public void complete(BackgroundWorker worker, bool canceled, Exception error, List<Object> args)
        {
            if (canceled)
                return;
            if (error != null)
            {
                MessageBox.Show("ERROR::" + error.Message + "\n" + error.StackTrace);
                return;
            }
            if (IDCMAppContext.MainManger != null)
            {
                IDCMAppContext.MainManger.getManager(typeof(HomeViewManager)).dispose();
                IDCMAppContext.MainManger.activeChildView(typeof(HomeViewManager), true);
            }
            else
                MessageBox.Show("ERROR::IDCMAppContext.MainManger is NULL.\n");
        }
        /// <summary>
        /// 后台任务执行过程中的状态反馈代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="progressPercentage"></param>
        /// <param name="args"></param>
        public void progressChanged(BackgroundWorker worker, int progressPercentage, List<Object> args)
        {
            if (progressPercentage == 0)
            {
                FrontProgressPrompt.startFrontProgress();
            }
            if (progressPercentage == 100)
            {
                FrontProgressPrompt.endFrontProgress();
            }
        }

        private LinkedList<CustomTColDef> customTCDList = null;
    }
}
