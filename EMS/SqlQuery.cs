using System;
using System.Data.SqlClient;

namespace EMS;

class SqlQuery
{
    private static SqlConnection _sqlConnection;

    public SqlQuery(SqlConnection sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }

    public static SqlDataReader ExecuteSelectQuery(string sqlQuery)
    {
        var cmd = new SqlCommand(sqlQuery, _sqlConnection);
        var rdr = cmd.ExecuteReader();
        return rdr;
    }

    public static void ExecuteInsertQuery(string sqlQuery)
    {
        using (var adapter = new SqlDataAdapter())
        {
            adapter.InsertCommand = new SqlCommand(sqlQuery, _sqlConnection);
            adapter.InsertCommand.ExecuteNonQuery();
        }
    }

    public static void ExecuteUpdateQuery(string sqlQuery)
    {
        using (var adapter = new SqlDataAdapter())
        {
            adapter.UpdateCommand = new SqlCommand(sqlQuery, _sqlConnection);
            adapter.UpdateCommand.ExecuteNonQuery();
        }
    }

    public static void ExecuteDeleteQuery(String sqlQuery)
    {
        using (var adapter = new SqlDataAdapter())
        {
            adapter.DeleteCommand = new SqlCommand(sqlQuery, _sqlConnection);
            adapter.DeleteCommand.ExecuteNonQuery();
        }
    }
}