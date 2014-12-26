using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Common.Generator
{
    interface Base64SharedI
    {
        char charAt(int index);
        int indexOf(char ch);
        char pad();
    }
}
