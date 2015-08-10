using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClassLibrary2Dot0
{
    public class DoHTML : System.Web.HttpApplication
    {
        DoLog4Net DoLog4Net1 = new DoLog4Net();

        /// <summary>
        /// 初始化html的引用,目地是asp.net从bin目录下拷贝到应用跟目录,获得目录的html访问权限
        /// </summary>
        /// <param name="srcPath">源路径</param>
        /// <param name="desPath">目标路径</param>
        /// <returns>成功返回null,失败返回异常信息</returns>
        public string moveHTML(string srcPath,string desPath) {
            string result = null;
            //目录校验
            string srcLastChar = srcPath.Substring(srcPath.Length-1,1);
            if (srcLastChar != "/" && srcLastChar != "\\") {
                srcPath = srcPath + "/";
            }
            string desLastChar = desPath.Substring(desPath.Length - 1, 1);
            if (desLastChar != "/" && desLastChar != "\\")
            {
                desPath = desPath + "/";
            }
            if (!Directory.Exists(desPath)) {
                Directory.CreateDirectory(desPath);
            }

            DoIO DoIO1 = new DoIO();
            object[] object1 = DoIO1.getFileNameInFolder(srcPath);
            //如果获取文件列表失败则返回
            if (object1[1] != null) {
                result = (string)object1[1];
                return result;
            }
            //遍历所有文件并拷贝
            try
            {
                foreach (string fileName in (List<string>)object1[0])
                {
                    File.Copy(srcPath + fileName, desPath + fileName,true);
                    File.Delete(srcPath + fileName);
                }
                if (Directory.Exists(srcPath))
                {
                    Directory.Delete(srcPath);
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 初始化Bootstrap的引用,目地是asp.net从bin目录下拷贝到应用跟目录,获得目录的html访问权限
        /// </summary>
        /// <param name="globalServer">HttpServerUtility对象Server</param>
        /// <param name="ILog1">ILog对象</param>
        public void initBootstrap(System.Web.HttpServerUtility globalServer,log4net.ILog ILog1)
        {
            string result = moveHTML(globalServer.MapPath("bin/bootstrapCss"), globalServer.MapPath("bootstrapCss"));
            if (result != null) {
                ILog1.Info("Bootstrap初始化失败:" + result);
            }
            result = moveHTML(globalServer.MapPath("bin/bootstrapJs"), globalServer.MapPath("bootstrapJs"));
            if (result != null)
            {
                ILog1.Info("Bootstrap初始化失败:" + result);
            }
            result = moveHTML(globalServer.MapPath("bin/fonts"), globalServer.MapPath("fonts"));
            if (result != null)
            {
                ILog1.Info("Bootstrap初始化失败:" + result);
            }
        }

        /// <summary>
        /// 初始化jquery的引用,目地是asp.net从bin目录下拷贝到应用跟目录,获得目录的html访问权限
        /// </summary>
        /// <param name="globalServer">HttpServerUtility对象Server</param>
        /// <param name="ILog1">ILog对象</param>
        public void initJquery(System.Web.HttpServerUtility globalServer, log4net.ILog ILog1)
        {
            string result = moveHTML(globalServer.MapPath("bin/jqueryJs"), globalServer.MapPath("jqueryJs"));
            if (result != null)
            {
                ILog1.Info("jquery初始化失败:" + result);
            }
        }

        /// <summary>
        /// 初始化html库的引用,目地是asp.net从bin目录下拷贝到应用跟目录,获得目录的html访问权限
        /// </summary>
        /// <param name="globalServer">HttpServerUtility对象Server</param>
        /// <param name="ILog1">ILog对象</param>
        public void initHtmlLibrary(System.Web.HttpServerUtility globalServer, log4net.ILog ILog1)
        {
            string result = moveHTML(globalServer.MapPath("bin/CSSLibrary"), globalServer.MapPath("CSSLibrary"));
            if (result != null)
            {
                ILog1.Info("HtmlLibrary初始化失败:" + result);
            }
            result = moveHTML(globalServer.MapPath("bin/ImgLibrary"), globalServer.MapPath("ImgLibrary"));
            if (result != null)
            {
                ILog1.Info("HtmlLibrary初始化失败:" + result);
            }
            result = moveHTML(globalServer.MapPath("bin/JSLibrary"), globalServer.MapPath("JSLibrary"));
            if (result != null)
            {
                ILog1.Info("HtmlLibrary初始化失败:" + result);
            }
            result = moveHTML(globalServer.MapPath("bin/fonts"), globalServer.MapPath("fonts"));
            if (result != null)
            {
                ILog1.Info("HtmlLibrary初始化失败:" + result);
            }
        }
    }
}
