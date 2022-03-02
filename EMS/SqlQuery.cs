using System;
using System.Data.SqlClient;

namespace EMS
{
    static class SqlQuery
    {
        public static SqlConnection sqlConnection;

        public static SqlDataReader ExecuteSelectQuery(string sqlQuery)
        {
            var cmd = new SqlCommand(sqlQuery, sqlConnection);
            var rdr = cmd.ExecuteReader();
            return rdr;
        }

        public static void ExecuteInsertQuery(string sqlQuery)
        {
            using (var adapter = new SqlDataAdapter())
            {
                adapter.InsertCommand = new SqlCommand(sqlQuery, sqlConnection);
                adapter.InsertCommand.ExecuteNonQuery();
            }
        }

        public static void ExecuteUpdateQuery(string sqlQuery)
        {
            using (var adapter = new SqlDataAdapter())
            {
                adapter.UpdateCommand = new SqlCommand(sqlQuery, sqlConnection);
                adapter.UpdateCommand.ExecuteNonQuery();
            }
        }

        public static void ExecuteDeleteQuery(String sqlQuery)
        {
            using (var adapter = new SqlDataAdapter())
            {
                adapter.DeleteCommand = new SqlCommand(sqlQuery, sqlConnection);
                adapter.DeleteCommand.ExecuteNonQuery();
            }
        }
    }
}