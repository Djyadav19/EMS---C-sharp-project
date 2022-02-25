using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;

namespace EMS
{
    internal class Manipulation : DataVarificationFromDB
    {
        private string _connectionString { get; set; }
        private string _userName { get; set; }
        private double _empID { get; set; }
        public SqlConnection sqlconnection { get; set; }

        public Manipulation(double _empID, string _connectionString, SqlConnection sqlconnection) : base(_connectionString)
        {
            this._empID = _empID;
            this._connectionString = _connectionString;
            this.sqlconnection = sqlconnection;
        }

        public void UpdateMobile_Email(string s, Regex r)
        {
            try
            {
                //Regex r = new Regex(@"^[0-9]{10}$");
                var input = InputCheck.RegexCheck(r , s);
                input = "'" + input + "'";
                var sql = "";
                sql = @"update  Employee set " + s + " = " + input + " where empID = '" + _empID + "'";
                SqlQuery.ExecuteUpdateQuery(sql, sqlconnection);
                

                Console.WriteLine("-------------->" + s + " Updated:\n-------------->Press Any key to return");
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                //return false;
            }
        }

        public void UpdateAdmin()
        {
            try
            {
                
                var query = @"SELECT userName from Employee where empID = " + _empID;
                var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
                while (rdr.Read())
                {
                    _userName = rdr.GetString(0);
                    break;
                }

                rdr.Close();
                Console.Clear();
                Console.WriteLine("Appoint as  : ");
                short isAdmin = 0;
                while (true)
                {
                    Console.WriteLine("1.Admin" + "\n2.Employee");
                    var choice = InputCheck.NumericCheck("choice");
                    switch (choice)
                    {
                        case 1:
                            if (IsAdmin_count(_userName, sqlconnection))
                            {
                                isAdmin = 1;
                                break;
                            }

                            Console.WriteLine("You are the single Admin :" + "Redirecting to previous menu ");
                            break;
                        case 2:
                            if (IsAdmin_count(_userName, sqlconnection))
                            {
                                isAdmin = 0;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("You are the single Admin :" + "Redirecting to previous menu ");
                                Thread.Sleep(1500);
                                return;
                            }
                        default:
                            Console.Clear();
                            Console.WriteLine("-------------->!!!Select from the above Option: -!!!!");
                            continue;
                    }

                    break;
                }
                var sql = @"update  Credentials set IsAdmin = " + isAdmin + " where UserName = '" + _userName + "'";
                SqlQuery.ExecuteUpdateQuery(sql, sqlconnection);
                
                Console.WriteLine("-------------->Admin type Updated:\n-------------->Press Any key to return");
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                //return false;
            }
        }

        public void Option()
        {
            while (true)
            {
                Console.WriteLine(@"1.Update _mobile: :
2.Update _email:
3.Update Employee Type:
4.Exit:
 ");
                var choice = InputCheck.NumericCheck("choice");
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Enter _mobile : ");
                        UpdateMobile_Email("Mobile", new Regex(@"^[0-9]{10}$"));
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Enter EmailID : ");
                        UpdateMobile_Email("EmailID", new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"));
                        break;
                    case 3:
                        Console.Clear();
                        UpdateAdmin();
                        break;
                    case 4: return;
                    default:
                        Console.WriteLine("-------------->!!! Select from the above Option !!!!");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }
    }
}