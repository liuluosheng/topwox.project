using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net;
namespace Topwox.Core.Utility
{
    public class EncryptedQueryString : Dictionary<string, string>
    {
        protected byte[] KeyBytes = { 0x11, 0x12, 0x13, 0x20, 0x22, 0x26, 0x37, 0x48 };
        protected string KeyString = "JPz7VBIq0_ztV2ZBTic3AyLR";
        protected string ChecksumKey = "_$*(";

        public EncryptedQueryString()
        {
        }

        public EncryptedQueryString(string encryptedData)
        {
            var data = Decrypt(encryptedData);
            string checksum = null;
            var args = data.Split('&');
            foreach (var arg in args)
            {
                var i = arg.IndexOf('=');
                if (i == -1) continue;
                var key = arg.Substring(0, i);
                var value = arg.Substring(i + 1);
                if (key == ChecksumKey)
                {
                    checksum = value;
                }
                else
                {
                    Add(WebUtility.UrlDecode(key), WebUtility.UrlDecode(value));
                }
            }
            if (checksum == null || checksum != ComputeChecksum())
            {
                Clear();
            }
        }

        public override string ToString()
        {
            var content = new StringBuilder();
            foreach (var key in Keys)
            {
                if (content.Length > 0)
                {
                    content.Append('&');
                }
                content.AppendFormat("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(base[key]));
            }
            if (content.Length > 0)
            {
                content.Append('&');
            }
            content.AppendFormat("{0}={1}", ChecksumKey, ComputeChecksum());
            return Encrypt(content.ToString());
        }

        protected string ComputeChecksum()
        {
            var checksum = 0;
            foreach (var pair in this)
            {
                checksum += pair.Key.Sum(c => c - '0');
                checksum += pair.Value.Sum(c => c - '0');
            }
            return checksum.ToString("X");
        }
        protected string Encrypt(string text)
        {
            try
            {
                var keyData = Encoding.UTF8.GetBytes(KeyString.Substring(0, 24));
                var des = TripleDES.Create();
                var textData = Encoding.UTF8.GetBytes(text);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms,
                  des.CreateEncryptor(keyData, KeyBytes), CryptoStreamMode.Write);
                cs.Write(textData, 0, textData.Length);
                cs.FlushFinalBlock();
                return GetString(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }
        protected string Decrypt(string text)
        {
            try
            {
                var keyData = Encoding.UTF8.GetBytes(KeyString.Substring(0, 24));
                var des = TripleDES.Create();
                var textData = GetBytes(text);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, des.CreateDecryptor(keyData, KeyBytes), CryptoStreamMode.Write);
                cs.Write(textData, 0, textData.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }
        protected string GetString(byte[] data)
        {
            var results = new StringBuilder();
            foreach (var b in data)
            {
                results.Append(b.ToString("X2"));
            }
            return results.ToString();
        }

        protected byte[] GetBytes(string data)
        {
            var results = new byte[data.Length / 2];
            for (var i = 0; i < data.Length; i += 2)
            {
                results[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
            }
            return results;
        }
    }
}