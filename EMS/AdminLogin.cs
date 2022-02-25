using System;
using System.Data.SqlClient;
using System.Threading;

namespace EMS
{
    class AdminLogin 
    {
        public SqlConnection sqlconnection { get; set; }
        private string _userName { get; set; }

        private void ToGetUsernameForManipulation(SqlConnection sqlconnection)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-------------->Enter EMP ID To Update the details: ");
                var _empID = InputCheck.NumericCheck("Emp Id");
                var obj2 = new DataVarificationFromDB();
                if (obj2.EmpIdCheck(_empID, sqlconnection))
                {
                    var sqlQuery = @"SELECT userName from Employee where empID = " + _empID;
                    try
                    {

                        using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection))
                        {
                            while (rdr.Read())
                            {
                                _userName = rdr.GetString(0);
                                break;
                            }
                            //rdr.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Press any key to return...");
                        Console.ReadLine();
                    }

                    var obj1 = new Manipulation(_userName, _empID, sqlconnection);
                    obj1.Option();
                }
                else
                {
                    Console.WriteLine("-------------->_empID Is not present in Database: ");
                    Console.WriteLine("-------------->press:" + "\n-------------->1. Re-enter _empID: " +
                                      "\n-------------->2.Press any key to Return previous menu:");
                    var check = Console.ReadLine();
                    if (check == "1") continue;
                }
                break;
            }

        }
        
        public void AdminOption(SqlConnection sqlconnection)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" ---->Logged in As Admin ");
                Console.WriteLine("\n1.To add an Employee: "+
                                  "\n2.To Manipulate Employee Details: "+
                                  "\n3.To Retrieve  Employee Details: "+
                                  "\n4.To Delete Employee Details: "+
                                  "\n5.TO generate Payroll: "+
                                  "\n6.Exit ");
                var choice = InputCheck.NumericCheck("choice");
                Employee obj;
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        obj = new Employee();
                        obj.SetDataEmployee(sqlconnection);
                        break;
                    case 2:
                        ToGetUsernameForManipulation(sqlconnection);
                        break;
                    case 3:
                        Console.Clear();
                        
                        obj = new Employee();
                        obj.GetEmployeeDetails(sqlconnection);
                        break;
                    case 4:
                        Console.Clear();
                        obj = new Employee();
                        var flag =obj.DelEmployeeDetails(sqlconnection);
                        if(flag) return;
                        break;
                    case 5:
                        Console.Clear();
                        obj = new Employee();
                        obj.calculateSalary(sqlconnection);
                        break;
                    case 6: return;
                    default:
                        Console.WriteLine("-------------->!!! Select From the above Option !!!!");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }
    }
}