using System;
using System.Collections.Generic;
using System.Text;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Data;

namespace ClassLibrary2Dot0
{
    public class DoNPOI
    {

        /// <summary>
        /// 读取DataTable的数据,并把数据写入excel
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="sheetName">excel里第一个sheet的名称</param>
        /// <param name="DataTable1">要写入的DataTable</param>
        /// <param name="freezePaneRow">冻结拆分窗口的行数</param>
        /// <returns></returns>
        public string createExcel(string filePath, string fileName, string sheetName, DataTable DataTable1, int freezePaneRow)
        {
            string result = null;
            try
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook();
                ISheet sheet = xssfworkbook.CreateSheet(sheetName);

                //设置居中
                ICellStyle styleCell = xssfworkbook.CreateCellStyle();
                styleCell.VerticalAlignment = VerticalAlignment.Center;
                styleCell.Alignment = HorizontalAlignment.Center;

                for (int i = 0; i < DataTable1.Rows.Count; i++)
                {
                    IRow row1 = sheet.CreateRow(i);
                    for (int j = 0; j < DataTable1.Columns.Count; j++)
                    {
                        ICell cell = row1.CreateCell(j);
                        cell.SetCellValue(DataTable1.Rows[i][j].ToString());
                        sheet.GetRow(i).GetCell(j).CellStyle = styleCell;
                    }
                }

                //自适应列宽
                for (int i = 0; i < DataTable1.Columns.Count; i++) { 
                    sheet.AutoSizeColumn(i);
                }
                //冻结拆分窗口
                sheet.CreateFreezePane(1, freezePaneRow);

                

                MemoryStream stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                stream.Close();
                stream.Dispose();
                if (!Directory.Exists(filePath)) {
                    Directory.CreateDirectory(filePath);
                }

                using (FileStream fs = new FileStream(filePath + fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                }

                
            }
            catch (Exception e) {
                result = e.Message;
            }
            return result;
        }


        /// <summary>
        /// 读取excel数据到datatable
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回object数组,元素分辨为DataSet类型的结果,string类型的异常信息</returns>
        public object[] readExcel(string filePath)
        {
            object[] result = new object[2] { null, null };
            DataSet DataSet1 = new DataSet();
            
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(fs);

                    for (int n = 0; n < xssfworkbook.NumberOfSheets; n++)
                    {
                        ISheet sheet = xssfworkbook.GetSheetAt(n);

                        DataTable DataTable1 = new DataTable();
                        for (int rowNum = 0; rowNum < sheet.LastRowNum + 1; rowNum++)
                        {
                            DataRow DataRow1 = DataTable1.NewRow();
                            IRow IRow1 = sheet.GetRow(rowNum);

                            if (IRow1 == null) {
                                continue;
                            }

                            //为datatable的列命名
                            if (rowNum == 0)
                            {
                                for (int i = 0; i < IRow1.LastCellNum; i++)
                                {
                                    DataColumn DataColumn1 = new DataColumn();
                                    DataColumn1.ColumnName = i.ToString();
                                    DataTable1.Columns.Add(DataColumn1);
                                }
                            }

                            //如果该行数据为空,跳过当前行
                            if (IRow1 == null)
                            {
                                DataTable1.Rows.Add(DataRow1);
                                continue;
                            }

                            for (int columnNum = 0; columnNum < IRow1.LastCellNum; columnNum++)
                            {
                                ICell ICell1 = IRow1.GetCell(columnNum);
                                string cellString = string.Empty;
                                if (ICell1 == null)
                                {
                                    cellString = "";
                                }
                                else
                                {
                                    cellString = IRow1.GetCell(columnNum).ToString();
                                }
                                DataRow1[columnNum.ToString()] = cellString;
                            }
                            DataTable1.Rows.Add(DataRow1);
                        }
                        
                        DataSet1.Tables.Add(DataTable1);
                        DataTable1.Dispose();
                    }
                    result[0] = DataSet1;
                }
                
            }
            catch (Exception e)
            {
                result[0] = null;
                result[1] = e.Message;
                return result;
            }

            return result;
        }
    }
}
