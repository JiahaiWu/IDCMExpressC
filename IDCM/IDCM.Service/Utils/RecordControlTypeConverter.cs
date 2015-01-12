using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.Data.Base.Utils;

namespace IDCM.Service.Utils
{
    public class RecordControlTypeConverter
    {
        /// <summary>
        /// 从系统表属性类型，获取Control的表达类型
        /// </summary>
        /// <param name="attrType"></param>
        /// <returns></returns>
        public static Type getControlType(string attrType)
        {
            if (attrType.Equals(AttrTypeConverter.IDCM_Enum))
            {
                return typeof(ComboBox);
            }
            else if (attrType.Equals(AttrTypeConverter.IDCM_Date))
            {
                return typeof(DateTimePicker);
            }
            else
            {
                return typeof(TextBox);
            }
        }
        /// <summary>
        ///  从系统表属性类型，获取DataGridViewColumn的表达类型
        /// </summary>
        /// <param name="attrType"></param>
        /// <returns></returns>
        public static Type getDGVColType(string attrType)
        {
            if (attrType.Equals(AttrTypeConverter.IDCM_Enum))
            {
                return typeof(DataGridViewComboBoxColumn);
            }
            else
            {
                return typeof(DataGridViewTextBoxColumn);
            }
        }
    }
}
