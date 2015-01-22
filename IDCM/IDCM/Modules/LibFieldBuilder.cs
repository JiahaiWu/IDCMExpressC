using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Data.Base;
using IDCM.Data.Base.Utils;
using IDCM.Service.BGHandler;
using System.Drawing;
using IDCM.Core;
using IDCM.Service.Common;
using System.Configuration;

namespace IDCM.Modules
{
    class LibFieldBuilder
    {
        #region 构造&析构
        public LibFieldBuilder(ComboBox comboBox_templ, DataGridView dgv_fields)
        {
            this.templatels = comboBox_templ;
            this.fieldsDGV = dgv_fields;
            loadTemplates();
        }
        ~LibFieldBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            this.templatels = null;
            this.fieldsDGV = null;
        }
        #endregion
        #region 实例对象保持部分
        //private Object SYNC_ROOT = new Object();
        private bool customUpdated = false;
        private LinkedList<CustomTColDef> customTCDList = null;
        private Dictionary<string, List<CustomTColDef>> templDict = null;

        private ComboBox templatels = null;

        public ComboBox Templatels
        {
            get { return templatels; }
        }
        private DataGridView fieldsDGV = null;

        public DataGridView FieldsDGV
        {
            get { return fieldsDGV; }
        }
        #endregion

        
        protected void loadTemplates(){
            customTCDList = LocalRecordMHub.loadCustomAll(DataSourceHolder.DataSource);
            templDict = LocalRecordMHub.getTableTemplateDef();
            templatels.Items.Clear();
            templatels.Items.Add("[Current Setting]");
            foreach (string name in templDict.Keys)
            {
                templatels.Items.Add(name);
            }
            DataGridViewComboBoxColumn dgvcbc = fieldsDGV.Columns["attrType"] as DataGridViewComboBoxColumn;
            dgvcbc.Items.Clear();
            dgvcbc.Items.Add(AttrTypeConverter.IDCM_Number);
            dgvcbc.Items.Add(AttrTypeConverter.IDCM_String);
            dgvcbc.Items.Add(AttrTypeConverter.IDCM_Integer);
            dgvcbc.Items.Add(AttrTypeConverter.IDCM_link);
            dgvcbc.Items.Add(AttrTypeConverter.IDCM_Enum);
            dgvcbc.Items.Add(AttrTypeConverter.IDCM_Date);
            //selectTemplate(0);
        }
        public void selectTemplate(int sIndex)
        {
            lock (ShareSyncLockers.LocalDataGridView_Lock)
            {
                fieldsDGV.Rows.Clear();
                ICollection<CustomTColDef> ctcds = sIndex>0?templDict[templatels.Items[sIndex].ToString()]:customTCDList.ToList();
                foreach (CustomTColDef ctcd in ctcds)
                {
                    List<object> vals = new List<object>();
                    vals.Add(CVNameConverter.toViewName(ctcd.Attr));
                    vals.Add(ctcd.AttrType);
                    vals.Add(ctcd.IsUnique);
                    vals.Add(ctcd.DefaultVal);
                    vals.Add(ctcd.Restrict);
                    vals.Add(ctcd.IsRequire);
                    vals.Add(ctcd.Comments);
                    int ridx=fieldsDGV.Rows.Add(vals.ToArray());
                    if (sIndex > 0 && existNameInCustomTCD(ctcd.Attr))
                    {
                        (fieldsDGV.Rows[ridx].Cells["Reuse"] as DataGridViewCheckBoxCell).Value = true;
                    }
                }
            }
            if(templatels.SelectedIndex != sIndex)
                templatels.SelectedIndex = sIndex;
            if (sIndex == 0)
            {
                fieldsDGV.AllowUserToAddRows = true;
                fieldsDGV.Font = new Font(fieldsDGV.Font, FontStyle.Regular);
                fieldsDGV.DefaultCellStyle.BackColor = Color.AliceBlue;
                fieldsDGV.DefaultCellStyle.Font = fieldsDGV.Font;
                fieldsDGV.ForeColor = Color.Black;
                fieldsDGV.Columns["Delete"].Visible = true;
                fieldsDGV.Columns["Up"].Visible = true;
                fieldsDGV.Columns["Down"].Visible = true;
                fieldsDGV.Columns["Reuse"].Visible = false;
                fieldsDGV.Columns["Overwrite"].Visible = false;
                foreach (DataGridViewColumn dgvc in fieldsDGV.Columns)
                {
                    dgvc.ReadOnly = false;
                }
            }
            else
            {
                fieldsDGV.CancelEdit();
                fieldsDGV.AllowUserToAddRows = false;
                fieldsDGV.Font = new Font(fieldsDGV.Font, FontStyle.Italic);
                fieldsDGV.DefaultCellStyle.Font = fieldsDGV.Font;
                fieldsDGV.DefaultCellStyle.BackColor = Color.WhiteSmoke;
                fieldsDGV.ForeColor = Color.Gray;
                fieldsDGV.Columns["Delete"].Visible = false;
                fieldsDGV.Columns["Up"].Visible = false;
                fieldsDGV.Columns["Down"].Visible = false;
                fieldsDGV.Columns["Reuse"].Visible = true;
                fieldsDGV.Columns["Overwrite"].Visible = true;
                foreach (DataGridViewColumn dgvc in fieldsDGV.Columns)
                {
                    dgvc.ReadOnly = true;
                }
                fieldsDGV.Columns["Reuse"].ReadOnly = false;
                fieldsDGV.Columns["Overwrite"].ReadOnly = false;
                fieldsDGV.Columns["Reuse"].DefaultCellStyle.Font = new Font(fieldsDGV.Font, FontStyle.Regular);
                fieldsDGV.Columns["Overwrite"].DefaultCellStyle.Font = new Font(fieldsDGV.Font, FontStyle.Regular);
                fieldsDGV.Columns["Reuse"].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                fieldsDGV.Columns["Overwrite"].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
            }
        }
        public void appendField(DataGridViewRow dgvr,string groupName)
        {
            string attr = CVNameConverter.toDBName( dgvr.Cells["attr"].Value.ToString() );
            if (!existNameInCustomTCD(attr))
            {
                customTCDList.AddLast(templDict[groupName][dgvr.Index]);
                (dgvr.Cells["Reuse"] as DataGridViewCheckBoxCell).Value = true;
                customUpdated = true;
            }
        }
        public void removeField(DataGridViewRow dgvr)
        {
            string attr = CVNameConverter.toDBName(dgvr.Cells["attr"].Value.ToString() );
            if (existNameInCustomTCD(attr))
            {
                removeNameInCustomTCD(attr);
                customUpdated = true;
            }
            if(templatels.SelectedIndex>0)
                (dgvr.Cells["Reuse"] as DataGridViewCheckBoxCell).Value = false;
        }
        public void overwriteField(DataGridViewRow dgvr, string groupName)
        {
            string attr = CVNameConverter.toDBName(dgvr.Cells["attr"].Value.ToString());
            if (existNameInCustomTCD(attr))
            {
                removeNameInCustomTCD(attr);
            }
            customTCDList.AddLast(templDict[groupName][dgvr.Index]);
            customUpdated = true;
            (dgvr.Cells["Reuse"] as DataGridViewCheckBoxCell).Value = true;
        }
        public void submitCustomSetting()
        {
            if (checkFieldsInCustom() && customUpdated==true)
            {
                UpdateTemplateHandler uth = new UpdateTemplateHandler(DataSourceHolder.DataSource,customTCDList);
                DWorkMHub.callAsyncHandle(uth);
            }
        }
        public bool checkFieldsInCustom()
        {
            HashSet<string> attrs = new HashSet<string>();
            foreach(CustomTColDef ctcd in customTCDList)
            {
                if (attrs.Contains(ctcd.Attr))
                    return false;
                attrs.Add(ctcd.Attr);
            }
            return true;
        }
        private bool existNameInCustomTCD(string attr)
        {
            foreach (CustomTColDef ctcd in customTCDList)
            {
                if (ctcd.Attr.Equals(attr))
                    return true;
            }
            return false;
        }
        private void removeNameInCustomTCD(string attr)
        {
            lock (ShareSyncLockers.LocalDataGridView_Lock)
            {
                int count = customTCDList.Count-1;
                while (count >=0)
                {
                    if (customTCDList.ElementAt(count).Attr.Equals(attr))
                    {
                        customTCDList.Remove(customTCDList.ElementAt(count));
                        break;
                    }
                    --count;
                }
            }
        }
    }
}
