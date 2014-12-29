using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using IDCM.Data.POO;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.IO;
using IDCM.Data.Common;
using System.Configuration;

namespace IDCM.Data.DAM
{
    class CustomTColDefDAM
    {
        /// <summary>
        /// 重建用户录入数据表的动态形式化定义
        /// 说明:
        /// 1.此操作将彻底清空原有表内数据记录
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        public static bool rebuildCustomTColDef(SQLiteConnPicker picker)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(picker != null);
#endif
            string cmd = @"drop table if exists " + typeof(CustomTColDef).Name + ";";
            cmd += "Create Table if Not Exists " + typeof(CustomTColDef).Name + "("
                + "Attr TEXT primary key,"
                + "AttrType TEXT default " + "string" + ","
                + "Comments TEXT,"
                + "Restrict TEXT,"
                + "IsUnique TEXT default '" + false.ToString() + "',"
                + "IsRequire TEXT default '" + false.ToString() + "',"
                + "DefaultVal TEXT default NULL,"
                + "Corder INTEGER default 0,"
                + "IsInter TEXT default '" + false.ToString() + "');";
            using (picker)
            {
                int res = picker.getConnection().Execute(cmd);
                return res>-1;
            }
        }

        /// <summary>
        /// 读取并创建默认的动态表单定义
        /// </summary>
        /// <param name="picker"></param>
        /// <returns></returns>
        public static bool buildDefaultSetting(SQLiteConnPicker picker)
        {
            try
            {
                string cTableDefpath = ConfigurationManager.AppSettings["CTableDef"];
                List<CustomTColDef> ctcds = getCustomTableDef(cTableDefpath);
                return overwriteAllCustomTColDef(picker,ctcds);
            }
            catch (Exception ex)
            {
                log.Fatal("无法读取并创建默认的动态表单定义，请检查CTableDef参数配置。",ex);
            }
            return false;
        }
        /// <summary>
        /// 重写用户自定义数据表的字段集定义
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="ctcds"></param>
        /// <returns></returns>
        public static bool overwriteAllCustomTColDef(SQLiteConnPicker picker,List<CustomTColDef> ctcds)
        {
            if (ctcds != null)
            {
                List<CustomTColDef> ictcds = getEmbeddedTableDef();
                ctcds.AddRange(ictcds);
                rebuildCustomTColDef(picker);
                CustomTColDefDAM.save(picker,ctcds.ToArray());
                return true;
            }
            return false;
        }
        /// <summary>
        /// 保存新列属性定义记录集
        /// </summary>
        /// <param name="ctcd"></param>
        /// <returns></returns>
        public static int save(SQLiteConnPicker picker, params CustomTColDef[] ctcds)
        {
            List<string> cmds = new List<string>();
            foreach (CustomTColDef ctcd in ctcds)
            {
                StringBuilder cmdBuilder = new StringBuilder();
                cmdBuilder.Append("insert Or Replace into " + typeof(CustomTColDef).Name
                    + "(attr,attrType,comments,defaultVal,corder,isInter,isRequire,isUnique,restrict) values(");
                cmdBuilder.Append("'").Append(ctcd.Attr).Append("',");
                if (ctcd.AttrType == null)
                    cmdBuilder.Append("null,");
                else
                    cmdBuilder.Append("'").Append(ctcd.AttrType).Append("',");
                if (ctcd.Comments == null)
                    cmdBuilder.Append("null,");
                else
                    cmdBuilder.Append("'").Append(ctcd.Comments).Append("',");
                if (ctcd.DefaultVal == null)
                    cmdBuilder.Append("null,");
                else
                    cmdBuilder.Append("'").Append(ctcd.DefaultVal).Append("',");
                cmdBuilder.Append(ctcd.Corder).Append(",");
                cmdBuilder.Append("'").Append(ctcd.IsInter).Append("',");
                cmdBuilder.Append("'").Append(ctcd.IsRequire).Append("',");
                cmdBuilder.Append("'").Append(ctcd.IsUnique).Append("',");
                if (ctcd.Restrict == null)
                    cmdBuilder.Append("null");
                else
                    cmdBuilder.Append("'").Append(ctcd.Restrict).Append("'");
                cmdBuilder.Append(");");
                cmds.Add(cmdBuilder.ToString());
            }
            if (cmds.Count > 0)
            {
                int[] res = DAMBase.executeSQL(picker, cmds.ToArray());
                return res.Length;
            }
            return -1;
        }
        /// <summary>
        /// 查询所有数据表属性定义对象
        /// </summary>
        /// <param name="picker"></param>
        /// <param name="refresh"></param>
        /// <returns></returns>
        public static List<CustomTColDef> loadAll(SQLiteConnPicker picker, bool refresh = true)
        {
            if (refresh)
            {
                lock (ctcdCache)
                {
                    ctcdCache.Clear();
                    string cmd = "SELECT * FROM CustomTColDef order by corder";
                    using (picker)
                    {
                        List<CustomTColDef> ctcds = picker.getConnection().Query<CustomTColDef>(cmd).ToList<CustomTColDef>();
                        foreach (CustomTColDef ctcd in ctcds)
                        {
                            ctcdCache[ctcd.Attr]= ctcd;
                        }
                        return ctcds;
                    }
                }
            }
            return ctcdCache.Values.ToList<CustomTColDef>();
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
            }
            return null;
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
            rid.Attr = CTDRecordDAM.CTD_RID;
            rid.AttrType = AttrTypeConverter.IDCM_Integer;
            rid.IsRequire = true;
            rid.IsUnique = true;
            rid.DefaultVal = "-1";
            rid.IsInter = true;
            rid.Comments = "CTDRecordDA.CTD_RID";
            ctcds.Add(rid);
            ///////////////////////////////////
            CustomTColDef plid = new CustomTColDef();
            plid.Attr = CTDRecordDAM.CTD_PLID;
            plid.AttrType = AttrTypeConverter.IDCM_Integer;
            plid.IsRequire = true;
            plid.IsUnique = false;
            plid.IsInter = true;
            plid.DefaultVal = LibraryNodeDAM.REC_ALL.ToString();
            plid.Comments = "CTDRecordDA.CTD_PLID";
            ctcds.Add(plid);
            ///////////////////////////////////////
            CustomTColDef lid = new CustomTColDef();
            lid.Attr = CTDRecordDAM.CTD_LID;
            lid.AttrType = AttrTypeConverter.IDCM_Integer;
            lid.IsRequire = true;
            lid.IsUnique = false;
            lid.IsInter = true;
            lid.DefaultVal = LibraryNodeDAM.REC_ALL.ToString();
            lid.Comments = "CTDRecordDA.CTD_LID";
            ctcds.Add(lid);

            return ctcds;
        }

        /// <summary>
        /// 用户自定义表字段声明缓冲池
        /// </summary>
        protected static Dictionary<string, CustomTColDef> ctcdCache = new Dictionary<string, CustomTColDef>();

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
