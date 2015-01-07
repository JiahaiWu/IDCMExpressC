using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Service.Utils;

namespace IDCM.ServiceBL.Handle
{
    
    class LoadGCMDataHandler : AbsHandler
    {
        public LoadGCMDataHandler(DataGridView itemDGV,TreeView recordTree,ListView recordView,Dictionary<string, int> loadedNoter)
        {
            this.itemDGV = itemDGV;
            this.recordTree=recordTree;
            this.recordView = recordView;
            this.loadedNoter = loadedNoter;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res=false;
            loadedNoter.Clear();
            res = GCMDataLoader.loadData(itemDGV,recordTree,recordView,loadedNoter);
            return new object[] { res };
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
        }

        private DataGridView itemDGV = null;
        private TreeView recordTree=null;
        private ListView recordView=null;
        private Dictionary<string, int> loadedNoter = null;
    }
}
