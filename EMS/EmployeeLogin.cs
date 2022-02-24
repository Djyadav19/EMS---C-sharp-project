using System;
using System.Data.SqlClient;
using System.Threading;

namespace EMS
{
    class EmployeeLogin : DataVarificationFromDB
    {
        private string _connectionString { get; set; }
        private string _userName { get; set; }
        private double _salary { get; set; }

        public EmployeeLogin(string _userName, string _connectionString) : base(_connectionString)
        {
            this._userName = _userName;
            this._connectionString = _connectionString;
        }

       

        public void calculateSalary(SqlConnection sqlconnection)
        {
            
            try
            {
                Console.WriteLine("-------------->Enter the Number of working days: ");
                var wrokingDays = InputCheck.NumericCheck(" Working Days ");
                string query = @"SELECT DailyWages from Employee where userName = " + "'" + _userName + "'";
                SqlDataReader rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
                while (rdr.Read())
                {
                    var tmp = rdr.GetDouble(0);
                    _salary = (wrokingDays * tmp) / 30;
                    break;
                }

                rdr.Close();
                Console.WriteLine("-------------->_salary of this Month : " + _salary +
                                  "\n-------------->Press Any key to return");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                //return false;
            }
            
        }

        public void GetEmployeeDetails(SqlConnection sqlconnection)
        {
            
            try
            {
                Console.Clear();
                string query = @"SELECT * from Employee where userName = '" + _userName + "'";
                var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
                while (rdr.Read())
                {
                    string[] arr = new string[10]
                    {
                        "Employee ID: ", "Employee _firstName: ", "Employee Last Name: ", "_userName: ",
                        "Postion: ", "Date of Joining: ", "_mobile: ", "_email ID: ", "_salary: ",
                        "Monthly Fixed Payment"
                    };
                    for (var i = 0; i < 10; i++) Console.WriteLine(arr[i] + ":--------->" + rdr.GetValue(i) + "\n");
                    break;
                }

                rdr.Close();
                Console.WriteLine("\n-------------->Press Any key to return");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                        Console.WriteLine("-------------->!!!Select from the above Option: -!!!!");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }
    }

}