using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EMS
{
    internal class Manipulation : DataVarificationFromDB
    {
        private string _userName { get; set; }
        private string _password { get; set; }

        public Manipulation(string userName)
        {
            this._userName = userName;
        }

        public void UpdateMobile_Email(string column, Regex regex)
        {
            try
            {
                var inputForUpdate = InputCheck.RegexCheck(regex, column);
                inputForUpdate = "'" + inputForUpdate + "'";
                var sqlQuery = "";
                sqlQuery = @"update  Employee set " + column + " = " + inputForUpdate + " where username = '" + _userName + "'";
                SqlQuery.ExecuteUpdateQuery(sqlQuery);
                Console.WriteLine("\n " + column + " Updated Successfully  \n Press Any key to return");
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\nPress any key to return...");
                Console.ReadLine();
            }
        }

        public void PasswordAndAdminChange(string column, string value, string displayChangeType)
        {
            try
            {
                var sqlQuery = @"update  Credentials set " + column + " = '" + value + "' where UserName = '" +
                               _userName + "'";
                SqlQuery.ExecuteUpdateQuery(sqlQuery);
                Console.WriteLine("\n " + displayChangeType + " Updated Successfully \n Press Any key to return");
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\nPress any key to return...");
                Console.ReadLine();
            }
        }

        public  void UpdateAdmin()
        {
            Console.Clear();
            Console.WriteLine("Appoint as  : ");
            while (true)
            {
                Console.WriteLine("1.Admin" + "\n2.Employee");
                var choice = InputCheck.NumericCheck("choice");
                short isAdmin = 0;
                switch (choice)
                {
                    case 1:
                        isAdmin = 1;
                        break;
                    case 2:
                        if (AdminCount(_userName) && IsAdmin(_userName))
                        {
                            isAdmin = 0;
                            break;
                        }
                        else if (!IsAdmin(_userName))
                        {
                            isAdmin = 0;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("You are the single Admin :" + "\nRedirecting to previous menu ");
                            Task.Delay(1500).GetAwaiter().GetResult();
                            return;
                        }
                    default:
                        Console.Clear();
                        Console.WriteLine("!!! Select from the above Option !!!!");
                        continue;
                }

                PasswordAndAdminChange("isAdmin", Convert.ToString(isAdmin), "Employee Type");
                break;
            }
        }

        public void Option()
        {
            while (true)
            {
                Console.WriteLine(" 1.Update Mobile Number : " + "\n 2.Update Email ID : " + "\n 3.Update Employee Type: " +
                                  "\n 4.To change Password :" + "\n 5.Previous Menu ");
                var choice = InputCheck.NumericCheck("choice");
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.Write("\nEnter Mobile : ");
                        UpdateMobile_Email("Mobile", new Regex(@"^[1-9]{1}[0-9]{9}$"));
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write("\nEnter EmailID : ");
                        UpdateMobile_Email("EmailID", new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"));
                        break;
                    case 3:
                        Console.Clear();
                        UpdateAdmin();
                        break;
                    case 4:
                        Console.Clear();
                        Console.Write("\nCreate a new Password: ");
                        _password = InputCheck.ComputeSha256Hash(InputCheck.ReadPassword());
                        PasswordAndAdminChange("Password", _password, "Password");
                        break;
                    case 5: return;
                    default:
                        Console.Write("!!! Select from the above Option !!!");
                        Task.Delay(1500).GetAwaiter().GetResult();
                        break;
                }
            }
        }
    }
}