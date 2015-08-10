using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class DoHttpListener
    {
        /// <summary>
        /// 创建HttpListener,并监听指定的url地址(必须以 / 正斜杠结尾)
        /// </summary>
        /// <param name="prefixes">url地址的数组</param>
        /// <returns></returns>
        public string createHttpListener(string[] prefixes,string responseString)
        {
            string result = null;
            
            // 检查系统是否支持
            if (!HttpListener.IsSupported)
            {
                result = "HttpListenerIsNotSupported";
                return result;
            }

            // 创建监听器.            
            HttpListener listener = new HttpListener();
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            // 开始监听
            listener.Start();
            while (true)
            {
                // 注意: GetContext 方法将阻塞线程，直到请求到达
                HttpListenerContext context = listener.GetContext();
                // 取得请求对象
                HttpListenerRequest request = context.Request;

                // 取得回应对象
                HttpListenerResponse response = context.Response;

                // 设置回应头部内容，长度，编码
                response.ContentLength64 = System.Text.Encoding.UTF8.GetByteCount(responseString);
                response.ContentType = "text/html; charset=UTF-8";
                // 输出回应内容
                Stream Stream1 = response.OutputStream;
                StreamWriter StreamWriter1 = new StreamWriter(Stream1);
                StreamWriter1.Write(responseString);
                StreamWriter1.Flush();
                StreamWriter1.Close();


            }
            // 关闭服务器
       //     listener.Stop();
        }
    }
}
