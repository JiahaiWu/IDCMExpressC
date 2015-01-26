using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Service.Utils;
using IDCM.Service.DataTransfer;
using IDCM.Service.Common;

namespace IDCM.Service.BGHandler
{
    /// <summary>
    /// 将目标excel文档导入至目标数据库
    /// </summary>
    public class ExcelImportHandler:AbsHandler
    {
        public ExcelImportHandler(DataSourceMHub datasource, string fpath, ref Dictionary<string, string> dataMapping, long lid, long plid)
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
        public override Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res=false;
            DWorkMHub.note(AsyncMessage.StartBackProgress);
            res = ExcelDataImporter.parseExcelData(datasource,xlsPath, ref dataMapping, lid, plid);
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
