using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Data;

namespace ClassLibrary2Dot0
{
    public class DoExcel
    {
        /// <summary>
        /// 创建excel,并写入数据
        /// </summary>
        /// <param name="sbcontent">要写入的数据stringbuilder</param>
        /// <param name="filename">文件名</param>
        /// <param name="folderpath">当前目录的子文件夹路径,在结尾带一个‘/’</param>
        /// <param name="sheetname">第一个表格的名称</param>
        /// <returns></returns>
        public string createExcel(StringBuilder sbcontent, string filename,string folderpath,string sheetname)
        {
            //返回Excel的路径
            if (folderpath != "") {
                folderpath = folderpath + "/";
            }
            string excelurl = Environment.CurrentDirectory + "/" + folderpath  + filename + ".xlsx";
            
            //目录不存在就创建
            if (!Directory.Exists(Environment.CurrentDirectory + "/" + folderpath)) {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/" + folderpath);
            }

            //文件存在就删除
            if (File.Exists(excelurl)==true) {
                File.Delete(excelurl);
            }

            object missing = Missing.Value;

            //实例Excel类
            Microsoft.Office.Interop.Excel.Application appExcel = new Microsoft.Office.Interop.Excel.Application();

            //DisplayAlerts 属性设置成 False，就不会出现这种警告。 
            appExcel.DisplayAlerts = false;

            //打开Excel
            Microsoft.Office.Interop.Excel.Workbooks workbooks = appExcel.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Missing.Value);
            Microsoft.Office.Interop.Excel.Sheets sheets = workbook.Worksheets;//实例表格
            Microsoft.Office.Interop.Excel.Worksheet sheet1 = (Microsoft.Office.Interop.Excel.Worksheet)sheets[1];

            sheet1.Name = sheetname;
            //设置格式为文本
            sheet1.Cells.NumberFormatLocal = "@";
            //写入数据
            string[] arrline = sbcontent.ToString().Replace("\r\n", "\n").Trim().Split('\n');
            
            for (int i = 0; i < arrline.Length; i++)
            {
                string[] arr = arrline[i].Split(',');
                for (int j = 0; j < arr.Length; j++)
                {
                    sheet1.Cells[i + 1, j + 1] = arr[j];
                }
            }

            //居中对齐
            sheet1.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            sheet1.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            //冻结拆分窗口
            Microsoft.Office.Interop.Excel.Range range = sheet1.get_Range(sheet1.Cells[2, 1], sheet1.Cells[2, 2]);
            range.Select();
            appExcel.ActiveWindow.FreezePanes = true;

            //自动调整行高列宽
            Microsoft.Office.Interop.Excel.Range allcolumn = sheet1.Columns;
            allcolumn.AutoFit();

            string result = "";
            try
            {
                workbook.SaveAs(excelurl);
                //退出excel
                appExcel.Quit();
                result = "succ";
            }
            catch(Exception e)
            {
                result = e.Message;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(appExcel);
                workbooks = null;
                workbook = null;
                sheets = null;
                appExcel = null;
                GC.Collect();

            }

            return result;
        }

        /// <summary>
        /// 创建excel,并写入数据,然后保存并退出,用于mysql查询后将结果保存在excel中
        /// </summary>
        /// <param name="fieldNameList">第一行的名称</param>
        /// <param name="DataTable1">System.Data.DataTable对象</param>
        /// <param name="fileName">文件名</param>
        /// <param name="folderPath">文件目录</param>
        /// <param name="sheetName">sheet名称</param>
        /// <returns>成功返回null,失败返回异常信息</returns>
        public string createExcelOnAspDotNet(List<string> fieldNameList,System.Data.DataTable DataTable1, string fileName, string folderPath, string sheetName)
        {
            //目录校正
            if (folderPath[folderPath.Length-1] != '/'&&folderPath[folderPath.Length-1] !='\\')
            {
                folderPath = folderPath + "/";
            }
            //目录不存在就创建
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //返回Excel的路径
            string excelurl = folderPath + fileName;

            //文件存在就删除
            if (File.Exists(excelurl) == true)
            {
                File.Delete(excelurl);
            }

            object missing = Missing.Value;

            //实例Excel类
            Application appExcel = new Application();

            //DisplayAlerts 属性设置成 False，就不会出现警告。 
            appExcel.DisplayAlerts = false;

            //打开Excel
            Workbooks workbooks;
            workbooks = appExcel.Workbooks;
            Workbook workbook = workbooks.Add(Missing.Value);
            Sheets sheets = workbook.Worksheets;
            Worksheet sheet1 = (Worksheet)sheets[1];

            //设置sheet名称
            sheet1.Name = sheetName;
            //设置格式为文本
            sheet1.Cells.NumberFormatLocal = "@";

            object[,] resultArray = new object[DataTable1.Rows.Count+1, DataTable1.Columns.Count];
            //获取列名称
            for (int j = 0; j < fieldNameList.Count; j++)
            {
                resultArray[0,j] = fieldNameList[j];
            }
            //获取查询结果
            for (int i = 2; i < DataTable1.Rows.Count+2; i++) {
                for (int j = 0; j < DataTable1.Columns.Count; j++)
                {
                    resultArray[i-1, j] = DataTable1.Rows[i-2][j];
                }
            }
            //赋值给range(非常耗费时间)
            Range resultRange = sheet1.get_Range(sheet1.Cells[1, 1], sheet1.Cells[DataTable1.Rows.Count+1, DataTable1.Columns.Count]);
            resultRange.set_Value(XlRangeValueDataType.xlRangeValueDefault, resultArray);
 

            //居中对齐
            sheet1.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            sheet1.Cells.VerticalAlignment = XlHAlign.xlHAlignCenter;

            //冻结拆分窗口
            Range range = sheet1.get_Range(sheet1.Cells[2, 1], sheet1.Cells[2, 2]);
            range.Select();
            appExcel.ActiveWindow.FreezePanes = true;

            //自动调整行高列宽
            Range allcolumn = sheet1.Columns;
            allcolumn.AutoFit();

            //保存并退出
            string result = null;
            try
            {
                workbook.SaveAs(excelurl);
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            finally
            {
                workbook.Close();
                workbooks.Close();
                //退出excel
                appExcel.Quit();
            }

            return result;
        }

    }
}
