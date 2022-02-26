using System;
using System.Data.SqlClient;
using System.Threading;
using ConsoleTables;

namespace EMS
{
    class EmployeeLogin : DataVarificationFromDB
    {
       
        private string _userName { get; set; }
        private double _salary { get; set; }

        public EmployeeLogin(string _userName)
        {
            this._userName = _userName;
            
        }
        

        public void calculateSalary(SqlConnection sqlconnection)
        {
            try
            {
                Console.Write(" Enter the Number of working days:  ");
                var wrokingDays = InputCheck.NumericCheck(" Working Days ");
                string sqlQuery = @"SELECT DailyWages from Employee where userName = " + "'" + _userName + "'";
                using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection))
                {
                    while (rdr.Read())
                    {
                        var tmp = rdr.GetDouble(0);
                        _salary = (wrokingDays * tmp) / 30;
                        break;
                    }
                    //rdr.Close();
                }

                Console.WriteLine("\nSalary of this Month : " + _salary +
                                  "\n\nPress Any key to return");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
            }
        }

        public void GetEmployeeDetails(SqlConnection sqlconnection)
        {
            try
            {
                Console.Clear();
                string sqlQuery = @"SELECT * from Employee where userName = '" + _userName + "'";
                using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection))
                {
                    var table = new ConsoleTable(" Employee ID ", " Employee _firstName ",
                        " Last Name ", " User Name ",
                        " Position ", " Date of Joining ", " Mobile ", " Email ID ", " Salary ",
                        " Monthly Fixed Payment ");
                    while (rdr.Read())
                    {
                        var i = 0;
                        table.AddRow(rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++));
                        break;
                    }
                    //rdr.Close();
                    Console.Clear();
                    Console.WriteLine(table);
                }
                Console.WriteLine("\nPress Any key to return");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                //return false;
            }
        }

        public void EmployeeOption(SqlConnection sqlconnection)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" ---->Logged in As Employee\n ");
                Console.WriteLine("1.To Retrieve  Employee Details: \n2.TO generate Payroll:    \n3.Exit ");
                var choice = InputCheck.NumericCheck("choice");
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        GetEmployeeDetails(sqlconnection);
                        break;
                    case 2:
                        Console.Clear();
                        calculateSalary(sqlconnection);
                        break;
                    case 3:
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("!!! Select from the above Option !!!");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }
    }
}