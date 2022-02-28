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

        public void calculateSalary()
        {
            try
            {
                Console.Write(" Enter the Number of working days:  ");
                var wrokingDays = InputCheck.NumericCheck(" Working Days ");
                string sqlQuery = @"SELECT DailyWages from Employee where userName = " + "'" + _userName + "'";
                using (var monthlySalaryDataReaderReader = SqlQuery.ExecuteSelectQuery(sqlQuery))
                {
                    while (monthlySalaryDataReaderReader.Read())
                    {
                        var tmp = monthlySalaryDataReaderReader.GetDouble(0);
                        _salary = (wrokingDays * tmp) / 30;
                        break;
                    }
                }

                Console.WriteLine("\nSalary of this Month : " + _salary + "\n\nPress Any key to return");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\nPress any key to return...");
                Console.ReadLine();
            }
        }

        public void GetEmployeeDetails()
        {
            try
            {
                Console.Clear();
                string sqlQuery = @"SELECT * from Employee where userName = '" + _userName + "'";
                using (var employeeDetailsReader = SqlQuery.ExecuteSelectQuery(sqlQuery))
                {
                    var table = new ConsoleTable(" Employee ID ",
                        " Employee _firstName ",
                        " Last Name ",
                        " User Name ",
                        " Position ",
                        " Date of Joining ",
                        " Mobile ",
                        " Email ID ",
                        " Salary ",
                        " Monthly Fixed Payment ");
                    while (employeeDetailsReader.Read())
                    {
                        var i = 0;
                        table.AddRow(employeeDetailsReader.GetValue(i++),
                            employeeDetailsReader.GetValue(i++),
                            employeeDetailsReader.GetValue(i++),
                            employeeDetailsReader.GetValue(i++),
                            employeeDetailsReader.GetValue(i++),
                            employeeDetailsReader.GetValue(i++),
                            employeeDetailsReader.GetValue(i++),
                            employeeDetailsReader.GetValue(i++),
                            employeeDetailsReader.GetValue(i++),
                            employeeDetailsReader.GetValue(i++));
                        break;
                    }

                    Console.Clear();
                    Console.WriteLine(table);
                }

                Console.WriteLine("\nPress Any key to return");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\nPress any key to return...");
                Console.ReadLine();
            }
        }

        public void EmployeeOption()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" ---->Logged in As Employee\n " +
                                  "\n1.To Retrieve  Employee Details: \n2.TO generate Payroll:    \n3.Exit ");
                var choice = InputCheck.NumericCheck("choice");
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        GetEmployeeDetails();
                        break;
                    case 2:
                        Console.Clear();
                        calculateSalary();
                        break;
                    case 3:
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("!!! Select from the above Option !!!");
                        Thread.Sleep(1500);
                        break;
                }
            }
        }
    }
}