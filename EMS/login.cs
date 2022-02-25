using System;
using System.Data.SqlClient;
using System.Threading;

namespace EMS
{
    class login : DataVarificationFromDB
    {
        
        private bool _isAdmin { get; set; }
        private string _connectionString { get; set; }

        public login(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public void logging()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Login :  ");
                    Console.WriteLine("Enter User Name :");
                    var _userName = InputCheck.StringCheck("User Name");
                    Console.WriteLine("Enter Password :");

                    //Inputcheck.ReadPassword() to hide the password from the screen...
                    //Inputcheck.ComputeSha256() for generating sha256 of the password...

                    var _password = InputCheck.ComputeSha256Hash(InputCheck.ReadPassword());
                    using (var sqlconnection = new SqlConnection(_connectionString))
                    {
                        //verifying the credentials from DB using the sqlQuery...
                        sqlconnection.Open();
                        var sqlQuery = @"SELECT IsAdmin  from Credentials where UserName = " + "'" + _userName + "'" +
                                    " and Password = " + "'" + _password + "'";
                        var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection);
                        if (rdr.Read())
                        {
                            //getting the bool value from the reader to decide the login type...
                            _isAdmin = rdr.GetBoolean(0);
                            rdr.Close();
                            Console.WriteLine("\nLogin Success full ");
                            if (_isAdmin)
                            {
                                Console.WriteLine("Logged in As Admin ");
                                Thread.Sleep(1000);
                                var obj = new AdminLogin();
                                obj.AdminOption(sqlconnection);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\nLogged in As Employee: ");
                                Thread.Sleep(1000);
                                var obj = new EmployeeLogin(_userName);
                                obj.EmployeeOption(sqlconnection);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                    //return false;
                }

                Console.WriteLine(
                    "\n-------------->!!!Wrong _userName _password!!!!<--------------\n--------------> In case Forget _userName and _password plz contact Admin<--------------  ");
                Console.WriteLine("\n-------------->press:" + "\n-------------->1. Re-enter _userName and _password: " +
                                  "\n-------------->2.Press any key to Return previous menu:");
                var check = Console.ReadLine();
                if (check == "1") continue;
                break;
            }
        }
    }
}