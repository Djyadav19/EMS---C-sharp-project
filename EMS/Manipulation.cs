using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;

namespace EMS
{
    internal class Manipulation : DataVarificationFromDB
    {
        private string _userName { get; set; }
        private string _password { get; set; }
        private double _empID { get; set; }
        public SqlConnection sqlconnection { get; set; }

        public Manipulation(string _userName,double _empID, SqlConnection sqlconnection) 
        {
            this._userName = _userName;
            this._empID = _empID;
            
            this.sqlconnection = sqlconnection;
        }

        public void UpdateMobile_Email(string s, Regex r)
        {
            try
            {
                var input = InputCheck.RegexCheck(r, s);
                input = "'" + input + "'";
                var sqlQuery = "";
                sqlQuery = @"update  Employee set " + s + " = " + input + " where username = '" + _userName+ "'";
                SqlQuery.ExecuteUpdateQuery(sqlQuery, sqlconnection);
                Console.WriteLine("\n " + s + " Updated Successfully  \n Press Any key to return");
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

        public void PasswordAndAdminChange(string column, string value,string displayChangeType)
        {
            
            try
            {
                var sqlQuery = @"update  Credentials set " + column + " = '" + value + "' where UserName = '" + _userName + "'";
                SqlQuery.ExecuteUpdateQuery(sqlQuery, sqlconnection);
                Console.WriteLine("\n "+ displayChangeType +" Updated Successfully \n Press Any key to return");
                Console.ReadLine();
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
            }
        }

        //just for the shake of polymorphism 
        /*
        public void PasswordAndAdminChange(string column, short value)
        {
            var sqlQuery = @"SELECT userName from Employee where empID = " + _empID;
            using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection))
            {
                while (rdr.Read())
                {
                    _userName = rdr.GetString(0);
                    break;
                }
                //rdr.Close();
            }
            sqlQuery = @"update  Credentials set " + column + " = " + value + " where UserName = '" + _userName + "'";
            SqlQuery.ExecuteUpdateQuery(sqlQuery, sqlconnection);
            
        }*/
        public void UpdateAdmin()
        {
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
                        isAdmin = 1;
                        break;
                        
                    case 2:
                        if (AdminCount(_userName, sqlconnection) && IsAdmin(_userName,sqlconnection))
                        {
                            isAdmin = 0;
                            break;
                        }
                        else if (!IsAdmin(_userName, sqlconnection))
                        {
                            isAdmin = 0;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("You are the single Admin :" + "\nRedirecting to previous menu ");
                            Thread.Sleep(1500);
                            return;
                        }
                    default:
                        Console.Clear();
                        Console.WriteLine("!!! Select from the above Option !!!!");
                        continue;
                }

                PasswordAndAdminChange("isAdmin", Convert.ToString(isAdmin),"Employee Type");
                break;
            }
            
        }

        public void Option()
        {
            while (true)
            {
                Console.WriteLine(" 1.Update _mobile: " + "\n 2.Update _email: " + "\n 3.Update Employee Type: " +
                                  "\n 4.To change Password :" +
                                  "\n 5.Exit: ");
                var choice = InputCheck.NumericCheck("choice");
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.Write("\nEnter Mobile : ");
                        UpdateMobile_Email("Mobile", new Regex(@"^[0-9]{10}$"));
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
                        //_password = "'" + _password + "'";
                        PasswordAndAdminChange("Password", _password,"Password");
                        break;
                    case 5:
                        return;
                    default:
                        Console.Write("!!! Select from the above Option !!!");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }
    }
}