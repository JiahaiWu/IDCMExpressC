using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.Data.Common.Generator
{
    /// <summary>
    /// 该序列编码符合Base64标准编码序列,以此实例生成的序列适应多数直观阅读场景，在应用于URL、正则表达式、文件名等情形中多需转义。
    /// @author JiahaiWu
    /// </summary>
    abstract class Base64Shared : Base64SharedI
    {
        protected static char[] chars = new char[65] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 
            'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/', '=' };

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
