using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Common
{
    public class ErrorNote
    {
        public ErrorNote(Type type, string errorMsg)
        {
            this.source = type.Name;
            this.errorMsg = errorMsg;
            this.timestamp=DateTime.Now.ToShortTimeString();
        }
        public ErrorNote(Exception ex,string errorMsg=null)
        {
            this.source = ex.Source;
            this.errorMsg =errorMsg!=null?errorMsg: ex.Message;
            this.trace=ex.StackTrace;
        }
        public override string ToString()
        {
            StringBuilder strbuilder=new StringBuilder();
            strbuilder.Append("[Source] ").Append(source).Append("\n[Message] ").Append(errorMsg);
            if(this.trace!=null)
            {
                strbuilder.Append("\n[StackTrace] ").Append(trace);
            }
            strbuilder.Append("\n@TS=").Append(timestamp);
            return strbuilder.ToString();
        }
        public readonly string timestamp; 
        public readonly string source;
        public readonly string errorMsg;
        public readonly string trace;
    }
}
