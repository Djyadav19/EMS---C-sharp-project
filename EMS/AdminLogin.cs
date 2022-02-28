using System;
using System.Data.SqlClient;
using System.Threading;

namespace EMS
{
    class AdminLogin
    {
        private string _userName { get; set; }

        private void ToGetUsernameForManipulation()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Enter EMP ID To Update the details: ");
                var empID = InputCheck.NumericCheck("Emp Id");
                var obj2 = new DataVarificationFromDB();
                if (obj2.EmpIdCheck(empID))
                {
                    var sqlQuery = @"SELECT userName from Employee where empID = " + empID;
                    try
                    {
                        using (var userNameReader = SqlQuery.ExecuteSelectQuery(sqlQuery))
                        {
                            while (userNameReader.Read())
                            {
                                _userName = userNameReader.GetString(0);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Press any key to return...");
                        Console.ReadLine();
                    }

                    var obj1 = new Manipulation(_userName);
                    obj1.Option();
                }
                else
                {
                    Console.WriteLine("!!!Emp ID Is not present in Database: !!!" + "\npress:" +
                                      "\n1. Re-enter _empID: " + "\n2.Press any key to Return previous menu:");
                    var check = Console.ReadLine();
                    if (check == "1") continue;
                }

                break;
            }
        }

        public void AdminOption()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" ---->Logged in As Admin<---- " + "\n1.To add an Employee: " +
                                  "\n2.To Manipulate Employee Details: " + "\n3.To Retrieve  Employee Details: " +
                                  "\n4.To Delete Employee Details: " + "\n5.TO generate Payroll: " + "\n6.Exit ");
                var choice = InputCheck.NumericCheck("choice");
                Employee obj;
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        obj = new Employee();
                        obj.SetDataEmployee();
                        break;
                    case 2:
                        ToGetUsernameForManipulation();
                        break;
                    case 3:
                        Console.Clear();
                        obj = new Employee();
                        obj.GetEmployeeDetails();
                        break;
                    case 4:
                        Console.Clear();
                        obj = new Employee();
                        var flag = obj.DelEmployeeDetails();
                        if (flag) return;
                        break;
                    case 5:
                        Console.Clear();
                        obj = new Employee();
                        obj.calculateSalary();
                        break;
                    case 6: return;
                    default:
                        Console.WriteLine("!!! Select From the above Option !!!!");
                        Thread.Sleep(1500);
                        break;
                }
            }
        }
    }
}