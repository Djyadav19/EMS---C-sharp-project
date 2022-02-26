using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace EMS;

class SqlQuery
{
    //function for executing Select Query...
    public static SqlDataReader ExecuteSelectQuery(string sqlQuery, SqlConnection sqlconnection)
    {
        var cmd = new SqlCommand(sqlQuery, sqlconnection);
        var rdr = cmd.ExecuteReader();
        return rdr;
    }

    //function for executing Insert Query...
    public static void ExecuteInsertQuery(string sqlQuery, SqlConnection sqlconnection)
    {
        using (var adapter = new SqlDataAdapter())
        {
            var cmd = new SqlCommand(sqlQuery, sqlconnection);
            adapter.InsertCommand = new SqlCommand(sqlQuery, sqlconnection);
            adapter.InsertCommand.ExecuteNonQuery();
        }
    }

    //function for executing Update Query...
    public static void ExecuteUpdateQuery(string sqlQuery, SqlConnection sqlconnection)
    {
        using (var adapter = new SqlDataAdapter())
        {
            var cmd = new SqlCommand(sqlQuery, sqlconnection);
            adapter.UpdateCommand = new SqlCommand(sqlQuery, sqlconnection);
            adapter.UpdateCommand.ExecuteNonQuery();
        }
    }
    
    //function for executing Delete Query...
    public static void ExecuteDeleteQuery(String sqlQuery, SqlConnection sqlconnection)
    {
        using (var adapter = new SqlDataAdapter())
        {
            var cmd = new SqlCommand(sqlQuery, sqlconnection);
            adapter.DeleteCommand = new SqlCommand(sqlQuery, sqlconnection);
            adapter.DeleteCommand.ExecuteNonQuery();
        }
    }
}