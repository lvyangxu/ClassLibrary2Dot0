using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class DoRandom
    {
        /// <summary>
        /// 使用默认字典构造指定长度的随机字符串
        /// </summary>
        /// <param name="stringLength">随机字符串长度</param>
        /// <returns>如果长度合法返回随机字符串,否则返回null</returns>
        public string buildRandomString(int stringLength) {
            if (stringLength < 1)
            {
                return null;
            }
            string dictionary = "0123456789+*-abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random Random1 = new Random();
            string result = "";
            for (int i = 0; i < stringLength; i++)
            {
                int index = Random1.Next(dictionary.Length);
                result = result+dictionary[index];
            }
            return result;
        }
    }
}
