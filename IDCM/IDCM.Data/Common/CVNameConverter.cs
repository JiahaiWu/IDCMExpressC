using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Common
{
    class CVNameConverter
    {
        public static bool isViewWrapName(string attr)
        {
            if (attr == null)
                return false;
            return (attr.StartsWith("[") && attr.EndsWith("]"));
        }
        /// <summary>
        /// toDBName
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string toDBName(string attr)
        {
            if (attr == null)
                return null;
            if (attr.StartsWith("[") && attr.EndsWith("]"))
                return attr;
            return "[" + attr + "]";
        }
        /// <summary>
        /// toViewName
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string toViewName(string attr)
        {
            if (attr == null)
                return null;
            if (attr.StartsWith("[") && attr.EndsWith("]"))
                return attr.Substring(1, attr.Length - 2);
            return attr;
        }
    }
}
