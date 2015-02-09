using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.DataTransfer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IDCM.Service.BGHandler
{
    public class XMLImportHandler : AbsHandler
    {
        public XMLImportHandler(DataSourceMHub datasource, string fpath, ref Dictionary<string, string> dataMapping, long lid, long plid)
        {
            this.xlsPath = System.IO.Path.GetFullPath(fpath);
            this.lid = lid;
            this.plid = plid;
            this.dataMapping = dataMapping;
            this.datasource = datasource;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override object doWork(System.ComponentModel.BackgroundWorker worker, bool cancel, List<object> args)
        {
            bool res = false;
            DWorkMHub.note(AsyncMessage.StartBackProgress);
            res = XMLDataImporter.parseXMLData(datasource, xlsPath, ref dataMapping, lid, plid);
            return new object[] { res, xlsPath };
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
                log.Error(error);
                return;
            }
        }
        public override void addHandler(AbsHandler nextHandler)
        {
            base.addHandler(nextHandler);
        }
        private string xlsPath = null;
        private long lid = -1;
        private long plid = -1;
        private Dictionary<string, string> dataMapping = null;
        private DataSourceMHub datasource = null;

    }
}
