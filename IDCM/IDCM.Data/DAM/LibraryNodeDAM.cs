using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.DAM
{
    class LibraryNodeDAM
    {
        public const int REC_ALL = -1;
        public const int REC_UNFILED = -2;
        public const int REC_TRASH = -4;
        public const int REC_TEMP = -8;

        public enum LibraryNodeType { GroupSet = 0, Group = 1, SmartGroup = 2 };
    }
}
