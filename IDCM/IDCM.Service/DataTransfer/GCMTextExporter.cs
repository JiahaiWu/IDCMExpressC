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
        public bool exportText(string filepath, DataTable starinViewList, bool exportDetail, string spliter, GCMSiteMHub gcmSiteHolder = null)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    StringBuilder strbuilder = new StringBuilder();
                    int startIndex = starinViewList.Columns.Count;
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
                            Dictionary<string, object> maps = sv.ToDictionary();
                            buildRow(strbuilder, maps, spliter);
                        }
                        strbuilder.Append("\n");
                    }
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

        public bool exportText(string filepath, DataGridViewSelectedRowCollection selectedRows, bool exportDetail, string spliter, GCMSiteMHub gcmSiteHolder = null)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    StringBuilder strbuilder = new StringBuilder();
                    for (int i = 0; i < selectedRows.Count;i++ )
                    {
                        DataGridViewRow row = selectedRows[i];
                        for (int j = 1; j < row.Cells.Count;j++ )
                        {
                            strbuilder.Append(Convert.ToString(row.Cells[j].Value)).Append(spliter);
                        }
                        if(exportDetail)
                        {
                            string strainId = Convert.ToString(row.Cells[1].Value);
                            StrainView sv = getStrainView(gcmSiteHolder, strainId);
                            Dictionary<string, object> maps = sv.ToDictionary();
                            buildRow(strbuilder, maps, spliter);
                        }
                        strbuilder.Append("\n");
                    }
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

        private void buildRow(StringBuilder strbuilder, Dictionary<string, object> maps, string spliter)
        {
            foreach (KeyValuePair<string, object> svEntry in maps)
            {
                if (svEntry.Value is string)
                {
                    strbuilder.Append(svEntry.Value).Append(spliter);
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        string value = Convert.ToString(subEntry.Value);
                        strbuilder.Append(value).Append(spliter);
                    }
                }
            }
        }

        private string addColumnStr(DataRow row, string spliter, Dictionary<string, object> maps)
        {
            StringBuilder strBuilder = new StringBuilder();
            DataTable dt = row.Table;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strBuilder.Append(dt.Columns[i].ColumnName).Append(spliter);
            }
            if (maps == null) return strBuilder.ToString();
            addStrainViewColumnStr(strBuilder, spliter, maps);
            return strBuilder.ToString();
        }

        private string addColumnStr(DataGridViewRow row, string spliter, Dictionary<string, object> maps = null)
        {
            StringBuilder strBuilder = new StringBuilder();
            DataGridView dgv = row.DataGridView;
            for (int i = 1; i < dgv.Columns.Count;i++ )
            {
                strBuilder.Append(dgv.Columns[i].Name).Append(spliter);
            }
            if (maps == null) return strBuilder.ToString();
            addStrainViewColumnStr(strBuilder, spliter, maps);
            return strBuilder.ToString();
        }
        private void addStrainViewColumnStr(StringBuilder strBuilder, string spliter, Dictionary<string, object> maps)
        {
            foreach (KeyValuePair<string, object> svEntry in maps)
            {
                if (svEntry.Value is string)
                {
                    strBuilder.Append(svEntry.Key).Append(spliter);
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        string columnName = svEntry.Key + "/" +subEntry.Key;
                        strBuilder.Append(columnName).Append(spliter);
                    }
                }
            }
        }

        private StrainView getStrainView(GCMSiteMHub gcmSiteHolder, string strainID)
        {
            GCMDataMHub gcmDataHub = new GCMDataMHub();
            return gcmDataHub.strainViewQuery(gcmSiteHolder, strainID);
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
