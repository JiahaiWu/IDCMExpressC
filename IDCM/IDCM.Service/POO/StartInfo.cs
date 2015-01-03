using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Service.POO
{
    public class StartInfo
    {
        public string LoginName { get; set; }
        public string Location { get; set; }
        public string GCMPassword { get; set; }
        public bool rememberPassword { get; set; }
        public bool asDefaultWorkspace { get; set; }
    }
}
