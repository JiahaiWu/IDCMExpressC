using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base
{
    public class CatalogNode
    {
        public const int REC_ALL = -1;
        public const int REC_UNFILED = -2;
        public const int REC_TRASH = -4;
        public const int REC_TEMP = -8;

        public enum CatalogNodeType { GroupSet = 0, Group = 1, SmartGroup = 2 };

        public CatalogNode()
        {
        }
        public CatalogNode(string _name, string _type = "Group", string _desc = "", int _order = 0)
        {
            this.name = _name;
            this.type = _type;
            this.desc = _desc;
            this.lorder = _order;
            this.pid = REC_ALL;
            this.pid = REC_UNFILED;
            this.updateTime = DateTime.Now.ToFileTimeUtc();
        }
        private long lid;

        public long Lid
        {
            get { return lid; }
            set { lid = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private long pid;

        public long Pid
        {
            get { return pid; }
            set { pid = value; }
        }

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private int lorder;

        public int Lorder
        {
            get { return lorder; }
            set { lorder = value; }
        }

        private string desc;

        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        private long updateTime;

        public long UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value; }
        }
    }
}
