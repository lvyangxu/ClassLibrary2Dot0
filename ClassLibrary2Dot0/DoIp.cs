using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ClassLibrary2Dot0
{
    public class DoIp
    {
        DoHttpRequest DoHttpRequest1 = new DoHttpRequest();
        DoJson DoJson1 = new DoJson();
        DecodeAndEncode DecodeAndEncode1 = new DecodeAndEncode();

        /// <summary>
        /// ip地址查询接口(新浪、淘宝)
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="channel">查询渠道,sina或taobao</param>
        /// <returns>返回string的数组,3个元素依次为省、城市、异常信息</returns>
        public string[] ipLookUp(string ip,string channel)
        {
            string[] result = new string[3] { null, null, null };
            string url = null;
            string provinceKey = null;
            switch (channel) { 
                case "sina":
                    url = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=";
                    provinceKey = "province";
                    break;
                case "taobao":
                    url = "http://ip.taobao.com/service/getIpInfo.php?ip=";
                    provinceKey = "region";
                    break;
                default:
                    result[2] = "errorChannel";
                    return result;
            }

            
            string[] httpResult = DoHttpRequest1.doHttpRequest( url + ip, "UTF-8");
            if (httpResult[1] != null)
            {
                result[2] = httpResult[1];
                return result;
            }
            Regex Regex1 = new Regex("{[^{}]*}");
            Match Match1 = Regex1.Match(httpResult[0]);
            if (Match1.Value == string.Empty)
            {
                result[2] = "errorSinaHttpResponse";
                return result;
            }

            string[] jsonResult = DoJson1.getValueByNameFromJsonStr(Match1.Value, provinceKey);
            if (jsonResult[1] != null)
            {
                result[2] = jsonResult[1];
                return result;
            }
            string province = jsonResult[0];
            jsonResult = DoJson1.getValueByNameFromJsonStr(Match1.Value, "city");
            if (jsonResult[1] != null)
            {
                result[2] = jsonResult[1];
                return result;
            }
            string city = jsonResult[0];           
            result[0] = province;
            result[1] = city; 
            return result;
        }

        /// <summary>
        /// ip地址查询接口(新浪、淘宝)，先用新浪进行查询，如果失败就调用淘宝的接口
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns>返回string的数组,3个元素依次为省、城市、异常信息</returns>
        public string[] doIpLookUp(string ip)
        {
            string[] ipResult = ipLookUp(ip, "sina");
            if (ipResult[2] != null)
            {
                ipResult = ipLookUp(ip, "taobao");
                return ipResult;
            }
            else {
                return ipResult;
            }
        }
    }
}
