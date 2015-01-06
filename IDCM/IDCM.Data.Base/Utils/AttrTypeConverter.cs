using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Base.Utils
{
    /// <summary>
    /// 表属性值类型枚举定义
    /// </summary>
    public class AttrTypeConverter
    {
        /// <summary>
        /// IDCM 表定义基本类型-数值类型
        /// </summary>
        public const string IDCM_Number = "number";
        /// <summary>
        /// IDCM 表定义基本类型-整数类型
        /// </summary>
        public const string IDCM_Integer = "integer";
        /// <summary>
        /// IDCM 表定义基本类型-字符串
        /// </summary>
        public const string IDCM_String = "string";
        /// <summary>
        /// IDCM 表定义基本类型-枚举类型
        /// </summary>
        public const string IDCM_Enum = "enum";
        /// <summary>
        /// IDCM 表定义基本类型-日期
        /// </summary>
        public const string IDCM_Date = "date";
        /// <summary>
        /// IDCM 表定义基本类型-文件路径
        /// </summary>
        public const string IDCM_link = "link";

        /// <summary>
        /// 从C#类型定义映射转换至系统表属性类型，如映射无效则返回null.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string getAttrIDCMType(Type type)
        {
            if (type.Namespace.Equals("System"))
            {
                if (type.Name.Equals("String") || type.Name.Equals("Char"))
                {
                    return IDCM_String;
                }
                if (type.Name.StartsWith("Int"))
                {
                    return IDCM_Integer;
                }
                if (!type.Name.Equals("Object"))
                {
                    return IDCM_Number;
                }
            }
            return null;
        }
        /// <summary>
        /// 从系统表属性类型映射转换至C#类型，如映射无效则返回string类型
        /// </summary>
        /// <param name="idcm_type"></param>
        /// <returns></returns>
        public static Type getAttrCSharpType(string idcm_type)
        {
            if (idcm_type.Equals(IDCM_Integer))
            {
                return typeof(Int64);
            }
            else if (idcm_type.Equals(IDCM_Number))
            {
                return typeof(Double);
            }
            else
            {
                return typeof(String);
            }
        }


        public static string getSQLiteType(string attrType)
        {
            if (attrType.Equals(IDCM_Integer))
            {
                return "Integer";
            }
            else if (attrType.Equals(IDCM_Number))
            {
                return "Real";
            }
            else
            {
                return "Text";
            }
        }

        /// <summary>
        /// 对目标字符串类型进行一般类型的数据格式化
        /// </summary>
        /// <param name="cellVal"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object formatStrVal(string cellVal, Type type)
        {
            if (type.Namespace.Equals("System"))
            {
                if (type.Name.StartsWith("Int"))
                {
                    return Convert.ToInt64(cellVal);
                }
                else if (type.Name.Equals("Double") || type.Name.Equals("Float"))
                {
                    return Convert.ToDouble(cellVal);
                }
                return cellVal;
            }
            else
                return null; //it's illegal
        }
    }
}
