using System;
using System.Data.SqlClient;

namespace EMS;

class DataVarificationFromDB
{
    public string _connectionString { get; set; }

   public DataVarificationFromDB(string _connectionString)
    {
        this._connectionString = _connectionString;
    }



    public int EmpId_assigning(SqlConnection sqlconnection)
    {
        var tmp = 0;
        try
        {
            //var adapter = new SqlDataAdapter();
            var query = @"SELECT MAX(EmpId) FROM Employee";
            using (var cmd = new SqlCommand(query, sqlconnection))
            {
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    tmp = rdr.GetInt32(0);
                    break;
                }

                rdr.Close();
            }

            return tmp + 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return...");
            Console.ReadLine();
            return -1;
        }
    }



    public bool EmpId_check(double _empID, SqlConnection sqlconnection)
    {
        try
        {
            var adapter = new SqlDataAdapter();
            var query = @"SELECT EmpId from Employee where empID = " + _empID;
            var cmd = new SqlCommand(query, sqlconnection);
            var rdr = cmd.ExecuteReader();
            double tmp = 0;
            while (rdr.Read())
            {
                var tmpID = rdr.GetInt32(0);
                tmp = Convert.ToDouble(tmpID);
                break;
            }

            rdr.Close();
            if (tmp == _empID) return true;
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return...");
            Console.ReadLine();
            return false;
        }
    }

    public bool IsAdmin_count(string _userName, SqlConnection sqlconnection)
    {
        try
        {
            var _isAdmin = false;
            var adapter = new SqlDataAdapter();
            var query = @"SELECT IsAdmin from Credentials";
            var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
            var count = 0;
            while (rdr.Read())
            {
                _isAdmin = rdr.GetBoolean(0);
                if (_isAdmin)
                {
                    count++;
                    if (count > 1)
                    {
                        rdr.Close();
                        return true;
                    }
                }
            }

            rdr.Close();
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return...");
            Console.ReadLine();
            return false;
        }
    }

   public bool IsAdmin_Check(string _userName, SqlConnection sqlconnection)
   {
        try
        {
            var query = @"SELECT IsAdmin  from Credentials where Username = '" + _userName + "'";
            var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
            while (rdr.Read())
            {
                var flag = rdr.GetBoolean(0);
                rdr.Close();
                return flag;
            }
            rdr.Close();
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return...");
            Console.ReadLine();
            return false;
        }
    }
}