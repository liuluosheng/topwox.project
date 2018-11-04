using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Topwox.Core.Utility
{
    /// <summary>
    /// 使用des算法加密解密字符串
    /// </summary>
    public sealed class DataEncryptHelper
    {
        private static readonly string encryptKey = "q0X%";
        private static readonly string encryptValue = "z$D1";

        private DataEncryptHelper() { }

        /// <summary> 
        /// 加密字符串  
        /// </summary> 
        /// <param name="str"></param> 
        /// <returns></returns> 
        public static string Encrypt(string str)
        {
            var descsp = TripleDES.Create();
            byte[] key = Encoding.Unicode.GetBytes(encryptKey);
            byte[] value = Encoding.Unicode.GetBytes(encryptValue);
            byte[] data = Encoding.Unicode.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, descsp.CreateEncryptor(key, value), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            cs.Dispose();
            ms.Flush();
            var result = Convert.ToBase64String(ms.ToArray());
            ms.Dispose();
            return result;
        }

        /// <summary> 
        /// 解密字符串  
        /// </summary> 
        /// <param name="str"></param> 
        /// <returns></returns> 
        public static string Decrypt(string str)
        {
            try
            {
                var descsp = TripleDES.Create();
                byte[] key = Encoding.Unicode.GetBytes(encryptKey);
                byte[] value = Encoding.Unicode.GetBytes(encryptValue);
                byte[] data = Convert.FromBase64String(str);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, descsp.CreateDecryptor(key, value), CryptoStreamMode.Write);
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                ms.Flush();
                var result = Encoding.Unicode.GetString(ms.ToArray());
                ms.Dispose();
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}