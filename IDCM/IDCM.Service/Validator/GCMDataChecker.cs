using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IDCM.Service.Validator
{
    class GCMDataChecker
    {
        public static bool checkForPublish(ref string xmlData)
        {
            string pattern1 = @"(<\w+_temperature_for_growth>)([^<]*)";
            Regex reg1 = new Regex(pattern1, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string replacement1 = "${1}10";
            xmlData = reg1.Replace(xmlData, replacement1);

            string pattern2 = @"(<date_of_isolation>)([^<]*)";
            string replacement2 = "${1}2014-12-11";
            Regex reg2 = new Regex(pattern2, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            xmlData = reg2.Replace(xmlData, replacement2);

            return true;
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
