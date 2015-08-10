using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class DecodeAndEncode
    {
        /// <summary>
        /// 以指定的编码对字符串进行md5的32位加密
        /// </summary>
        /// <param name="encodeString">待加密的字符串</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回object的数组，元素分别为加密后的字符串和异常信息</returns>
        public object[] MD5Encode32(string encodeString, string encode)
        {
            object[] result = new object[2] { null, null };
            StringBuilder sb = new StringBuilder();
            MD5 md5 = MD5.Create();
            try
            {
                Encoding Encoding1 = Encoding.GetEncoding(encode);
                byte[] buffer = md5.ComputeHash(Encoding1.GetBytes(encodeString));
                for (int i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("x2"));
                }
                result[0] = sb.ToString();
                return result;
            }
            catch(Exception e) {
                result[1] = e.Message;
                return result;
            }

        }

        /// <summary>
        /// 以指定的编码对字符串进行url加密
        /// </summary>
        /// <param name="encodeString">待加密的字符串</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回object的数组，元素分别为加密后的字符串和异常信息</returns>
        public string[] urlEncode(string encodeString, string encode) {
            string[] result = new string[2] { null, null };
            try
            {
                Encoding Encoding1 = Encoding.GetEncoding(encode);
                result[0] = System.Web.HttpUtility.UrlEncode(encodeString, Encoding1);
                return result;
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }

        }

        /// <summary>
        /// 以Utf8编码对字符串进行url加密
        /// </summary>
        /// <param name="encodeString">待加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public string urlEncodeUtf8(string encodeString)
        {
            string result = null;
            Encoding Encoding1 = Encoding.UTF8;
            result = System.Web.HttpUtility.UrlEncode(encodeString, Encoding1);
            return result;
        }


        /// <summary>
        /// 以指定的编码对字符串进行url解密
        /// </summary>
        /// <param name="decodeString">待解密的字符串</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回object的数组，元素分别为解密后的字符串和异常信息</returns>
        public object[] urlDecode(string decodeString, string encode)
        {
            object[] result = new object[2] { null, null };
            try
            {
                Encoding Encoding1 = Encoding.GetEncoding(encode);
                result[0] = System.Web.HttpUtility.UrlDecode(decodeString, Encoding1);
                return result;  
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
     
        }

        /// <summary>
        /// 以Utf8编码对字符串进行url解密
        /// </summary>
        /// <param name="decodeString">待解密的字符串</param>
        /// <returns>返回解密后的字符串</returns>
        public string urlDecodeUtf8(string decodeString)
        {
            string result = null;
            Encoding Encoding1 = Encoding.UTF8;
            result = System.Web.HttpUtility.UrlDecode(decodeString, Encoding1);
            return result;
        }


        /// <summary>
        /// 以指定的编码对字符串进行base64加密
        /// </summary>
        /// <param name="encodeString">待加密的字符串</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回object的数组，元素分别为加密后的字符串和异常信息</returns>
        public string[] base64Encode(string encodeString, string encode)
        {
            string[] result = new string[2] { null, null };
            try
            {
                Encoding Encoding1 = Encoding.GetEncoding(encode);
                byte[] buffer = Encoding1.GetBytes(encodeString);
                result[0] = System.Convert.ToBase64String(buffer);
                return result;
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }

        }

        /// <summary>
        /// 以指定的编码对字符串进行base64解密
        /// </summary>
        /// <param name="decodeString">待解密的字符串</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回object的数组，元素分别为解密后的字符串和异常信息</returns>
        public object[] base64Decode(string decodeString,string encode) {
            object[] result = new object[2] { null, null };
            try
            {
                Encoding Encoding1 = Encoding.GetEncoding(encode);
                result[0] = Encoding1.GetString(Convert.FromBase64String(decodeString));
                return result;  
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
      
        }



        /// <summary>
        /// 异或加密
        /// </summary>
        /// <param name="code"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string encrypt(string code, string key)
        {
            if (code == "" || key == "")
            {
                return "";
            }

            byte[] b_code = Encoding.ASCII.GetBytes(code);
            byte[] b_key = Encoding.ASCII.GetBytes(key);

            StringBuilder result = new StringBuilder();

            int j = 0;
            for (int i = 0; i < b_code.Length; i++)
            {
                b_code[i] = (byte)(b_code[i] ^ b_key[j]);
                j = (++j) % (b_key.Length);

            }

            return Encoding.ASCII.GetString(b_code).ToString(); //返回密码结果
        }




        /// <summary>
        /// 进行数字签名
        /// </summary>
        /// <param name="macidfa"></param>
        /// <returns></returns>
        public int makeSignData(string[] macidfa)
        {
            int mod = 5533;
            int value = 0;
            for (int i = 0; i < macidfa.Length; i += 1)
            {
                int factor = signToNumber(macidfa[i]);
                value = (value * value + factor) % mod;
            }
            return value;
        }

        public int signToNumber(string str)
        {
            int number = 0;
            for (int i = 0; i < str.Length; i += 1)
            {
                char fac = str[i];
                switch (fac)
                {
                    case '0': ; break;
                    case '1': number += 1; break;
                    case '2': number += 2; break;
                    case '3': number += 3; break;
                    case '4': number += 4; break;
                    case '5': number += 5; break;
                    case '6': number += 6; break;
                    case '7': number += 7; break;
                    case '8': number += 8; break;
                    case '9': number += 9; break;
                    case 'a':
                    case 'A':
                        number += 10; break;
                    case 'b':
                    case 'B':
                        number += 11; break;
                    case 'c':
                    case 'C':
                        number += 12; break;
                    case 'd':
                    case 'D':
                        number += 13; break;
                    case 'e':
                    case 'E':
                        number += 14; break;
                    case 'f':
                    case 'F':
                        number += 15; break;
                }
            }
            return number;
        }
    }
}
