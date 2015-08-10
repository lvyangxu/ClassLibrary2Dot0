using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ClassLibrary2Dot0
{
    public class DoXml
    {
        /// <summary>
        /// 获取指定路径的xml文档
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <returns>返回object类型的二维数组,参数分别为XmlDocument类型的对象、string类型的异常信息</returns>
        public object[] getXmlDocument(string xmlPath)
        {
            object[] result = new object[2] { null, null };
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            result[0] = xd;
            return result;      
        }

        /// <summary>
        /// 检查指定节点名称及指定节点的值是否已存在于xml中
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">节点值</param>
        /// <returns>返回object类型的二维数组,参数分别为bool类型的结果、string类型的异常信息</returns>
        public object[] checkXmlNodeValueExist(string xmlPath, string nodeName, string nodeValue)
        {
            object[] result = new object[2] { false, null };
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            foreach (XmlNode xn in xd.SelectNodes("//" + nodeName))
            {
                if (xn.InnerText == nodeValue)
                {
                    result[0] = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指定节点名称在xml中出现的次数
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>返回object类型的二维数组,参数分别为int类型的结果、string类型的异常信息</returns>
        public object[] getXmlNodeTimes(string xmlPath, string nodeName)
        {
            object[] result = new object[2] { 0, null };
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            result[0] = xd.SelectNodes("//" + nodeName).Count;
            return result;
        }

        /// <summary>
        /// 移除指定xml中指定xml节点名称的第n个子节点
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeIndex">要移除节点在xml中的序号</param>
        /// <returns>返回object类型的二维数组,参数分别为bool类型的结果、string类型的异常信息</returns>
        public object[] removeXmlNode(string xmlPath, string nodeName, int nodeIndex)
        {
            object[] result = new object[2] { false, null };
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            XmlNodeList XmlNodeList1 = xd.SelectNodes("//" + nodeName);
            for (int i = 0; i < xd.SelectNodes("//" + nodeName).Count;i++ )
            {
                if (i == nodeIndex)
                {
                    try
                    {
                        xd.RemoveChild(XmlNodeList1[i]);
                        result[0] = true;
                    }
                    catch(Exception exc) {
                        result[1] = exc.Message;
                    }                    
                }
            }
            return result;       
        }

        /// <summary>
        /// 根据指定节点名称及指定节点的值获取第一个xmlNode
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">节点值</param>
        /// <returns>返回object类型的二维数组,参数分别为xmlNode类型的结果、string类型的异常信息</returns>
        public object[] getXmlNodeByNodeNameAndNodeValue(string xmlPath, string nodeName, string nodeValue)
        { 
            object[] result = new object[2] { null, null };
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            foreach (XmlNode xn in xd.SelectNodes("//" + nodeName))
            {
                if (xn.InnerText == nodeValue)
                {
                    result[0] = xn;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据指定节点名称及指定节点的值获取第一个xmlNode
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">节点值</param>
        /// <returns>返回object类型的二维数组,参数分别为xmlNode类型的结果、string类型的异常信息</returns>
        public object[] getXmlNodeByNodeNameAndNodeValue(XmlDocument xd, string nodeName, string nodeValue)
        {
            object[] result = new object[2] { null, null };
            try
            {
                foreach (XmlNode xn in xd.SelectNodes("//" + nodeName))
                {
                    if (xn.InnerText == nodeValue)
                    {
                        result[0] = xn;
                        break;
                    }
                }
            }
            catch (Exception e) {
                result[1] = e.Message;
                return result;
            }
            return result;
        }


        /// <summary>
        /// 根据指定节点名称获取所有的xmlNode
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>返回object类型的二维数组,参数分别为List xmlNode类型的结果、string类型的异常信息</returns>
        public object[] getAllXmlNodeByNodeNameAndNodeValue(string xmlPath, string nodeName)
        {
            object[] result = new object[2] { null, null };
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            List<XmlNode> xnlist = new List<XmlNode>();
            foreach (XmlNode xn in xd.SelectNodes("//" + nodeName))
            {
                xnlist.Add(xn);
            }
            result[0] = xnlist;
            return result;
        }

        /// <summary>
        /// 将指定节点的值赋值给第一个匹配到的xmlNode
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">节点值</param>
        /// <returns>成功返回Null,失败返回string类型的异常信息</returns>
        public string setXmlNodeValueByNodeName(string xmlPath, string nodeName, string nodeValue)
        {
            string result = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result = e.Message;
                return result;
            }
            xd.SelectSingleNode("//" + nodeName).InnerText = nodeValue;
            try
            {
                xd.Save(xmlPath);
            }
            catch (Exception e)
            {
                result = e.Message;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 将指定节点列表的值赋值给第一个匹配到的xmlNode
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeNameList">节点名称列表</param>
        /// <param name="nodeValueList">节点值列表</param>
        /// <returns>成功返回Null,失败返回string类型的异常信息</returns>
        public string setXmlNodeValuesByNodeNames(string xmlPath, List<string> nodeNameList, List<string> nodeValueList)
        {
            string result = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result = e.Message;
                return result;
            }
            if (nodeNameList.Count != nodeValueList.Count) {
                result = "invalidNameAndValue";
            }

            for (int i = 0; i < nodeNameList.Count; i++) {
                xd.SelectSingleNode("//" + nodeNameList[i]).InnerText = nodeValueList[i];
            }
            try
            {
                xd.Save(xmlPath);
            }
            catch (Exception e)
            {
                result = e.Message;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 获取指定节点名称获取第一个节点的值
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>返回string类型的二维数组,参数分别为节点的值、异常信息</returns>
        public string[] getXmlNodeValueByNodeName(string xmlPath, string nodeName)
        {
            string[] result = new string[2] { null, null };
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }
            XmlNode xn = xd.SelectSingleNode("//" + nodeName);
            if (xn == null)
            {
                result[1] = "noSuchNode";
                return result;
            }
            result[0] = xn.InnerText;
            return result;
        }

        /// <summary>
        /// 获取指定节点名称获取指定节点的值
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <param name="nodeNameArr">节点名称数组</param>
        /// <returns>返回object类型的二维数组,参数分别为节点的值list、异常信息</returns>
        public object[] getXmlNodeValueByNodeName(string xmlPath, string[] nodeNameArr)
        {
            object[] result = new object[2] { null, null };
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.Load(xmlPath);
            }
            catch (Exception e)
            {
                result[1] = e.Message;
                return result;
            }

            List<string> nodeValueList = new List<string>();
            for (int i = 0; i < nodeNameArr.Length; i++)
            {
                XmlNode xn = xd.SelectSingleNode("//" + nodeNameArr[i]);
                if (xn == null)
                {
                    result[1] = "noSuchNode";
                    return result;
                }
                nodeValueList.Add(xn.InnerText);
            }
            result[0] = nodeValueList;
            return result;
        }        
    }
}
