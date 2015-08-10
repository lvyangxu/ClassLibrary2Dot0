using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ClassLibrary2Dot0
{


    public class DoHttpContext : System.Web.HttpApplication
    {
        DoLog4Net DoLog4Net1 = new DoLog4Net();
        DecodeAndEncode DecodeAndEncode1 = new DecodeAndEncode();

        /// <summary>
        /// 检查http请求的参数,并返回string
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="para">参数名称</param>
        /// <returns>如果参数不存在，返回空字符串</returns>
        public string getRequestPara(HttpContext context,string para)
        {
            if (context.Request[para] == null)
            {
                return "";
            }
            else {
                return context.Request[para].ToString();
            }            
        }


        /// <summary>
        /// 检查http请求的参数,如果参数不存在或者为空,则停止该页执行,并回复给客户端指定的内容
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="para">参数名称</param>
        /// <param name="replyMessage">回复客户端的内容</param>
        public void checkNullParaAndReplyClient(HttpContext context, string para,string replyMessage)
        {
            para = getRequestPara(context,para);
            if (para == "") {
                context.Response.ContentType = "text/plain";
                context.Response.Write(replyMessage);
                context.Response.End();
            }
        }

        /// <summary>
        /// 检查http请求的参数,如果参数不存在或者为空,则停止该页执行,并回复给客户端指定的内容
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="paraArray">参数名称数组</param>
        /// <param name="replyMessage">回复客户端的内容</param>
        public void checkNullParaAndReplyClient(HttpContext context, string[] paraArray, string replyMessage)
        {
            foreach (string para in paraArray)
            {
                string para1 = getRequestPara(context, para);
                if (para1 == "")
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(replyMessage);
                    context.Response.End();
                }
            }
        }


        /// <summary>
        /// 停止该页执行,并回复给客户端指定的内容
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="replyMessage">回复客户端的内容</param>
        public void replyClient(HttpContext context, string replyMessage)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(replyMessage);
            context.Response.End();     
        }

        /// <summary>
        /// 停止该页执行,并回复给客户端指定的内容
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="replyMessage">回复客户端的内容</param>
        public void replyClient200(HttpContext context, string replyMessage)
        {
            context.Response.ContentType = "text/plain";
            context.Response.StatusCode = 200;
            context.Response.Write(replyMessage);
            context.Response.End();
        }


        /// 停止该页执行,并回复给客户端指定的内容
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="replyMessage">回复客户端的内容</param>
        public void replyClientJson(HttpContext context, string replyMessage)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(replyMessage);
            context.Response.End();
        }



        /// <summary>
        /// 停止该页执行,并回复给客户端指定的内容
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="replyMessage">回复客户端的内容</param>
        public void replyClientXmlType(HttpContext context, string replyMessage)
        {
            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(replyMessage);
            context.Response.End();
        }


        /// <summary>
        /// 检查http请求的参数,如果参数不存在或者为空,则打印日志并停止该页执行,并回复给客户端指定的内容
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="para">参数名称</param>
        /// <param name="Ilog1">ILog对象</param>
        /// <param name="replyMessage">回复给客户端的内容</param>
        /// <returns>返回参数的值</returns>
        public string getParaWithLogAndReply(HttpContext context,string para,log4net.ILog Ilog1,string replyMessage) {
            para = getRequestPara(context, para);
            if (para == "") {
                Ilog1.Info(context.Request.Url+"该请求的参数"+para+"为空,停止执行该页面");
                replyClient(context, replyMessage);
            }
            return para;
        }

        /// <summary>
        /// 根据accout和password参数验证登录并回复客户端消息,成功则设置account的session为当前的account的值,有效期为30天,登录验证页面需要实现接口IRequiresSessionState
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serverAccount"></param>
        /// <param name="serverPassword"></param>
        public void checkLogin(HttpContext context, string serverAccount, string serverPassword)
        {
            string account = getRequestPara(context, "account");
            string password = getRequestPara(context, "password");
            if (account == serverAccount && password == serverPassword)
            { 
                context.Session["account"] = account;
                context.Session["password"] = password;
                context.Session.Timeout = 300;
                replyClient(context, "{\"success\":\"true\",\"message\":\"\"}");
            }
            else
            {
                replyClient(context, "{\"success\":\"false\",\"message\":\"errorAccount\"}");
            }  
        }

        /// <summary>
        /// 根据accout和password参数验证登录,成功则设置account的session为当前的account的值,有效期为30天,登录验证页面需要实现接口IRequiresSessionState
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serverAccount"></param>
        /// <param name="serverPassword"></param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool reLogin(HttpContext context, string serverAccount, string serverPassword)
        {
            string account = getCookie(context, "account");
            string password = getCookie(context, "password");
            account = (string)DecodeAndEncode1.base64Decode(account, "UTF-8")[0];
            password = (string)DecodeAndEncode1.base64Decode(password, "UTF-8")[0];
            if (account == serverAccount && password == serverPassword)
            {
                context.Session["account"] = account;
                context.Session["password"] = password;
                context.Session.Timeout = 300;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 打印出asp.net服务器的错误
        /// </summary>
        /// <param name="globalServer">System.Web.HttpServerUtility对象</param>
        /// <param name="Ilog1">log4net.ILog对象</param>
        public void doAspDotNetError(System.Web.HttpServerUtility globalServer, log4net.ILog Ilog1)
        {
            Exception lastExcetion = globalServer.GetLastError().GetBaseException();
            Ilog1.Info("应用发生异常:" + lastExcetion.Message);
        }

        /// <summary>
        /// 根据名称获取cookie
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="cookieName">cookie名称</param>
        /// <returns>返回cookie的值,如果cookie为null,返回空字符串</returns>
        public string getCookie(HttpContext context,string cookieName) {
            if (context.Request.Cookies[cookieName] == null)
            {
                return "";
            }
            else {
                return context.Request.Cookies[cookieName].Value;
            }
        }

        /// <summary>
        /// 根据名称获取session
        /// </summary>
        /// <param name="context">HttpContext对象</param>
        /// <param name="sessionName)">session名称</param>
        /// <returns>返回session的值,如果session为null,返回空字符串</returns>
        public string getSession(HttpContext context, string sessionName)
        {
            if (context.Session[sessionName] == null)
            {
                return "";
            }
            else
            {
                return context.Session[sessionName].ToString();
            }
        }
    }
}
