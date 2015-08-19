using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace ClassLibrary2Dot0
{
    /// <summary>
    /// mysql连接用法参考http://www.connectionstrings.com/mysql-connector-net-mysqlconnection/standard/
    /// </summary>
    public class DoMysql
    {

        public class MysqlClass
        {
            public string netConnectionStrings { get; set; }
            public MySqlConnection MySqlConnection1 { get; set; }
            public string exceptionString { get; set; }
        }


        /// <summary>
        /// mysql查询操作
        /// </summary>
        /// <param name="MysqlClass1">MysqlClass对象</param>
        /// <param name="sqlstring">sql语句</param>
        /// <returns>返回List List string 的结果</returns>
        public List<List<string>> mysqlSelect(MysqlClass MysqlClass1, string sqlstring)
        {
            List<List<string>> result = new List<List<string>>();
            lock (MysqlClass1)
            {
                string netConnectionStrings = MysqlClass1.netConnectionStrings;
                //将mysql中的数据读取到dataset
                MySqlDataAdapter MySqlDataAdapter1 = new MySqlDataAdapter(sqlstring, netConnectionStrings);
                DataSet DataSet1 = new DataSet();
                try
                {
                    MySqlDataAdapter1.Fill(DataSet1);
                }
                catch (Exception e)
                {
                    DataSet1.Dispose();
                    MySqlDataAdapter1.Dispose();
                    MysqlClass1.exceptionString = e.Message;
                    return null;
                }


                //判断是否有结果
                if (DataSet1.Tables[0].Rows.Count != 0)
                {
                    //遍历dataset中的所有数据
                    for (int i = 0; i < DataSet1.Tables[0].Rows.Count; i++)
                    {
                        List<string> rowtemp = new List<string>();
                        for (int j = 0; j < DataSet1.Tables[0].Columns.Count; j++)
                        {
                            rowtemp.Add(DataSet1.Tables[0].Rows[i][j].ToString());
                        }
                        result.Add(rowtemp);
                    }
                }
                //释放资源
                DataSet1.Dispose();
                MySqlDataAdapter1.Dispose();
                MysqlClass1.exceptionString = null;
            }
            return result;
        }

        /// <summary>
        /// mysql查询操作
        /// </summary>
        /// <param name="MysqlClass1">MysqlClass对象</param>
        /// <param name="sqlstring">sql语句</param>
        /// <returns>返回System.Data.DataTable的结果</returns>
        public DataTable mysqlSelectToDataSet(MysqlClass MysqlClass1, string sqlstring)
        {
            string netConnectionStrings = MysqlClass1.netConnectionStrings;
            DataTable DataTable1 = new DataTable();
            lock (MysqlClass1)
            {
                //将mysql中的数据读取到dataset
                MySqlDataAdapter MySqlDataAdapter1 = new MySqlDataAdapter(sqlstring, netConnectionStrings);
                DataSet DataSet1 = new DataSet();
                try
                {
                    MySqlDataAdapter1.SelectCommand.CommandTimeout = 36000;
                    MySqlDataAdapter1.Fill(DataSet1);
                }
                catch (Exception e)
                {
                    DataSet1.Dispose();
                    MySqlDataAdapter1.Dispose();
                    MysqlClass1.exceptionString = e.Message;
                    return null;
                }
                DataTable1 = DataSet1.Tables[0];
                MysqlClass1.exceptionString = null;

                //释放资源
                DataSet1.Dispose();
                MySqlDataAdapter1.Dispose();

            }
            return DataTable1;
        }

        /// <summary>
        /// 构建MySQL Connector/Net connection strings
        /// </summary>
        /// <param name="Server">mysql服务器地址</param>
        /// <param name="Port">mysql服务器端口</param>
        /// <param name="Database">数据库名称</param>
        /// <param name="Uid">mysql用户名</param>
        /// <param name="Pwd">mysql密码</param>
        /// <param name="CharSet">编码格式(utf8小写)</param>
        /// <returns>返回MySQL Connector/Net connection strings</returns>
        public string buildNetConnectionStrings(string Server, string Port, string Database, string Uid, string Pwd, string CharSet)
        {
            string netConnectionStrings = "Server=" + Server + ";Port=" + Port + ";Database=" + Database + ";Uid=" + Uid + ";Pwd=" + Pwd + ";CharSet=" + CharSet + ";";
            return netConnectionStrings;
        }


        /// <summary>
        /// mysql连接初始化
        /// </summary>
        /// <param name="netConnectionStrings">mysql连接字符串</param>
        /// <returns>返回MysqlClass对象</returns>
        public MysqlClass mysqlInit(string netConnectionStrings)
        {
            MysqlClass MysqlClass1 = new MysqlClass();
            MySqlConnection MySqlConnection1 = new MySql.Data.MySqlClient.MySqlConnection(netConnectionStrings);
            try
            {
                MySqlConnection1.Open();
                MysqlClass1.MySqlConnection1 = MySqlConnection1;
                MysqlClass1.netConnectionStrings = netConnectionStrings;
                MysqlClass1.exceptionString = null;
                return MysqlClass1;
            }
            catch (Exception e)
            {
                MysqlClass1.exceptionString = e.Message;
                return MysqlClass1;
            }
        }

        /// <summary>
        /// mysql修改操作,包括查询和更新等
        /// </summary>
        /// <param name="MysqlClass1">MysqlClass对象</param>
        /// <param name="sqlstring">执行的sql语句</param>
        /// <returns>返回一个ExecuteNonQuery执行是否成功的bool值</returns>
        private bool mysqlAlter(MysqlClass MysqlClass1, string sqlstring)
        {
            MySqlConnection MySqlConnection1 = MysqlClass1.MySqlConnection1;
            string netConnectionStrings = MysqlClass1.netConnectionStrings;

            //如果mysql连接失效,就重新初始化连接
            if (MySqlConnection1 == null || MySqlConnection1.State == System.Data.ConnectionState.Closed || MySqlConnection1.State == System.Data.ConnectionState.Broken)
            {
                if (MySqlConnection1 != null)
                {
                    MySqlConnection1.Close();
                    MySqlConnection1.Dispose();
                }
                //判断mysql连接失效后,mysql初始化是否失败
                MysqlClass1 = mysqlInit(netConnectionStrings);
                if (MysqlClass1.exceptionString != null)
                {
                    return false;
                }
                //成功后赋值MySqlConnection1
                MySqlConnection1 = MysqlClass1.MySqlConnection1;
            }


            //执行指定的sql
            lock (MysqlClass1)
            {
                MySqlCommand MySqlCommand1 = MySqlConnection1.CreateCommand();
                MySqlCommand1.CommandText = sqlstring;
                try
                {
                    MySqlCommand1.ExecuteNonQuery();
                    MysqlClass1.exceptionString = null;
                    return true;
                }
                catch (Exception e)
                {
                    MySqlConnection1.Close();
                    MySqlConnection1.Dispose();
                    MySqlCommand1.Dispose();
                    MysqlClass1.exceptionString = e.Message;
                    return false;
                }
            }
        }

        /// <summary>
        /// mysql修改操作,包括查询和更新等，失败后重试一次
        /// </summary>
        /// <param name="MysqlClass1">MysqlClass对象</param>
        /// <param name="sqlstring">执行的sql语句</param>
        /// <returns>返回一个ExecuteNonQuery执行是否成功的bool值</returns>
        public bool doMysqlAlter(MysqlClass MysqlClass1, string sqlstring)
        {
            bool mysqlAlterResult = mysqlAlter(MysqlClass1, sqlstring);
            //第一次就成功执行
            if (mysqlAlterResult)
            {
                MysqlClass1.exceptionString = null;
                return mysqlAlterResult;
            }
            //失败后重试一次
            else
            {
                mysqlAlterResult = mysqlAlter(MysqlClass1, sqlstring);
                //重试后成功
                if (mysqlAlterResult)
                {
                    MysqlClass1.exceptionString = null;
                    return mysqlAlterResult;
                }
                //重试后仍然失败
                else
                {
                    return mysqlAlterResult;
                }
            }
        }

        /// <summary>
        /// 以默认的mysql.xml初始化mysql连接
        /// </summary>
        /// <param name="xmlPath">mysql.xml的地址</param>
        /// <returns>返回MySqlClass对象</returns>
        public MysqlClass initMysqlByDefaultXml(string xmlPath)
        {
            MysqlClass MysqlClass1 = new MysqlClass();
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                MysqlClass1.exceptionString = e.Message;
                return MysqlClass1;
            }

            string Server = xd.SelectSingleNode("//mysql").SelectSingleNode("Server").InnerText;
            string Port = xd.SelectSingleNode("//mysql").SelectSingleNode("Port").InnerText;
            string Database = xd.SelectSingleNode("//mysql").SelectSingleNode("Database").InnerText;
            string Uid = xd.SelectSingleNode("//mysql").SelectSingleNode("Uid").InnerText;
            string Pwd = xd.SelectSingleNode("//mysql").SelectSingleNode("Pwd").InnerText;
            string CharSet = xd.SelectSingleNode("//mysql").SelectSingleNode("CharSet").InnerText;
            string netConnectionStrings = buildNetConnectionStrings(Server, Port, Database, Uid, Pwd, CharSet);
            MysqlClass1 = mysqlInit(netConnectionStrings);
            return MysqlClass1;
        }



        /// <summary>
        /// 执行sql非查询操作，并输出日志到ILog1
        /// </summary>
        /// <param name="MysqlClass1">MysqlClass对象</param>
        /// <param name="sqlString">sql非查询语句</param>
        /// <param name="businessLog">mysql执行成功时的log4net.ILog对象</param>
        /// <param name="errorLog">mysql执行失败时的log4net.ILog对象</param>
        /// <param name="logHeadString">log4net日志每一行的前面字符</param>
        /// <returns>mysql语句执行成功返回true,失败返回false</returns>
        public bool doMysqlAlterWithLog4net(MysqlClass MysqlClass1, string sqlString, log4net.ILog businessLog, log4net.ILog errorLog, string logHeadString)
        {
            lock (MysqlClass1)
            {
                bool queryResult = doMysqlAlter(MysqlClass1, sqlString);
                if (!queryResult)
                {
                    errorLog.Info(logHeadString + "mysql插入失败:" + sqlString + ":" + MysqlClass1.exceptionString);
                    return false;
                }
                else
                {
                    businessLog.Info(logHeadString + "mysql插入成功:" + sqlString);
                    MysqlClass1.exceptionString = null;
                    return true;
                }
            }
        }



        /// <summary>
        /// mysql查询操作
        /// </summary>
        /// <param name="MysqlClass1">MysqlClass对象</param>
        /// <param name="sqlString">sql语句</param>
        /// <param name="businessLog">mysql执行成功时的log4net.ILog对象</param>
        /// <param name="errorLog">mysql执行失败时的log4net.ILog对象</param>
        /// <param name="logHeadString">log4net日志每一行的前面字符</param> 
        /// <returns>返回DataTable类型的结果</returns>
        public DataTable doMysqlSelectToDataSetWithLog4net(MysqlClass MysqlClass1, string sqlString, log4net.ILog businessLog, log4net.ILog errorLog, string logHeadString)
        {
            DataTable DataTable1 = mysqlSelectToDataSet(MysqlClass1, sqlString);
            if (MysqlClass1.exceptionString != null)
            {
                errorLog.Info(logHeadString + "mysql查询失败:" + sqlString + ":" + MysqlClass1.exceptionString);
                return null;
            }
            businessLog.Info(logHeadString + "mysql查询成功:" + sqlString);
            MysqlClass1.exceptionString = null;
            return DataTable1;
        }

        /// <summary>
        /// mysql查询操作
        /// </summary>
        /// <param name="MysqlClass1">MysqlClass对象</param>
        /// <param name="sqlCommand">sql语句</param>
        /// <param name="businessLog">mysql执行成功时的log4net.ILog对象</param>
        /// <param name="errorLog">mysql执行失败时的log4net.ILog对象</param>
        /// <param name="logHeadString">log4net日志每一行的前面字符</param>
        /// <returns>成功返回指定的json（按查询结果的行构建）,失败返回null</returns>
        public string doMysqlSelectToRowsJsonWithLog4net(MysqlClass MysqlClass1, string sqlCommand, log4net.ILog businessLog, log4net.ILog errorLog, string logHeadString)
        {
            string result = null;
            DataTable DataTable1 = mysqlSelectToDataSet(MysqlClass1, sqlCommand);
            if (MysqlClass1.exceptionString != null)
            {
                errorLog.Info(logHeadString + "mysql查询失败:" + sqlCommand + ":" + MysqlClass1.exceptionString);
                return result;
            }
            businessLog.Info(logHeadString + "mysql查询成功:" + sqlCommand);
            StringBuilder jsonStringBuilder = new StringBuilder();

            jsonStringBuilder.Append("[");
            for (int i = 0; i < DataTable1.Rows.Count; i++)
            {
                jsonStringBuilder.Append("[");
                for (int j = 0; j < DataTable1.Columns.Count; j++)
                {
                    jsonStringBuilder.Append("\"" + DataTable1.Rows[i][j].ToString() + "\",");
                }
                //移除最后的逗号
                if (jsonStringBuilder.ToString().Substring(jsonStringBuilder.Length - 1) == ",")
                {
                    jsonStringBuilder.Remove(jsonStringBuilder.Length - 1, 1);
                }
                jsonStringBuilder.Append("],");
            }
            //移除最后的逗号
            if (jsonStringBuilder.ToString().Substring(jsonStringBuilder.Length - 1) == ",")
            {
                jsonStringBuilder.Remove(jsonStringBuilder.Length - 1, 1);
            }
            jsonStringBuilder.Append("]");
            result = jsonStringBuilder.ToString();
            MysqlClass1.exceptionString = null;
            return result;
        }

        /// <summary>
        /// mysql查询操作
        /// </summary>
        /// <param name="MysqlClass1">MysqlClass对象</param>
        /// <param name="sqlCommand">sql语句</param>
        /// <param name="businessLog">mysql执行成功时的log4net.ILog对象</param>
        /// <param name="errorLog">mysql执行失败时的log4net.ILog对象</param>
        /// <param name="logHeadString">log4net日志每一行的前面字符</param>
        /// <returns>成功返回指定的json（按查询结果的列构建）,失败返回null</returns>
        public string doMysqlSelectToColumnsJsonWithLog4net(MysqlClass MysqlClass1, string sqlCommand, log4net.ILog businessLog, log4net.ILog errorLog, string logHeadString)
        {
            string result = null;
            DataTable DataTable1 = mysqlSelectToDataSet(MysqlClass1, sqlCommand);
            if (MysqlClass1.exceptionString != null)
            {
                errorLog.Info(logHeadString + "mysql查询失败:" + sqlCommand + ":" + MysqlClass1.exceptionString);
                return result;
            }
            businessLog.Info(logHeadString + "mysql查询成功:" + sqlCommand);
            StringBuilder jsonStringBuilder = new StringBuilder();
            if (DataTable1.Rows.Count == 0)
            {
                return null;
            }

            jsonStringBuilder.Append("[");
            for (int j = 0; j < DataTable1.Columns.Count; j++)
            {
                jsonStringBuilder.Append("[");
                for (int i = 0; i < DataTable1.Rows.Count; i++)
                {
                    jsonStringBuilder.Append("\"" + DataTable1.Rows[i][j].ToString() + "\",");
                }
                //移除最后的逗号
                if (jsonStringBuilder.ToString().Substring(jsonStringBuilder.Length - 1) == ",")
                {
                    jsonStringBuilder.Remove(jsonStringBuilder.Length - 1, 1);
                }
                jsonStringBuilder.Append("],");
            }
            //移除最后的逗号
            if (jsonStringBuilder.ToString().Substring(jsonStringBuilder.Length - 1) == ",")
            {
                jsonStringBuilder.Remove(jsonStringBuilder.Length - 1, 1);
            }
            jsonStringBuilder.Append("]");
            result = jsonStringBuilder.ToString();
            return result;
        }

        /// <summary>
        /// mysql查询操作
        /// </summary>
        /// <param name="MysqlClass1">MysqlClass对象</param>
        /// <param name="sqlCommand">sql语句</param>
        /// <param name="businessLog">mysql执行成功时的log4net.ILog对象</param>
        /// <param name="errorLog">mysql执行失败时的log4net.ILog对象</param>
        /// <param name="logHeadString">log4net日志每一行的前面字符</param>
        /// <returns>成功返回指定的json（按查询结果的列构建）,失败返回null</returns>
        public string doMysqlSelectToBootstrapTableJsonWithLog4net(MysqlClass MysqlClass1, string sqlCommand, log4net.ILog businessLog, log4net.ILog errorLog, string logHeadString)
        {
            string result = null;
            DataTable DataTable1 = mysqlSelectToDataSet(MysqlClass1, sqlCommand);
            if (MysqlClass1.exceptionString != null)
            {
                errorLog.Info(logHeadString + "mysql查询失败:" + sqlCommand + ":" + MysqlClass1.exceptionString);
                return result;
            }
            businessLog.Info(logHeadString + "mysql查询成功:" + sqlCommand);
            StringBuilder jsonStringBuilder = new StringBuilder();
            if (DataTable1.Rows.Count == 0)
            {
                return null;
            }

            jsonStringBuilder.Append("[");
            for (int i = 0; i < DataTable1.Rows.Count; i++)
            {
                jsonStringBuilder.Append("{");
                for (int j = 0; j < DataTable1.Columns.Count; j++)
                {
                    jsonStringBuilder.Append("\"" + DataTable1.Columns[j].ColumnName + "\":\"" + DataTable1.Rows[i][j].ToString() + "\",");
                }
                //移除最后的逗号
                if (jsonStringBuilder.ToString().Substring(jsonStringBuilder.Length - 1) == ",")
                {
                    jsonStringBuilder.Remove(jsonStringBuilder.Length - 1, 1);
                }
                jsonStringBuilder.Append("},");
            }
            //移除最后的逗号
            if (jsonStringBuilder.ToString().Substring(jsonStringBuilder.Length - 1) == ",")
            {
                jsonStringBuilder.Remove(jsonStringBuilder.Length - 1, 1);
            }
            jsonStringBuilder.Append("]");
            result = jsonStringBuilder.ToString();
            return result;
        }

    }
}
