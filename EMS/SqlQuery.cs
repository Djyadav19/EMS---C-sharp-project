using System.Data.SqlClient;

namespace EMS;

class SqlQuery
{
    public static SqlDataReader ExecuteSelectQuery(string query, SqlConnection con)
    {
        var cmd = new SqlCommand(query, con);
        var rdr = cmd.ExecuteReader();
        return rdr;
    }

    public static void ExecuteInsertQuery(string query, SqlConnection con)
    {
        var adapter = new SqlDataAdapter();
        var cmd = new SqlCommand(query, con);
        adapter.InsertCommand = new SqlCommand(query, con);
        adapter.InsertCommand.ExecuteNonQuery();
    }

    public static void ExecuteUpdateQuery(string query, SqlConnection con)
    {
        var adapter = new SqlDataAdapter();
        var cmd = new SqlCommand(query, con);
        adapter.UpdateCommand = new SqlCommand(query, con);
        adapter.UpdateCommand.ExecuteNonQuery();
    }
}