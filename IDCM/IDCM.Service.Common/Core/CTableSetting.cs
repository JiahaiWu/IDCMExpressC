using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base.Utils;
using IDCM.Data.Base;
using System.Configuration;
using System.IO;

namespace IDCM.Service.Common.Core
{
    internal class CTableSetting
    {
        public static bool buildDefaultSetting()
        {
            try
            {
                string cTableDefpath = ConfigurationManager.AppSettings["CTableDef"];
                List<CustomTColDef> ctcds = getCustomTableDef(cTableDefpath);
                overwriteAllCustomTColDef(ctcds);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return false;
        }
        /// <summary>
        /// 重写用户自定义数据表的字段集定义
        /// </summary>
        /// <param name="ctcds"></param>
        /// <returns></returns>
        public static bool overwriteAllCustomTColDef(List<CustomTColDef> ctcds)
        {
            ////////////////////////////////////////////////
            //if (ctcds != null)
            //{
            //    List<CustomTColDef> ictcds = getEmbeddedTableDef();
            //    ctcds.AddRange(ictcds);
            //    CustomTColDefDAM.rebuildCustomTColDef();
            //    CustomTColDefDAM.save(ctcds.ToArray());
            //    return true;
            //}
            /////////////////////////////////////////////
            //调用层次有点问题，暂行实现阻断，有待改进
            return false;
        }
        public static Dictionary<string, List<CustomTColDef>> getTableTemplateDef(string settingPath)
        {
            Dictionary<string, List<CustomTColDef>> templDict = new Dictionary<string, List<CustomTColDef>>();
            if (File.Exists(settingPath))
            {
                List<CustomTColDef> ctcds = new List<CustomTColDef>();
                string[] lines = FileUtil.readAsUTF8Text(settingPath).Split(new char[] { '\n', '\r' });
                foreach (string line in lines)
                {
                    if (line.Length < 1 || line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("[") && line.TrimEnd().EndsWith("]"))
                    {
                        ctcds = new List<CustomTColDef>();
                        string groupName = line.Substring(1, line.LastIndexOf(']') - 1);
                        templDict[groupName] = ctcds;
                        continue;
                    }
                    if (line.StartsWith(">>Def"))
                    {
                        string ver = line.Substring(9).Trim();
                        ctcds.Clear();
                        continue;
                    }
                    CustomTColDef ctcd = formatSettingLine(line);
                    if (ctcd != null)
                        ctcds.Add(ctcd);
                }
                return templDict;
            }
            return null;
        }
        public static List<CustomTColDef> getCustomTableDef(string settingPath)
        {
            List<CustomTColDef> ctcds = new List<CustomTColDef>();
            if (File.Exists(settingPath))
            {
                string[] lines = FileUtil.readAsUTF8Text(settingPath).Split(new char[] { '\n', '\r' });
                foreach (string line in lines)
                {
                    if (line.Length < 1 || line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("[") && line.TrimEnd().EndsWith("]"))
                        continue;
                    if (line.StartsWith(">>Def"))
                    {
                        string ver = line.Substring(9).Trim();
                        ctcds.Clear();
                        continue;
                    }
                    CustomTColDef ctcd = formatSettingLine(line);
                    if (ctcd != null)
                        ctcds.Add(ctcd);
                }
                return ctcds;
            }
            else
            {
                log.Fatal("The setting file note exist! @Path=" + settingPath);
                throw new System.Data.DataException("The setting file note exist! @Path=" + settingPath);
            }
        }
        public static CustomTColDef formatSettingLine(string line)
        {
            string[] vals = line.Split(new char[] { ',' });
            if (vals.Length > 0)
            {
                CustomTColDef ctcd = new CustomTColDef();
                ctcd.Attr = CVNameConverter.toDBName(vals[0]);
                ctcd.AttrType = vals.Length > 1 ? vals[1] : AttrTypeConverter.IDCM_String;
                ctcd.IsRequire = vals.Length > 2 ? Convert.ToBoolean(vals[2]) : false;
                ctcd.IsUnique = vals.Length > 3 ? Convert.ToBoolean(vals[3]) : false;
                ctcd.Restrict = vals.Length > 4 ? vals[4] : null;
                ctcd.DefaultVal = vals.Length > 5 ? vals[5] : null;
                ctcd.Comments = vals.Length > 6 ? vals[6] : null;
                return ctcd;
            }
            return null;
        }
        public static List<CustomTColDef> getEmbeddedTableDef()
        {
            List<CustomTColDef> ctcds = new List<CustomTColDef>();
            CustomTColDef rid = new CustomTColDef();
            rid.Attr = CTDRecordA.CTD_RID;
            rid.AttrType = AttrTypeConverter.IDCM_Integer;
            rid.IsRequire = true;
            rid.IsUnique = true;
            rid.DefaultVal = "-1";
            rid.IsInter = true;
            rid.Comments = "CTDRecordDA.CTD_RID";
            ctcds.Add(rid);
            ///////////////////////////////////
            CustomTColDef plid = new CustomTColDef();
            plid.Attr = CTDRecordA.CTD_PLID;
            plid.AttrType = AttrTypeConverter.IDCM_Integer;
            plid.IsRequire = true;
            plid.IsUnique = false;
            plid.IsInter = true;
            plid.DefaultVal = CatalogNode.REC_ALL.ToString();
            plid.Comments = "CTDRecordDA.CTD_PLID";
            ctcds.Add(plid);
            ///////////////////////////////////////
            CustomTColDef lid = new CustomTColDef();
            lid.Attr = CTDRecordA.CTD_LID;
            lid.AttrType = AttrTypeConverter.IDCM_Integer;
            lid.IsRequire = true;
            lid.IsUnique = false;
            lid.IsInter = true;
            lid.DefaultVal = CatalogNode.REC_ALL.ToString();
            lid.Comments = "CTDRecordDA.CTD_LID";
            ctcds.Add(lid);

            return ctcds;
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
