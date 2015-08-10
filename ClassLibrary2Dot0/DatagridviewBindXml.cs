using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ClassLibrary2Dot0
{
    public class DatagridviewBindXml
    {


        /// <summary>
        /// 绑定xml到控件,并设定最大显示的行数
        /// </summary>
        /// <param name="uri">xml地址</param>
        /// <param name="DataGridView1">要绑定的控件</param>
        /// <param name="displayRow">控件内显示的行数</param>
        /// <param name="page">第几页的数据,从1开始计数</param>
        /// <returns>多个结果时,依次为说明,当前页数,最大页数</returns>
        public List<string> datagridviewBindXml(XmlDocument xd, DataGridView DataGridView1,int displayRow,int indexpage)
        {
            List<string> result = new List<string>();

            //控件为空则直接返回
            if (DataGridView1 == null) {
                result.Add("DataGridView控件为空");
                return result;
            }

            //重置控件数据
            DataGridView1.Rows.Clear();

            //检查xml是否符合要求,并写入数据到控件
            if (xd.DocumentElement.SelectNodes("*").Count == 0)
            {
                result.Add("xml根几点下没有内容");
                return result;
            }
            XmlNode root = xd.DocumentElement;
            for (int i = 0; i < root.SelectNodes("*").Count; i++)
            {
                if (root.SelectNodes("*")[i].SelectNodes("*").Count == 0)
                {
                    result.Add("xml内容不匹配");
                    return result;
                }

                if (root.SelectNodes("*")[0].SelectNodes("*").Count != root.SelectNodes("*")[i].SelectNodes("*").Count)
                {
                    result.Add("xml内容不匹配");
                    return result;
                }
            }

            
            //如果传入的长度小于1,强制修正显示的行数为1
            int len = displayRow;
            if (len < 1)
            {
                len = 1;
            }
            //如果长度大于xml节点总长度,强制修正为xml节点总长度
            if (len > root.SelectNodes("*").Count)
            {
                len = root.SelectNodes("*").Count;
            }
            //计算总的页数,判断传入的页数是否超长
            int page = root.SelectNodes("*").Count / len+1;
            if (indexpage > page) {
                indexpage = page;
            }
            //如果页数小于1,则修正为1
            if (indexpage <= 0) {
                indexpage = 1;
            }
            //按行数来增加控件的显示行数
            if (len > 1)
            {
                DataGridView1.Rows.Add(len - 1);
            }
            //判断当前页有多少行
            int end = indexpage * len;
            if (end > root.SelectNodes("*").Count) {
                end = root.SelectNodes("*").Count;
            }
            //处理显示的数据
            for (int i = (indexpage - 1) * len; i < end; i++)
            {    
                for (int j = 0; j < root.SelectNodes("*")[i].SelectNodes("*").Count; j++)
                {                    
                    DataGridView1.Rows[i%len].Cells[j].Value = root.SelectNodes("*")[i].SelectNodes("*")[j].InnerText;
                }
            }

            result.Add("succ");
            result.Add(indexpage.ToString());
            result.Add(page.ToString());
            return result;
            
        }

        /// <summary>
        /// 绑定xml到控件,并设定最大显示的行数,失败后会重试一次
        /// </summary>
        /// <param name="uri">xml地址</param>
        /// <param name="DataGridView1">要绑定的控件</param>
        /// <param name="displayRow">控件内显示的行数</param>
        /// <param name="page">第几页的数据,从1开始计数</param>
        /// <returns></returns>
        public  List<string> doDatagridviewBindXml(XmlDocument xd, DataGridView DataGridView1, int displayRow, int indexpage)
        {
            List<string> bindResult = datagridviewBindXml(xd, DataGridView1, displayRow, indexpage);
            if (bindResult[0] != "succ")
            {
                bindResult = datagridviewBindXml(xd, DataGridView1, displayRow, indexpage);
            }
            return bindResult;
        }


    }
}
