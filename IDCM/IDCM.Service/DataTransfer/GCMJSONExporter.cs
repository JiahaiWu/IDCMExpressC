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
        /// 以JSON格式导出数据，数据源DataTable.
        /// </summary>
        /// <param name="xpath">导出路径</param>
        /// <param name="strainViewList">数据源</param>
        /// <param name="exportDetail">是否导出strain_tree</param>
        /// <param name="gcmSiteHolder">底层GCMSiteMHub句柄，用于获取strain_tree，封装GCM账户信息</param>
        /// <returns></returns>
        public bool exportJSONList(string xpath, DataTable strainViewList, bool exportDetail, GCMSiteMHub gcmSiteHolder = null)
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
                        Dictionary<string, object> strain_treeMap_from = sv.ToDictionary();
                        AddToDictionary(dict, strain_treeMap_from);
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
        /// 以JSON格式导出数据，数据源DataGridViewSelectedRowCollection
        /// </summary>
        /// <param name="xpath">导出路径</param>
        /// <param name="selectedRows">数据源</param>
        /// <param name="exportDetail">是否导出strain_tree</param>
        /// <param name="gcmSiteHolder">底层GCMSiteMHub句柄，用于获取strain_tree，封装GCM账户信息</param>
        /// <returns></returns>
        internal bool exportJSONList(string xpath, DataGridViewSelectedRowCollection selectedRows, bool exportDetail, GCMSiteMHub gcmSiteHolder = null)
        {
            if (selectedRows == null | selectedRows.Count < 1) return true;
            StringBuilder strbuilder = new StringBuilder();
            DataGridView dgv = selectedRows[0].Cells[0].DataGridView;
            using (FileStream fs = new FileStream(xpath, FileMode.Create))
            {
                for (int ridx = selectedRows.Count - 1; ridx >= 0; ridx--)
                {
                    DataGridViewRow row = selectedRows[ridx];
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    for (int j = 1; j < row.Cells.Count; j++)
                    {
                        dict[dgv.Columns[j].Name] = Convert.ToString(row.Cells[j].Value);
                    }
                    if (exportDetail)
                    {
                        string strainID = Convert.ToString(row.Cells[1].Value);
                        StrainView sv = getStrainView(gcmSiteHolder, strainID);
                        Dictionary<string, object> strain_treeMap_from = sv.ToDictionary();
                        AddToDictionary(dict, strain_treeMap_from);
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
        /// 将strain_tree的map数据添加到需要数据源构建好的map中
        /// </summary>
        /// <param name="dict_To">数据源构建好的map</param>
        /// <param name="strain_treeMap_from">需要添加的map</param>
        public void AddToDictionary(Dictionary<string, string> dict_To, Dictionary<string, object> strain_treeMap_from)
        {
            foreach (KeyValuePair<string, object> svEntry in strain_treeMap_from)
            {
                if (svEntry.Value is string)
                {
                    dict_To[svEntry.Key] = svEntry.Value as string;
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        string keyName = svEntry.Key + "_" + subEntry.Key;
                        dict_To[keyName] = Convert.ToString(subEntry.Value);
                    }
                }
            }
        }
        /// <summary>
        /// 获取strain_tree
        /// </summary>
        /// <param name="gcmSiteHolder">底层GCMSiteMHub句柄，封装GCM账户信息</param>
        /// <param name="strainID">strain的ID</param>
        /// <returns>strain_tree</returns>
        private StrainView getStrainView(GCMSiteMHub gcmSiteHolder, string strainID)
        {
            GCMDataMHub gcmDataHub = new GCMDataMHub();
            return gcmDataHub.strainViewQuery(gcmSiteHolder, strainID);
        }
    }
}
