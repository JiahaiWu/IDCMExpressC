using IDCM.Data.Base;
using IDCM.Service.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Service.DataTransfer;
using System.IO;
using IDCM.Data.Base.Utils;
using System.Configuration;
using IDCM.Service.Utils;
using IDCM.Service.Validator;

namespace IDCM.Service.BGHandler
{
    public class LocalDataUploadHandler : AbsHandler
    {
        private DataSourceMHub datasource;
        private GCMSiteMHub gcmSite;
        private DataGridViewSelectedRowCollection selectedRows;
        private Dictionary<string, string> dataMapping;

        public LocalDataUploadHandler(DataSourceMHub datasource,GCMSiteMHub gcmSite, DataGridViewSelectedRowCollection selectedRows, Dictionary<string, string> dataMapping)
        {
            this.datasource = datasource;
            this.gcmSite = gcmSite;
            this.selectedRows = selectedRows;
            this.dataMapping = dataMapping;
        }

        /// <summary>
        /// 建立上传目标数据至GCM目录事务（内置临时XML导出及更新菌种资源软链接消息）
        /// 注意：
        /// 1.后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public override Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res = false;
            XMLImportStrainsRes importRes = null;
            DWorkMHub.note(AsyncMessage.StartBackProgress);
            LocalDataUploadExporter exporter = new LocalDataUploadExporter();
            if (selectedRows != null)
            {
                string sidAttr = ConfigurationManager.AppSettings.Get(SysConstants.StrainNumber);
                string sidName = null;
                ///////////////////////////////////////////////////////
                //将目标XML导出字段名和数据库存储字段名映射存储位序产生关联
                Dictionary<string, int> dbMaps = LocalRecordMHub.getCustomAttrDBMapping(datasource);
                Dictionary<string, int> dbLinkMaps = new Dictionary<string, int>();
                foreach (KeyValuePair<string, string> gcmMapEntry in dataMapping)
                {
                    if (gcmMapEntry.Value == null)
                        continue;
                    int dbOrder = -1;
                    if (dbMaps.TryGetValue(gcmMapEntry.Key, out dbOrder))
                    {
                        dbLinkMaps.Add(gcmMapEntry.Value, dbOrder);
                    }
                    if (sidName==null && gcmMapEntry.Value.Equals(sidAttr))
                        sidName = gcmMapEntry.Key;
                }
                if (sidName != null)
                {
                    int sidViewOrder = LocalRecordMHub.getViewOrder(datasource,sidName);
                    if (sidViewOrder > 0)
                    {
                        ////////////////////////////////////////////////////////
                        //抽取菌种号
                        List<string> linkIds = new List<string>();
                        for (int ridx = selectedRows.Count - 1; ridx >= 0; ridx--)
                        {
                            DataGridViewRow dgvRow = selectedRows[ridx];
                            DataGridViewCell dgvc = dgvRow.Cells[sidViewOrder];
                            if (dgvc != null)
                            {
                                string strainId = DGVUtil.getCellValue(dgvc);
                                if (strainId != null && strainId.Length > 0)
                                    linkIds.Add(strainId);
                            }
                        }
                        ///////////////////////////////////////////////////////
                        //导出XML文档数据
                        string xmldata=null;
                        res=exporter.exportGCMXML(datasource, selectedRows, dbLinkMaps, out xmldata);
                        if(res)
                        {
                            //提交前数据检验
                            if (GCMDataChecker.checkForPublish(ref xmldata))
                            {
                                //提交至GCM站点
                                importRes = GCMDataMHub.xmlImportStrains(gcmSite, xmldata);
                                if (importRes.msg_num.Equals("2"))
                                {
                                    //更新本地软连接记录信息
                                    DWorkMHub.note(new AsyncMessage(AsyncMessage.UpdateGCMLinkStrains, linkIds));
                                    MessageBox.Show("The selected data was uploaded into GCM Service.", "Publish Notice", MessageBoxButtons.OK);
                                }
                            }
                        }
                    }
                }
            }
            return new object[] { res, importRes };
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
    }
}
