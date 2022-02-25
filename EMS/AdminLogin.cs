using System;
using System.Data.SqlClient;
using System.Threading;

namespace EMS
{
    class AdminLogin 
    {
        //public SqlConnection sqlconnection { get; set; }
        private string _connectionString { get; set; }

        public AdminLogin(string _connectionString) //: base(_connectionString)
        {
            this._connectionString = _connectionString;
        }

        public void AdminOption(SqlConnection sqlconnection)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" ---->Logged in As Admin\n ");
                Console.WriteLine(@"1.To add an Employee:
2.To Manipulate Employee Details:
3.To Retrieve  Employee Details:
4.To Delete Employee Details:
5.TO generate Payroll.
6.Exit ");
                var choice = InputCheck.NumericCheck("choice");
                Employee obj;
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        obj = new Employee(_connectionString);
                        obj.SetDataEmployee(sqlconnection);
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("-------------->Enter EMP ID To Update the details: ");
                        var _empID = InputCheck.NumericCheck("Emp Id");
                        var obj2 = new DataVarificationFromDB(_connectionString);
                        if (obj2.EmpId_check(_empID, sqlconnection))
                        {
                            var obj1 = new Manipulation(_empID, _connectionString, sqlconnection);
                            obj1.Option();
                        }
                        else
                        {
                            Console.WriteLine("-------------->_empID Is not present in Database: ");
                            Console.WriteLine("-------------->press:" + "\n-------------->1. Re-enter _empID: " +
                                              "\n-------------->2.Press any key to Return previous menu:");
                            var check = Console.ReadLine();
                            if (check == "1") goto case 2;
                        }

                        break;
                    case 3:
                        Console.Clear();
                        //Console.WriteLine("check kar raha hun bhai");
                        obj = new Employee(_connectionString);
                        obj.GetEmployeeDetails(sqlconnection);
                        break;
                    case 4:
                        Console.Clear();
                        obj = new Employee(_connectionString);
                        obj.DelEmployeeDetails(sqlconnection);
                        return;
                    case 5:
                        Console.Clear();
                        obj = new Employee(_connectionString);
                        //obj.Payslip();
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