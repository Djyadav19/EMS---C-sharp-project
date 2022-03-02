using System;
using System.Threading.Tasks;

namespace EMS
{
    
    class AdminLogin
    {
        private string _loggedInUserName { get; set; }
        private string _userName { get; set; }

        public AdminLogin(string _loggedInUser)
        {
            _loggedInUserName = _loggedInUser;
        }

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

                    var obj = new Manipulation(_userName);
                    obj.Option();
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

        public async Task AdminOption()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" ---->Logged in As Admin<---- ");
                MenuUsingEnum.AdminOptions();
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
                        var isAdminDeleted = obj.DelEmployeeDetails(_loggedInUserName);
                        if (Convert.ToBoolean(isAdminDeleted)) return;
                        break;
                    case 5:
                        Console.Clear();
                        obj = new Employee();
                        obj.calculateSalary();
                        break;
                    case 6: return;
                    default:
                        Console.WriteLine("!!! Select From the above Option !!!!");
                        await Task.Delay(1500);
                        break;
                }
            }
        }
    }
}