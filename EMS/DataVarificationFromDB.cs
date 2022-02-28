using System;

namespace EMS
{
    class DataVarificationFromDB
    {
        public int EmpIdAssigning()
        {
            var empIdMaxValue = 0;
            try
            {
                var sqlQuery = @"SELECT MAX(EmpId) FROM Employee";
                using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery))
                {
                    while (rdr.Read())
                    {
                        empIdMaxValue = rdr.GetInt32(0);
                        break;
                    }
                }

                return empIdMaxValue + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                return -1;
            }
        }

        public bool EmpIdCheck(double empID)
        {
            try
            {
                var sqlQuery = @"SELECT EmpId from Employee where empID = " + empID;
                double tmp = 0;
                using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery))
                {
                    while (rdr.Read())
                    {
                        var tmpID = rdr.GetInt32(0);
                        tmp = Convert.ToDouble(tmpID);
                        break;
                    }
                }

                return tmp == empID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                return false;
            }
        }

        public bool AdminCount(string userName)
        {
            try
            {
                var sqlQuery = @"SELECT IsAdmin from Credentials";
                using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery))
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
                                return true;
                            }
                        }
                    }
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

        public bool IsAdmin(string userName)
        {
            try
            {
                var sqlQuery = @"SELECT IsAdmin  from Credentials where Username = '" + userName + "'";
                using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery))
                {
                    while (rdr.Read())
                    {
                        var isAdmin = rdr.GetBoolean(0);
                        return isAdmin;
                    }
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
}