using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.DataTransfer
{
    class GCMJSONExporter
    {
        /// <summary>
        /// 以JSON格式导出数据，此方法一般用做数据源以DataTable为格式的导出.
        /// 说明：
        /// 1：此方法有两种情况
        ///     A：exportDetail==true，需要导出详细列表，此方法会发送网络请求，比较耗时.
        ///     B：exportDetail==false，不需要导出详细列表，不会发送网络请求，直接导出DataTable中数据.
        /// 主意：
        /// 1：此方法可能会导致客户端弹出异常信息，但是不会影响程序运行(用户输入的路径无法读写)
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="strainViewList"></param>
        /// <param name="exportDetail"></param>
        /// <param name="gcmSiteHolder"></param>
        /// <returns></returns>
        public bool exportJSONList(string xpath,DataTable strainViewList,bool exportDetail,GCMSiteMHub gcmSiteHolder = null)
        {
            if (strainViewList == null | strainViewList.Rows.Count < 1) return true;
            StringBuilder strbuilder = new StringBuilder();
            using (FileStream fs = new FileStream(xpath, FileMode.Create))
            {
                for (int i = 0; i < strainViewList.Rows.Count; i++)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    DataRow row = strainViewList.Rows[i];
                    for (int j = 0; j < strainViewList.Columns.Count; j++)
                    {
                        dict[strainViewList.Columns[j].ColumnName] = Convert.ToString(row[j]);
                    }
                    if (exportDetail)
                    {
                        string strainID = Convert.ToString(row[0]);
                        StrainView sv = getStrainView(gcmSiteHolder, strainID);
                        Dictionary<string, object> maps_from = sv.ToDictionary();
                        AddToDictionary(dict, maps_from);
                    }
                    string jsonStr = JsonConvert.SerializeObject(dict);
                    strbuilder.Append(jsonStr).Append("\n");
                }
                if (strbuilder.Length > 0)
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                    BinaryWriter bw = new BinaryWriter(fs);
                    fs.Write(info, 0, info.Length);
                    strbuilder.Length = 0;
                }
            }
            return true;
        }

        /// <summary>
        /// 以JSON格式导出数据，此方法一般用做数据源以DataGridViewSelectedRowCollection为格式的导出.
        /// 说明：
        /// 1：此方法有两种情况
        ///     A：exportDetail==true，需要导出详细列表，此方法会发送网络请求，比较耗时.
        ///     B：exportDetail==false，不需要导出详细列表，不会发送网络请求，直接导出DataGridViewSelectedRowCollection中数据.
        /// 主意：
        /// 1：此方法可能会导致客户端弹出异常信息，但是不会影响程序运行(用户输入的路径无法读写)
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="selectedRows"></param>
        /// <param name="exportDetail"></param>
        /// <param name="gcmSiteHolder"></param>
        /// <returns></returns>
        internal bool exportJSONList(string xpath, DataGridViewSelectedRowCollection selectedRows, bool exportDetail, GCMSiteMHub gcmSiteHolder = null)
        {
            if (selectedRows == null | selectedRows.Count < 1) return true;
            StringBuilder strbuilder = new StringBuilder();
            DataGridView dgv = selectedRows[0].Cells[0].DataGridView;
            using (FileStream fs = new FileStream(xpath, FileMode.Create))
            {
                for (int i = 0; i < selectedRows.Count;i++ )
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    DataGridViewRow row = selectedRows[i];
                    for (int j = 1; j < row.Cells.Count; j++)
                    {
                        dict[dgv.Columns[j].Name] = Convert.ToString(row.Cells[j].Value);
                    }
                    if (exportDetail)
                    {
                        string strainID = Convert.ToString(row.Cells[1].Value);
                        StrainView sv = getStrainView(gcmSiteHolder, strainID);
                        Dictionary<string, object> maps_from = sv.ToDictionary();
                        AddToDictionary(dict, maps_from);
                    }
                    string jsonStr = JsonConvert.SerializeObject(dict);
                    strbuilder.Append(jsonStr).Append("\n");
                }
                if (strbuilder.Length > 0)
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                    BinaryWriter bw = new BinaryWriter(fs);
                    fs.Write(info, 0, info.Length);
                    strbuilder.Length = 0;
                }
            }
            return true;
        }

        public void AddToDictionary(Dictionary<string, string> dict_To, Dictionary<string, object> maps_from)
        {
            foreach (KeyValuePair<string, object> svEntry in maps_from)
            {
                if (svEntry.Value is string)
                {
                    dict_To[svEntry.Key] = svEntry.Value as string;
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        string keyName = svEntry.Key + "/" + subEntry.Key;
                        dict_To[keyName] = Convert.ToString(subEntry.Value);
                    }
                }
            }
        }

        private StrainView getStrainView(GCMSiteMHub gcmSiteHolder, string strainID)
        {
            GCMDataMHub gcmDataHub = new GCMDataMHub();
            return gcmDataHub.strainViewQuery(gcmSiteHolder, strainID);
        }
    }
}
