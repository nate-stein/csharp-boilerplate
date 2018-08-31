using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Linq;

namespace Trinity.DataUtilities
{
    public static class DataTableTools
    {
        #region GetDataTableFromDatabase
        /// <summary>
        /// Return a DataTable for a given sql query from the database specified by the input connection string.
        /// </summary>
        /// <param name="databaseConnectionString"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromDatabase (string databaseConnectionString, string sqlQuery)
        {
            using (OleDbConnection connection = new OleDbConnection (databaseConnectionString))
            {
                connection.Open ();
                OleDbDataAdapter dbAdapter = new OleDbDataAdapter (sqlQuery, connection);
                DataTable table = new DataTable ();
                dbAdapter.Fill (table);
                return table;
            }
        }
        #endregion

        #region WriteDataToTextFile
        /// <summary>
        /// Takes in a DataTable and writes all of its columns to a text file with different fields separated by a 
        /// semicolon.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="fileSavePath"></param>
        public static void WriteDataToTextFile (DataTable dataTable, string fileSavePath)
        {
            int column = 0;
            System.IO.StreamWriter sw = null;

            sw = new System.IO.StreamWriter (fileSavePath, false);

            for (column = 0; column < dataTable.Columns.Count - 1; column++)
            {
                sw.Write (dataTable.Columns [column].ColumnName + ";");
            }
            sw.Write (dataTable.Columns [column].ColumnName);
            sw.WriteLine ();

            foreach (DataRow row in dataTable.Rows)
            {
                object [] array = row.ItemArray;

                for (column = 0; column < array.Length - 1; column++)
                {
                    sw.Write (array [column].ToString () + ";");
                }
                sw.Write (array [column].ToString ());
                sw.WriteLine ();
            }
        }
        #endregion

        #region WriteDataToCsvFile
        /// <summary>
        /// Takes in a DataTable and outputs all fields to a CSV file.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="fileSavePath"></param>
        public static void WriteDataToCsvFile (DataTable dataTable, string fileSavePath)
        {
            StringBuilder sb = new StringBuilder ();

            IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn> ().
                                              Select (column => column.ColumnName);
            sb.AppendLine (string.Join (",", columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select (field => field.ToString ());
                sb.AppendLine (string.Join (",", fields));
            }

            System.IO.File.WriteAllText (fileSavePath, sb.ToString ());
        }
        #endregion
    }
}