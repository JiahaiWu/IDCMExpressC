using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Common.Generator
{
    /// <summary>
    /// 该序列具有唯一性，编码方式允许2、4、8、16、32、64进制，生成的序列适应多数场景而无须转义，包括用于URL、正则表达式、文件名等情形.<br/>
    /// 生成序列读取要求区分大小写，才能保证唯一性。<br/>
    /// @author   jiahaiwu
    /// </summary>
    class CUIDGenerator
    {
        /**
         * 基于randomUUID生成uid字符串描述形式，使用64进制表现形式，相应的字符串长度为22字节
         * 
         * @date 2011-5-31 ~ 2011-5-31
         * @author JiahaiWu
         * @return
         */
        public static String getUID()
        {
            return getUID(Radix_64);
        }
        /**
         * 基于randomUUID生成uid字符串描述形式
         * @note shift=4 将生成32位,shift=5将生成26位,shift=6将生成22位
         * @date 2011-5-31 ~ 2011-5-31
         * @author JiahaiWu
         * @param shift
         * @return
         */
        public static string getUID(int shift)
        {
            if (shift > 0 && shift < 7)
            {
                byte[] uuids = Guid.NewGuid().ToByteArray();

                ulong mostSigBits = BitConverter.ToUInt64(uuids, 0);
                ulong leastSigBits = BitConverter.ToUInt64(uuids, 8);
                return numToString(mostSigBits, shift) + numToString(leastSigBits, shift);
            }
            throw new NotImplementedException();
        }
        /**
         * 格式化数字至字符序列显示，移位值可取范围1~6，分别对2、4、8、16、32、64进制显示形式
         * 
         * @date 2011-5-31 ~ 2011-5-31
         * @author JiahaiWu
         * @param num
         * @param shift
         * @return
         */
        private static string numToString(ulong num, int shift)
        {
            StringBuilder uid = new StringBuilder();
            int i = 64;
            for (; i >= shift; i -= shift)
            {
                ulong val = num << (i - shift) >> (64 - shift);
                uid.Append(base64Shared.charAt((int)val));
            }
            if (i > 0)
            {
                ulong val = num >> (64 - i);
                uid.Append(base64Shared.charAt((int)val));
            }
            return StringUtil.Reverse(uid.ToString());
        }
        /**
         * 基于日期字符串和randomUUID生成CUID字符串，标识前缀为年月日，返回的字符串长度为30字节
         * 
         * @date 2012-5-27
         * @author JiahaiWu
         * @return
         */
        public static string getCUID()
        {
            String cuid = DateTime.Now.ToString("yyyyMMdd") + getUID();
            return cuid;
        }
        /**
         * 基于日期字符串和randomUUID生成CUID字符串，标识前缀为年月日，返回的一定长度编码的字符串
         * 
         * @date 2012-5-27
         * @author JiahaiWu
         * @return
         */
        public static String getCUID(int shift)
        {
            String cuid = DateTime.Now.ToString("yyyyMMdd") + getUID(shift);
            return cuid;
        }

        public static Base64SharedI base64Shared = new Base64CShared();
        public const int Radix_8 = 3;
        public const int Radix_16 = 4;
        public const int Radix_32 = 5;
        public const int Radix_64 = 6;
    }
}
