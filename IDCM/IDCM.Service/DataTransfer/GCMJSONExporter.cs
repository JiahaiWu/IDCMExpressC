using IDCM.Data.Base;
using IDCM.Service.Common;
using IDCM.Service.Utils;
using Newtonsoft.Json;
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
        public bool exportJSONList(string xpath,DataTable dt)
        {
            StringBuilder strbuilder = new StringBuilder();
            using (FileStream fs = new FileStream(xpath, FileMode.Create))
            {
                string json = JsonConvert.SerializeObject(dt);
                strbuilder.Append(json);
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

        internal bool exportJSONList(string xpath, DataGridViewSelectedRowCollection selectedRows,GCMSiteMHub gcmSiteHolder)
        {
            GCMDataMHub gcmDataHub = new GCMDataMHub();
            if (selectedRows == null || gcmSiteHolder == null) 
                return false;
            using (FileStream fs = new FileStream(xpath, FileMode.Create))
            {
                foreach (DataGridViewRow dgvRow in selectedRows)
                {
                    string strainId = DGVUtil.getCellValue(dgvRow.Cells[1]);
                    StrainView sv = gcmDataHub.strainViewQuery(gcmSiteHolder, strainId);
                    Dictionary<string, object> dictionay=sv.ToDictionary();
                    
                }
            }
            //gcmDataHub.strainViewQuery(gcmSiteHolder, strainid);
            return true;
        }

    }
}
