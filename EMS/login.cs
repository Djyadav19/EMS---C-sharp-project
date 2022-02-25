using System;
using System.Data.SqlClient;
using System.Threading;

namespace EMS
{
    class login: DataVarificationFromDB
    {
        private string _userName { get; set; }
        private string _password { get; set; }
        private bool _isAdmin { get; set; }
        private string _connectionString { get; set; }

        public login(string _connectionString) : base(_connectionString)
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
                    var _password = InputCheck.ComputeSha256Hash(InputCheck.ReadPassword());
                    using (var sqlconnection = new SqlConnection(_connectionString))
                    {
                        sqlconnection.Open();
                        var query = @"SELECT IsAdmin  from Credentials where UserName = " +
                                    "'" + _userName + "'" + " and Password = "+ "'" + _password + "'";
                        var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
                        if (rdr.Read())
                        {
                            _isAdmin = rdr.GetBoolean(0);
                            rdr.Close();
                            
                            Console.WriteLine("\nLogin Success full ");
                            Thread.Sleep(1000);
                            if (_isAdmin)
                            {
                                Console.WriteLine("Logged in As Admin ");
                                Thread.Sleep(1000);
                                var obj = new AdminLogin(_connectionString);
                                obj.AdminOption(sqlconnection);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\nLogged in As Employee: ");
                                Thread.Sleep(1000);
                                var obj = new EmployeeLogin(_userName, _connectionString);
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