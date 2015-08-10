using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class DoList
    {
        /// <summary>
        /// 搜索一个string在list中的index位置
        /// </summary>
        /// <param name="para">string的值</param>
        /// <param name="list">list对象</param>
        /// <returns>string不存在返回-1,否则返回index位置</returns>
        public int findStringIndexInList(string para,List<string> list) {
            int result = -1;
            for (int i = 0; i < list.Count; i++) {
                if (para == list[i]) {
                    result = i;
                }
            }
            return result;
        }
    }
}
