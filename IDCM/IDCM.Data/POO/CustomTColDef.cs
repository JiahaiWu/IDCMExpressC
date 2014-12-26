using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.POO
{
    class CustomTColDef
    {
        /// <summary>
        /// 属性名称(属性名称可以由大小写字母、数字、下划线组成，字段名大小写敏感)
        /// </summary>
        public string Attr { get; set; }
        /// <summary>
        /// 属性注解说明
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// 属性名称类型
        /// </summary>
        public string AttrType { get; set; }
        /// <summary>
        /// 属性值约束条件定义
        /// </summary>
        public string Restrict { get; set; }
        /// <summary>
        /// 唯一性键值约束
        /// </summary>
        public bool IsUnique { get; set; }
        /// <summary>
        /// 必选性键值约束
        /// </summary>
        public bool IsRequire { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultVal { get; set; }
        /// <summary>
        /// 内置排序标记值
        /// </summary>
        public int Corder { get; set; }
        /// <summary>
        /// 是否为内置属性
        /// </summary>
        public bool IsInter { get; set; }
    }
}
