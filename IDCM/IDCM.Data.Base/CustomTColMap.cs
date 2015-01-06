using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base
{
    public class CustomTColMap
    {
        /// <summary>
        /// 属性名称(属性名称可以由大小写字母、数字、下划线组成，字段名大小写敏感)
        /// </summary>
        public string Attr { get; set; }
        /// <summary>
        /// 数据表映射位序(正值视为有效，负值视为隐藏)
        /// </summary>
        public int MapOrder { get; set; }
        /// <summary>
        /// 数据展示映射位序(正值视为有效，负值视为隐藏)
        /// </summary>
        public int ViewOrder { get; set; }
    }
}
