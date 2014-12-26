using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Common.Generator
{
    /// <summary>
    /// 该序列编码区别于Base64标准编码序列,以此实例生成的序列适应多数场景而无须转义，可直接应用于URL、正则表达式、文件名定义等情形。
    /// @author JiahaiWu
    /// </summary>
    class Base64CShared : Base64SharedI
    {
        protected static char[] chars = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 
            'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '-', '~', '_' };

        public char charAt(int index)
        {
            return chars[index];
        }

        public int indexOf(char ch)
        {
            int i = 0;
            while (i < 64)
            {
                if (ch == chars[i])
                    return i;
            }
            return -1;
        }

        public char pad()
        {
            return chars[64];
        }
    }
}
