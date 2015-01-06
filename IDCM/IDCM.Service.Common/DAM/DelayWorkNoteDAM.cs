using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.Data.Base;
using IDCM.Data;

namespace IDCM.Service.Common.DAM
{
    class DelayWorkNoteDAM
    {
        public static long save(DelayWorkNote ltwNote,WorkSpaceManager wsm)
        {
            if (ltwNote.JobSerialInfo != null && ltwNote.JobSerialInfo.Length > 0)
            {
                if (ltwNote.Nid < 1)
                    ltwNote.Nid = DataSupporter.nextSeqID(wsm);
                StringBuilder cmdBuilder = new StringBuilder();
                cmdBuilder.Append("insert or Replace into " + typeof(DelayWorkNote).Name);
                cmdBuilder.Append("(nid,jobType,jobSerialInfo,jobLevel,createTime,startCount,lastResult) values(");
                cmdBuilder.Append(ltwNote.Nid).Append(",'").Append(ltwNote.JobType).Append("','");
                cmdBuilder.Append(ltwNote.JobSerialInfo).Append("',").Append(ltwNote.JobLevel).Append(",").Append(ltwNote.CreateTime).Append(",");
                cmdBuilder.Append(ltwNote.StartCount).Append(",'").Append(ltwNote.LastResult).Append("')");
                DataSupporter.executeSQL(wsm, cmdBuilder.ToString());
                return ltwNote.Nid;
            }
            return -1;
        }
    }
}
