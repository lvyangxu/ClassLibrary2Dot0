using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class DoHttpRequest
    {
        /// <summary>
        ///  执行一次http请求
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回string类型的数组,第一个元素为返回的值,第二个元素为异常信息,二者有一个为null</returns>
        public string[] httpRequest(string url, string encode)
        {
            string[] result = new string[2] { null, null };
            //创建web请求，并取得客户端列表
            try
            {
                WebRequest WebRequest1 = WebRequest.Create(url);
                WebResponse WebResponse1 = WebRequest1.GetResponse();
                
                Stream Stream1 = WebResponse1.GetResponseStream();
                StreamReader StreamReader1 = new StreamReader(Stream1, System.Text.Encoding.GetEncoding(encode));
                result[0] = StreamReader1.ReadToEnd();
            }
            catch (Exception e)
            {
                result[1] = e.Message;
            }
            return result;
        }

        /// <summary>
        ///  执行一次http请求
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回string类型的数组,第一个元素为返回的值,第二个元素为异常信息,二者有一个为null</returns>
        public string[] httpRequest(string url, string encode,string header)
        {
            string[] result = new string[2] { null, null };
            //创建web请求，并取得客户端列表
            try
            {
                WebRequest WebRequest1 = WebRequest.Create(url);
                WebHeaderCollection WebHeaderCollection1 = new WebHeaderCollection();

                WebHeaderCollection1.Add(header);
                WebRequest1.Headers = WebHeaderCollection1;
                WebRequest1.ContentType = "application/x-www-form-urlencoded";
                WebResponse WebResponse1 = WebRequest1.GetResponse();
                
                Stream Stream1 = WebResponse1.GetResponseStream();
                StreamReader StreamReader1 = new StreamReader(Stream1, System.Text.Encoding.GetEncoding(encode));
                result[0] = StreamReader1.ReadToEnd();
            }
            catch (Exception e)
            {
                result[1] = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 执行一次http请求,请求失败后重试一次
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回string类型的数组,第一个元素为返回的值,第二个元素为异常信息,二者有一个为null</returns>
        public string[] doHttpRequest(string url, string encode)
        {
            string[] httpResult = httpRequest(url, encode);
            if (httpResult[1] != null)
            {
                httpResult = httpRequest(url, encode);
            }
            return httpResult;
        }

        /// <summary>
        /// 执行一次http请求,请求失败后重试一次
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回string类型的数组,第一个元素为返回的值,第二个元素为异常信息,二者有一个为null</returns>
        public string[] doHttpRequest(string url, string encode,string header)
        {
            string[] httpResult = httpRequest(url, encode, header);
            if (httpResult[1] != null)
            {
                httpResult = httpRequest(url, encode, header);
            }
            return httpResult;
        }

        /// <summary>
        ///  执行一次http请求
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <param name="encode">编码格式</param>
        /// <returns>返回string类型的数组,第一个元素为返回的值,第二个元素为异常信息,二者有一个为null</returns>
        public string[] httpRequestPost(string url, string encode,string postStr)
        {
            string[] result = new string[2] { null, null };
            //创建web请求，并取得客户端列表
            try
            {
                WebRequest WebRequest1 = WebRequest.Create(url);
                WebRequest1.Method = "Post";
                WebRequest1.ContentType = "application/x-www-form-urlencoded";
                Byte[] postData = Encoding.GetEncoding(encode).GetBytes(postStr);
                WebRequest1.ContentLength = postData.Length;
                Stream Stream2 = WebRequest1.GetRequestStream();
                Stream2.Write(postData, 0, postData.Length);
                Stream2.Close();
                WebResponse WebResponse1 = WebRequest1.GetResponse();

                Stream Stream1 = WebResponse1.GetResponseStream();


                StreamReader StreamReader1 = new StreamReader(Stream1, System.Text.Encoding.GetEncoding(encode));
                result[0] = StreamReader1.ReadToEnd();
            }
            catch (Exception e)
            {
                result[1] = e.Message;
            }
            return result;
        }

    }
}
