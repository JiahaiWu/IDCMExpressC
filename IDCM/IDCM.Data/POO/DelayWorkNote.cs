using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.POO
{
    class DelayWorkNote
    {
        public DelayWorkNote()
        {
        }
        public DelayWorkNote(string type)
        {
            this.createTime = System.DateTime.Now.Ticks;
            this.jobLevel = 1;
            this.jobType = type;
        }
        private long nid;

        public long Nid
        {
            get { return nid; }
            set { nid = value; }
        }

        private string jobType;

        public string JobType
        {
            get { return jobType; }
            set { jobType = value; }
        }
        private string jobSerialInfo;

        public string JobSerialInfo
        {
            get { return jobSerialInfo; }
            set { jobSerialInfo = value; }
        }
        private long createTime;

        public long CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        private int startCount = 0;

        public int StartCount
        {
            get { return startCount; }
            set { startCount = value; }
        }
        private string lastResult;

        public string LastResult
        {
            get { return lastResult; }
            set { lastResult = value; }
        }
        private int jobLevel = 1;

        public int JobLevel
        {
            get { return jobLevel; }
            set { jobLevel = value; }
        }
    }
}
