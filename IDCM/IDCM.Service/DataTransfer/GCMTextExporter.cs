using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.DataTransfer
{
    class GCMTextExporter
    { 
        /// <summary>
        /// 导出数据到文本或csv(根据分隔符)，数据源DataTable
        /// </summary>
        /// <param name="filepath">导出路径</param>
        /// <param name="starinViewList">数据源</param>
        /// <param name="exportDetail">是否导出strain_tree</param>
        /// <param name="spliter">分隔符，如果导出文本分隔符是"\t"，如果导出csv分隔符是","</param>
        /// <param name="gcmSiteHolder">底层GCMSiteMHub句柄，用于获取strain_tree，封装GCM账户信息</param>
        /// <returns></returns>
        public bool exportText(string filepath, DataTable starinViewList, bool exportDetail, string spliter, GCMSiteMHub gcmSiteHolder = null)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    StringBuilder strbuilder = new StringBuilder();
                    string columnStr = "";
                    for (int i = 0; i < starinViewList.Columns.Count;i++ )
                    {
                        columnStr = strbuilder.Append(starinViewList.Columns[i].ColumnName).Append(spliter).ToString();
                    }
                    strbuilder.Clear();
                    //int startIndex = starinViewList.Columns.Count;
                    for (int i = 0; i < starinViewList.Rows.Count; i++)
                    {
                        DataRow row = starinViewList.Rows[i];
                        for (int j = 0; j < starinViewList.Columns.Count;j++ )
                        {
                            strbuilder.Append(Convert.ToString(row[j])).Append(spliter);
                        }
                        if (exportDetail)//如果不需要导出详细信息，就让maps等于Null
                        {
                            string strainId = Convert.ToString(row[0]);
                            StrainView sv = getStrainView(gcmSiteHolder, strainId);
                            Dictionary<string, object> strain_treeMap = sv.ToDictionary();
                            mergeMapValueToRow(strbuilder, strain_treeMap, spliter);                 
                            if(i == 0)columnStr += buildColumn(strain_treeMap, spliter);
                        }
                        strbuilder.Append("\n");
                    }
                    strbuilder.Insert(0, columnStr+"\n");
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR::" + ex.Message + "\n" + ex.StackTrace);
                log.Error(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 导出数据到文本或csv(根据分隔符)，数据源DataGridViewSelectedRowCollection
        /// </summary>
        /// <param name="filepath">导出路径</param>
        /// <param name="selectedRows">数据源</param>
        /// <param name="exportDetail">是否导出strain_tree</param>
        /// <param name="spliter">分隔符，如果导出文本分隔符是"\t"，如果导出csv分隔符是","</param>
        /// <param name="gcmSiteHolder">底层GCMSiteMHub句柄，用于获取strain_tree，封装GCM账户信息</param>
        public bool exportText(string filepath, DataGridViewSelectedRowCollection selectedRows, bool exportDetail, string spliter, GCMSiteMHub gcmSiteHolder = null)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    StringBuilder strbuilder = new StringBuilder();
                    string columnStr = "";
                    DataGridView dgv = selectedRows[0].DataGridView;
                    for (int i = 1; i < dgv.Columns.Count; i++)
                    {
                        columnStr = strbuilder.Append(dgv.Columns[i].Name).Append(spliter).ToString();
                    }
                    strbuilder.Clear();
                    for (int i = 0; i < selectedRows.Count; i++)
                    {
                        DataGridViewRow row = selectedRows[i];
                        for (int j = 1; j < row.Cells.Count; j++)
                        {
                            strbuilder.Append(Convert.ToString(row.Cells[j].Value)).Append(spliter);
                        }
                        if (exportDetail)
                        {
                            string strainId = Convert.ToString(row.Cells[1].Value);
                            StrainView sv = getStrainView(gcmSiteHolder, strainId);
                            Dictionary<string, object> strain_treeMap = sv.ToDictionary();
                            mergeMapValueToRow(strbuilder, strain_treeMap, spliter);
                            if (i == 0) if (i == 0) columnStr += buildColumn(strain_treeMap, spliter);
                        }
                        strbuilder.Append("\n");
                    }
                    strbuilder.Insert(0, columnStr + "\n");
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR::" + ex.Message + "\n" + ex.StackTrace);
                log.Error(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 构建strain_tree列
        /// </summary>
        /// <param name="strain_treeMap">strain_tree,key->value映射集合</param>
        /// <param name="spliter">分隔符，如果导出文本分隔符是"\t"，如果导出csv分隔符是","</param>
        /// <returns></returns>
        private string buildColumn(Dictionary<string, object> strain_treeMap, string spliter)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (KeyValuePair<string, object> svEntry in strain_treeMap)
            {
                if (svEntry.Value is string)
                {
                    strBuilder.Append(svEntry.Key).Append(spliter);
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        string columnName = svEntry.Key + "_" + subEntry.Key;
                        strBuilder.Append(columnName).Append(spliter);
                    }
                }
            }
            return strBuilder.ToString();
        }
        /// <summary>
        /// 合并strain_tree中的value，到数据源的一行数据中
        /// </summary>
        /// <param name="dataSourceRow">数据源一行数据</param>
        /// <param name="strain_treeMap">strain_tree,key->value映射集合</param>
        /// <param name="spliter">分隔符，如果导出文本分隔符是"\t"，如果导出csv分隔符是","</param>
        private void mergeMapValueToRow(StringBuilder dataSourceRow, Dictionary<string, object> strain_treeMap, string spliter)
        {
            foreach (KeyValuePair<string, object> svEntry in strain_treeMap)
            {
                if (svEntry.Value is string)
                {
                    dataSourceRow.Append(svEntry.Value).Append(spliter);
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        string value = Convert.ToString(subEntry.Value);
                        dataSourceRow.Append(value).Append(spliter);
                    }
                }
            }
        }
        /// <summary>
        /// 获取strain_tree
        /// </summary>
        /// <param name="gcmSiteHolder">底层GCMSiteHolder句柄，封装GCMGCM账户信息</param>
        /// <param name="strainID">strain的ID</param>
        /// <returns>strain_tree</returns>
        private StrainView getStrainView(GCMSiteMHub gcmSiteHolder, string strainID)
        {
            GCMDataMHub gcmDataHub = new GCMDataMHub();
            return gcmDataHub.strainViewQuery(gcmSiteHolder, strainID);
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
