using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IDCM.Data.Base;
using IDCM.Data.Base.Utils;
using IDCM.Data;
using IDCM.Service.Common;
using System.IO;
using System.Windows.Forms;

namespace IDCM.Service.DataTransfer
{
    class XMLExporter
    {
        /// <summary>
        /// 根据历史查询条件导出目标文本数据集
        /// @author JiahaiWu
        /// 通过getValidViewDBMapping()方法获取已经被缓存的用户浏览字段~数据库字段位序的映射关系；
        /// 然后对历史查询条件进行限定批量长度的轮询和导出转换操作。
        /// 本数据导出方式使用静态分隔符策略。
        /// 请注意对存储数据值中存在等同分隔符情形，尚未考虑额外处理策略，使用不当将造成导出不规范的数据文件。
        /// @note 更进一步支持转移字符转换的模式有待补充。
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="cmdstr"></param>
        /// <param name="tcount"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public bool exportXML(DataSourceMHub datasource, string filepath, string cmdstr, int tcount, string spliter = " ")
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                //int count = 0;
                //using (FileStream fs = new FileStream(filepath, FileMode.Create))
                //{
                //    Dictionary<string, int> maps = LocalRecordMHub.getCustomViewDBMapping(datasource);
                //    //填写表头
                //    int i = 0;
                //    string key = null;
                //    for (i = 0; i < maps.Count - 1; i++)
                //    {
                //        key = CVNameConverter.toViewName(maps.ElementAt(i).Key);
                //        strbuilder.Append(key).Append(spliter);
                //    }
                //    key = CVNameConverter.toViewName(maps.ElementAt(i).Key);
                //    strbuilder.Append(key);
                //    //填写内容////////////////////
                //    int offset = 0;
                //    int stepLen = SysConstants.EXPORT_PAGING_COUNT;
                //    while (offset < tcount)
                //    {
                //        int lcount = tcount - offset > stepLen ? stepLen : tcount - offset;
                //        DataTable table = LocalRecordMHub.queryCTDRecordByHistSQL(datasource, cmdstr, lcount, offset);
                //        foreach (DataRow row in table.Rows)
                //        {
                //            string dataLine = convertToXML(maps, row, spliter);
                //            strbuilder.Append("\n\r").Append(dataLine);
                //            /////////////
                //            if (++count % 100 == 0)
                //            {
                //                Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                //                BinaryWriter bw = new BinaryWriter(fs);
                //                fs.Write(info, 0, info.Length);
                //                strbuilder.Length = 0;
                //            }
                //        }
                //        if (strbuilder.Length > 0)
                //        {
                //            Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                //            BinaryWriter bw = new BinaryWriter(fs);
                //            fs.Write(info, 0, info.Length);
                //            strbuilder.Length = 0;
                //        }
                //        offset += lcount;
                //    }
                //    fs.Close();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR::" + ex.Message + "\n" + ex.StackTrace);
                log.Error(ex);
                return false;
            }
            return true;
        }
        private static string convertToXML(Dictionary<string, int> maps, DataRow row, string spliter)
        {
            StringBuilder strbuilder = new StringBuilder();
            //int j = 0;
            //int idx = -1;
            //for (j = 0; j < maps.Count - 1; j++)
            //{
            //    idx = maps.ElementAt(j).Value;
            //    idx = idx > SysConstants.Max_Attr_Count ? idx - SysConstants.Max_Attr_Count : idx;
            //    if (idx >= 0)
            //        strbuilder.Append(row[idx]).Append(spliter);
            //}
            //idx = maps.ElementAt(j).Value;
            //idx = idx > SysConstants.Max_Attr_Count ? idx - SysConstants.Max_Attr_Count : idx;
            //if (idx >= 0)
            //    strbuilder.Append(row[idx]);
            return strbuilder.ToString();
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
