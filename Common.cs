using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Beinet.cn.DataSync
{
    public static class Common
    {
        #region 3DES加解密
        /// <summary>
        /// 3DES加解密的默认密钥, 前8位作为向量
        /// </summary>
        const string DES3_KEY = "beinet.cn%G1&73#;0(=+)`!";

        /// <summary>
        /// 使用指定的key和iv，加密input数据
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">密钥，必须为24位长度</param>
        /// <param name="iv">微量，必须为8位长度</param>
        /// <returns></returns>
        public static string TripleDES_Encrypt(string input, string key = null, string iv = null)
        {
            key = key ?? DES3_KEY;
            int lenKey = 24 - key.Length;
            if (lenKey < 0)
                key = key.Substring(0, 24);         // 太长时，只取前24位
            else if (lenKey > 0)
                key += DES3_KEY.Substring(0, lenKey); // 太短时，补足24位

            iv = iv ?? key.Substring(0, 8);
            int lenIV = 8 - iv.Length;
            if (lenIV < 0)
                iv = iv.Substring(0, 24);           // 太长时，只取前8位
            else if (lenIV > 0)
                iv += iv.Substring(0, lenIV);       // 太短时，补足8位

            byte[] arrKey = Encoding.UTF8.GetBytes(key);
            byte[] arrIV = Encoding.UTF8.GetBytes(iv);

            // 获取加密后的字节数据
            byte[] arrData = Encoding.UTF8.GetBytes(input);
            byte[] result = TripleDesEncrypt(arrKey, arrIV, arrData);

            // 转换为16进制字符串
            StringBuilder ret = new StringBuilder();
            foreach (byte b in result)
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// 使用指定的key和iv，解密input数据
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">密钥，必须为24位长度</param>
        /// <param name="iv">微量，必须为8位长度</param>
        /// <returns></returns>
        public static string TripleDES_Decrypt(string input, string key = null, string iv = null)
        {
            key = key ?? DES3_KEY;
            int lenKey = 24 - key.Length;
            if (lenKey < 0)
                key = key.Substring(0, 24);         // 太长时，只取前24位
            else if (lenKey > 0)
                key += DES3_KEY.Substring(0, lenKey); // 太短时，补足24位

            iv = iv ?? key.Substring(0, 8);
            int lenIV = 8 - iv.Length;
            if (lenIV < 0)
                iv = iv.Substring(0, 24);           // 太长时，只取前8位
            else if (lenIV > 0)
                iv += iv.Substring(0, lenIV);       // 太短时，补足8位

            byte[] arrKey = Encoding.UTF8.GetBytes(key);
            byte[] arrIV = Encoding.UTF8.GetBytes(iv);

            // 获取加密后的字节数据
            int len = input.Length / 2;
            byte[] arrData = new byte[len];
            for (int x = 0; x < len; x++)
            {
                int i = (Convert.ToInt32(input.Substring(x * 2, 2), 16));
                arrData[x] = (byte)i;
            }

            byte[] result = TripleDesDecrypt(arrKey, arrIV, arrData);
            return Encoding.UTF8.GetString(result);
        }


        #region TripleDesEncrypt加密(3DES加密)
        /// <summary>
        /// 3Des加密，密钥长度必需是24字节
        /// </summary>
        /// <param name="key">密钥字节数组</param>
        /// <param name="iv">向量字节数组</param>
        /// <param name="source">源字节数组</param>
        /// <returns>加密后的字节数组</returns>
        private static byte[] TripleDesEncrypt(byte[] key, byte[] iv, byte[] source)
        {
            TripleDESCryptoServiceProvider dsp = new TripleDESCryptoServiceProvider();
            dsp.Mode = CipherMode.CBC; // 默认值
            dsp.Padding = PaddingMode.PKCS7; // 默认值
            using (MemoryStream mStream = new MemoryStream())
            using (CryptoStream cStream = new CryptoStream(mStream, dsp.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                cStream.Write(source, 0, source.Length);
                cStream.FlushFinalBlock();
                byte[] result = mStream.ToArray();
                cStream.Close();
                mStream.Close();
                return result;
            }
        }
        #endregion

        #region TripleDesDecrypt解密(3DES解密)
        /// <summary>
        /// 3Des解密，密钥长度必需是24字节
        /// </summary>
        /// <param name="key">密钥字节数组</param>
        /// <param name="iv">向量字节数组</param>
        /// <param name="source">加密后的字节数组</param>
        /// <param name="dataLen">解密后的数据长度</param>
        /// <returns>解密后的字节数组</returns>
        private static byte[] TripleDesDecrypt(byte[] key, byte[] iv, byte[] source, out int dataLen)
        {
            TripleDESCryptoServiceProvider dsp = new TripleDESCryptoServiceProvider();
            dsp.Mode = CipherMode.CBC; // 默认值
            dsp.Padding = PaddingMode.PKCS7; // 默认值
            using (MemoryStream mStream = new MemoryStream(source))
            using (CryptoStream cStream = new CryptoStream(mStream, dsp.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
                byte[] result = new byte[source.Length];
                dataLen = cStream.Read(result, 0, result.Length);
                cStream.Close();
                mStream.Close();
                return result;
            }
        }

        /// <summary>
        /// 3Des解密，密钥长度必需是24字节
        /// </summary>
        /// <param name="key">密钥字节数组</param>
        /// <param name="iv">向量字节数组</param>
        /// <param name="source">加密后的字节数组</param>
        /// <returns>解密后的字节数组</returns>
        private static byte[] TripleDesDecrypt(byte[] key, byte[] iv, byte[] source)
        {
            int dataLen;
            byte[] result = TripleDesDecrypt(key, iv, source, out dataLen);

            if (result.Length != dataLen)
            {
                // 如果数组长度不是解密后的实际长度，需要截断多余的数据，用来解决Gzip的"Magic byte doesn't match"的问题
                byte[] resultToReturn = new byte[dataLen];
                Array.Copy(result, resultToReturn, dataLen);
                return resultToReturn;
            }
            else
                return result;
        }
        #endregion

        #endregion


        //// 获取指定事件的绑定的全部委托
        //void ttt()
        //{
        //    PropertyInfo propertyInfo = (typeof(System.Windows.Forms.Button)).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
        //    EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(btn_Retrive, null);
        //    FieldInfo fieldInfo = (typeof(Control)).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
        //    if (fieldInfo != null)
        //    {
        //        Delegate d = eventHandlerList[fieldInfo.GetValue(null)];
        //        if (d != null)
        //        {
        //            foreach (Delegate temp in d.GetInvocationList())
        //            {
        //               // btn_Retrive -= temp;
        //            }
        //        }
        //    }
        //}

    }
}
