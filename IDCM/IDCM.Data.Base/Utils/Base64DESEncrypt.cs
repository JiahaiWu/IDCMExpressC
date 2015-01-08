using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO; 

namespace IDCM.Data.Base.Utils
{
    public class Base64DESEncrypt
    {

        public Base64DESEncrypt(byte[] key)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(key != null && key.Length < 56, "The Length of key must be not short than 56 bits.");
#endif
            this.Key = key;
        }
        public Base64DESEncrypt(string key)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(key != null && key.Length < 7, "The Length of key must be not short than 7 Bytes.");
#endif
            this.Key =encoding.GetBytes(key);
        }
        public Base64DESEncrypt(string key,Encoding encoding)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(key != null && key.Length<7,"The Length of key must be not short than 7 Bytes.");
#endif
            this.encoding = encoding;
            this.Key = encoding.GetBytes(key);
        }

        /// <summary> 
        /// DES加密 -> Base64 编码
        /// </summary> 
        /// <param name="encryptString"></param> 
        /// <returns></returns> 
        public string Encrypt(string encryptString)
        {
            byte[] keyBytes = new byte[8];
            for (int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = Key[i];
            }
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary> 
        /// Base64 解码 -> DES解密 
        /// </summary> 
        /// <param name="decryptString"></param> 
        /// <returns></returns> 
        public string Decrypt(string decryptString)
        {
            byte[] keyBytes = new byte[8];
            for (int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = Key[i];
            }
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        public static Base64DESEncrypt CreateInstance(string key = null, char append = '\0')
        {
            byte[] pkey = null;
            if (key == null)
                pkey = Guid.NewGuid().ToByteArray();
            else
            {
                string skey = HashUtil.md5HexCode(Encoding.UTF8.GetBytes(key));
                pkey = Encoding.UTF8.GetBytes(skey);
            }
            return new Base64DESEncrypt(pkey);
        }


        public readonly byte[] Key = null;
        public readonly Encoding encoding = Encoding.UTF8;
    }
}
