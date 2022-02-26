using System;
using System.Data.SqlClient;

namespace EMS;

class DataVarificationFromDB
{
   
    public int EmpIdAssigning(SqlConnection sqlconnection)
    {
        var tmp = 0;
        try
        {
            var sqlQuery = @"SELECT MAX(EmpId) FROM Employee";
            using (var cmd = new SqlCommand(sqlQuery, sqlconnection))
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



    public bool EmpIdCheck(double _empID, SqlConnection sqlconnection)
    {
        try
        {
            
            var sqlQuery = @"SELECT EmpId from Employee where empID = " + _empID;
            using (var cmd = new SqlCommand(sqlQuery, sqlconnection))
            {
                double tmp = 0;
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var tmpID = rdr.GetInt32(0);
                        tmp = Convert.ToDouble(tmpID);
                        break;
                    }
                    //rdr.Close();
                }
                return tmp == _empID;
            }

            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key to return...");
            Console.ReadLine();
            return false;
        }
    }

    public bool AdminCount(string _userName, SqlConnection sqlconnection)
    {
        try
        {
            var sqlQuery = @"SELECT IsAdmin from Credentials";
            using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection))
            {
                var count = 0;
                while (rdr.Read())
                {
                    var isAdmin = rdr.GetBoolean(0);
                    if (isAdmin)
                    {
                        count++;
                        if (count > 1)
                        {
                            //rdr.Close();
                            return true;
                        }
                    }
                }
                //rdr.Close();
            }

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

   public bool IsAdmin(string _userName, SqlConnection sqlconnection)
   {
        try
        {
            var sqlQuery = @"SELECT IsAdmin  from Credentials where Username = '" + _userName + "'";
            using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection))
            {
                while (rdr.Read())
                {
                    var flag = rdr.GetBoolean(0);
                    //rdr.Close();
                    return flag;
                }
                //rdr.Close();
            }

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