using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.Service.UIM
{
    class StrainTreesLoader
    {
        private List<TreeView> treeViews;

        private string[] getStrainID(Dictionary<string, int> strainID_cellIndex_Map)
        {
            string[] idArray = new string[strainID_cellIndex_Map.Count];
            int i = 0;
            foreach (KeyValuePair<string, int> kvp in strainID_cellIndex_Map)
            {
                idArray[i++] = kvp.Key;
            }
            return idArray;
        }
    }
}
