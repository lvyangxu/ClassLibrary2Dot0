using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace ClassLibrary2Dot0
{
    public class DoJson
    {
        /// <summary>
        /// xmlnode转为json字符串
        /// </summary>
        /// <param name="xn">xmlnode对象</param>
        /// <returns>返回json字符串</returns>
        public string xmlToJsonstr(XmlNode xn) {
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xn);
            return jsonStr;           
        }

        /// <summary>
        /// string转为jArray对象
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>返回jArray对象</returns>
        public JArray stringTojArray(string jsonStr)
        {
            JArray JArray1 = (JArray)JsonConvert.DeserializeObject(jsonStr);
            return JArray1;
        }

        /// <summary>
        /// 获取指定json串中的指定键的值
        /// </summary>
        /// <param name="jsonStr">json串</param>
        /// <param name="name">键值对的名字</param>
        /// <returns>返回指定键的值</returns>
        public string[] getValueByNameFromJsonStr(string jsonStr, string name)
        {
            string[] result = new string[2] { null, null };
            try
            {
                JObject json = JObject.Parse(jsonStr);
                result[0] = json[name].Value<string>();
            }
            catch (Exception e)
            {
                result[1] = e.Message;
            }
            return result;
        }

        public string stringToJarrayString(string content)
        {

            return "";
        }
    }
}
