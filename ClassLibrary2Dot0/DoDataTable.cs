using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ClassLibrary2Dot0
{
    public class DoDataTable
    {

        public DataTable getDataTableFromDataSet(DataSet DataSet1, string sheetName) {
            DataTable DataTable1 = null;
            for (int i = 0; i < DataSet1.Tables.Count; i++) {
                if (DataSet1.Tables[i].TableName == sheetName) {
                    DataTable1 = DataSet1.Tables[i];
                }
            }
            return DataTable1;
        }

    }
}
