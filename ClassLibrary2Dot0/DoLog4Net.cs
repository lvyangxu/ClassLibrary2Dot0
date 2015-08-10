using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ClassLibrary2Dot0
{
    /// <summary>
    /// 用法参照http://logging.apache.org/log4net/release/manual/configuration.html
    /// </summary>
    public class DoLog4Net : System.Web.HttpApplication
    {
        /// <summary>
        /// 以指定的路径的xml配置初始化log4net
        /// </summary>
        /// <param name="xmlPath">xml配置路径</param>
        public void initLog4net(string xmlPath)
        {            
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(xmlPath));
        }

        /// <summary>
        /// 根据名称获取log4net.ILog对象
        /// </summary>
        /// <param name="logname">log4net.ILog对象的名称</param>
        /// <returns>返回指定名称的log4net.ILog对象</returns>
        public log4net.ILog getLogger(string logname) {
            log4net.ILog log = log4net.LogManager.GetLogger(logname);
            return log;     
        }

        /// <summary>
        /// 以默认的配置App_Data/xml/log4net.xml及单一Ilog对象mylog初始化log4net
        /// </summary>
        /// <param name="globalServer">System.Web.HttpServerUtility对象</param>
        /// <returns>返回log4net.ILog对象</returns>
        public log4net.ILog initLog4netByMyDefaultConfig(System.Web.HttpServerUtility globalServer) {
            initLog4net(globalServer.MapPath("App_Data/xml/log4net.xml"));
            log4net.ILog log = getLogger("mylog");
            return log;
        }

        /// <summary>
        /// 以默认的配置App_Data/xml/log4net.xml配置初始化log4net,并返回所有appenderName的log对象
        /// </summary>
        /// <param name="globalServer">log4net的xml配置</param>
        /// <param name="appenderNameList">appenderName的list对象</param>
        /// <returns>返回log4net.ILog对象的list</returns>
        public List<log4net.ILog> initLog4netByMyDefaultConfig(System.Web.HttpServerUtility globalServer, List<string> appenderNameList)
        {
            initLog4net(globalServer.MapPath("App_Data/xml/log4net.xml"));
            List<log4net.ILog> logList = new List<log4net.ILog>();
            foreach (string appenderName in appenderNameList)
            {
                log4net.ILog log = getLogger(appenderName);
                logList.Add(log);
            }
            return logList;
        }

        /// <summary>
        /// 以指定路径的log4net配置初始化log4net,并返回所有appenderName的log对象
        /// </summary>
        /// <param name="path">log4net的xml配置</param>
        /// <param name="appenderNameList">appenderName的list对象</param>
        /// <returns>返回log4net.ILog对象的list</returns>
        public List<log4net.ILog> initLog4netByMyDefaultConfig(string path, List<string> appenderNameList)
        {
            initLog4net(path);
            List<log4net.ILog> logList = new List<log4net.ILog>(); 
            foreach (string appenderName in appenderNameList)
            {
                log4net.ILog log = getLogger(appenderName);
                logList.Add(log);
            }
            return logList;
        }

        /// <summary>
        /// 以配置/xml/log4netAdvertise.xml初始化log4net,用于广告
        /// </summary>
        /// <param name="path">参数为Server.MapPath("bin/xml/log4netAdvertise.xml")</param>
        /// <returns>返回log4net.ILog的list,共4个元素,分别为database、system、click、active</returns>
        public List<log4net.ILog> initLog4netForAdvertise(string path)
        {
            initLog4net(path);
            List<log4net.ILog> logList = new List<log4net.ILog>();
            logList.Add(getLogger("database"));
            logList.Add(getLogger("system"));
            logList.Add(getLogger("click"));
            logList.Add(getLogger("active"));
            return logList;
        }

    }
}
