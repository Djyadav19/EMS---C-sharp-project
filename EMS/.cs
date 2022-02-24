using System;
using System.Data.SqlClient;

namespace EMS
{
    class HelpMethod
    {
        public string _connectionString { get; set; }

        public HelpMethod(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

       

        public int EmpId_assigning(SqlConnection sqlconnection)
        {
            var tmp = 0;
            try
            {
                //var adapter = new SqlDataAdapter();
                var query = @"SELECT MAX(_empID) FROM Employee";
                using (var cmd = new SqlCommand(query, sqlconnection))
                {
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        tmp = rdr.GetInt32(0);
                        break;
                    }

                    rdr.Close();
                }

                return tmp + 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return -1;
            }
        }

        

        public bool EmpId_check(double _empID, SqlConnection sqlconnection)
        {
            try
            {
                var adapter = new SqlDataAdapter();
                var query = @"SELECT _empID from Employee where _empID = " + _empID;
                var cmd = new SqlCommand(query, sqlconnection);
                var rdr = cmd.ExecuteReader();
                double tmp = 0;
                while (rdr.Read())
                {
                    var tmpID = rdr.GetInt32(0);
                    tmp = Convert.ToDouble(tmpID);
                    break;
                }

                rdr.Close();
                if (tmp == _empID) return true;
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return false;
            }
        }

        public bool IsAdmin_count(string _userName, SqlConnection sqlconnection)
        {
            try
            {
                var _isAdmin = false;
                var adapter = new SqlDataAdapter();
                var query = @"SELECT _isAdmin from Credentials";
                var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
                var count = 0;
                while (rdr.Read())
                {
                    _isAdmin = rdr.GetBoolean(0);
                    if (_isAdmin)
                    {
                        count++;
                        if (count > 1)
                        {
                            rdr.Close();
                            return true;
                        }
                    }
                }

                rdr.Close();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return false;
            }
        }

        public string ReadPassword()
        {
            while (true)
            {
                var _password = "";
                var info = Console.ReadKey(true);
                while (info.Key != ConsoleKey.Enter)
                {
                    if (info.Key != ConsoleKey.Backspace)
                    {
                        Console.Write("*");
                        _password += info.KeyChar;
                    }
                    else if (info.Key == ConsoleKey.Backspace)
                    {
                        if (!string.IsNullOrEmpty(_password))
                        {
                            // remove one character from the list of _password characters
                            _password = _password.Substring(0, _password.Length - 1);
                            // get the location of the cursor
                            int pos = Console.CursorLeft;
                            // move the cursor to the left by one character
                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                            // replace it with space
                            Console.Write(" ");
                            // move the cursor to the left by one character again
                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        }
                    }

                    info = Console.ReadKey(true);
                }

                if (string.IsNullOrWhiteSpace(_password))
                {
                    Console.WriteLine("_password Can't be NuLL");
                    continue;
                }

                return _password;
            }
        }

        

        public bool IsAdmin_Check(string _userName, SqlConnection sqlconnection)
        {
            try
            {
                var query = @"SELECT _isAdmin  from Credentials where _userName = '" + _userName + "'";
                var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
                while (rdr.Read())
                {
                    var flag = rdr.GetBoolean(0);
                    rdr.Close();
                    return flag;
                }
                rdr.Close();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return false;
            }
        }
    }
}