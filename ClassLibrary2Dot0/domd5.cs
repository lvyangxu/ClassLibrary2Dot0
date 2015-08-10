using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class DoMD5
    {
        /// <summary>
        /// 获取文件的md5码
        /// </summary>
        /// <param name="fileName">文件全路径</param>
        /// <returns>返回string数组,元素分别为md5码和异常信息</returns>
        public  string[] GetMD5HashFromFile(string fileName)
        {
            string[] result = new string[2]{null,null}; 
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                result[0] = sb.ToString();
            }
            catch (Exception e)
            {
                result[1] = e.Message;
            }

            return result;
        }

    }
}
