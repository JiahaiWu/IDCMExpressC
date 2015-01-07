using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Service.Utils;
using IDCM.Service.DataTransfer;

namespace IDCM.Service.BGHandler
{
    /// <summary>
    /// 将目标excel文档导入至目标数据库
    /// </summary>
    class ExcelImportHandler:AbsHandler
    {
        public ExcelImportHandler(string fpath,long lid,long plid)
        {
            this.xlsPath = System.IO.Path.GetFullPath(fpath);
            this.lid = lid;
            this.plid = plid;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res=false;
            res = ExcelDataImporter.parseExcelData(xlsPath, lid, plid);
            return new object[] { res, xlsPath };
        }

        private string xlsPath = null;
        private long lid = -1;
        private long plid = -1;
    }
}
