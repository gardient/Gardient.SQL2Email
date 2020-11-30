using Gardient.SQL2Email.Helpers;
using Gardient.SQL2Email.Models;
using Serilog;
using System;
using System.Data.SqlClient;

namespace Gardient.SQL2Email
{
    static class DBExecuter
    {
        public static QueryResult GetData()
        {
            Log.Debug("Getting from Database");
            using (SqlConnection conn = new SqlConnection(ConfigHelper.ConnectionString))
            {
                Log.Debug("Connection estabilished to {Instance}", conn.DataSource);
                SqlCommand cmd = new SqlCommand(ConfigHelper.Query, conn);

                conn.Open();

                Log.Debug("executing query: {Query}", ConfigHelper.Query);
                SqlDataReader reader = cmd.ExecuteReader();

                Log.Debug("Got {ColumnCount} columns", reader.FieldCount);

                Log.Debug("Getting Column names");
                string[] colNames = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                    colNames[i] = reader.GetName(i);
                Log.Debug("Got Column names: {ColumnNames}", string.Join(", ", colNames));

                QueryResult result = new QueryResult(colNames);


                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    int count = reader.GetValues(row);
                    result.AddRow(row, HandleSpecialValues);
                }

                Log.Debug("Got {RowCount} rows from database", result.RowCount);

                return result;
            }
        }

        private static string HandleSpecialValues(object val)
        {
            DateTime? asDateTime = val as DateTime?;
            if (asDateTime != null)
                return asDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");

            return val.ToString();
        }
    }
}
