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

        public void Logging()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Login :  ");
                    Console.Write("Enter User Name : ");
                    var userName = InputCheck.StringCheck("User Name");
                    Console.Write("Enter Password : ");

                    var password = InputCheck.ComputeSha256Hash(InputCheck.ReadPassword());
                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {

                        sqlConnection.Open();
                        var establishingSqlConnection = new SqlQuery(sqlConnection);
                        var sqlQuery = @"SELECT IsAdmin  from Credentials where UserName = " + "'" + userName + "'" +
                                       " and Password = " + "'" + password + "'";

                        var isAdminDataReader = SqlQuery.ExecuteSelectQuery(sqlQuery);

                        if (isAdminDataReader.Read())
                        {
                            _isAdmin = isAdminDataReader.GetBoolean(0);
                            isAdminDataReader.Close();

                            Console.WriteLine("\nLogin Success full ");
                            if (_isAdmin)
                            {
                                var obj = new AdminLogin();
                                obj.AdminOption();
                                break;
                            }
                            else
                            {
                                var obj = new EmployeeLogin(userName);
                                obj.EmployeeOption();
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
                    "\n!!!Wrong User Name Password!!!\n In case Forget User Name and Password plz contact Admin " +
                    "\npress:" +
                    "\n1. Re-enter User Name and Password: " +
                    "\n2.Press any key to Return previous menu:");
                var check = Console.ReadLine();
                if (check == "1")
                    continue;
                break;
            }
        }
    }
}