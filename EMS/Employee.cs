using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace EMS
{
    internal class Employee : DataVarificationFromDB
    {
        private string _connectionString { get; set; }
        private double _empID { get; set; }
        private string _firstName { get; set; }
        private string _lastName { get; set; }
        private string _userName { get; set; }
        private string _position { get; set; }
        private DateTime _dojDateTime { get; set; }
        private string _mobile { get; set; }
        private string _email { get; set; }
        private double _salary;
        private string _montlyFixedSalary;
        private bool _isAdmin { get; set; }
        private string _password;

        public Employee(string _connectionString) : base(_connectionString)
        {
            this._connectionString = _connectionString;
        }

        public void SetDataEmployee(SqlConnection sqlconnection)
        {
            //SqlCommand cmd;
            string sql;
            sql = "";
            try
            {
                //
                Console.Clear();
                _empID = EmpId_assigning(sqlconnection);
                Console.WriteLine("Your Emp ID is : " + _empID + "\nPlease Note It");
                Console.WriteLine("Enter First Name: ");
                Regex r;
                r = new Regex(@"^[a-zA-Z][a-zA-Z0-9 ]+[a-z0-9A-Z]{1,30}$");
                ;
                _firstName = InputCheck.RegexCheck(r," First Name");
                Console.WriteLine("Enter Last Name: ");
                r = new Regex(@"^[a-zA-Z][a-zA-Z0-9 ]+[a-z0-9A-Z]{1,30}$");
                _lastName = InputCheck.RegexCheck(r, " Last name");
                Console.WriteLine("Create a _userName: ");
                while (true)
                {
                    string checkUserName = null;
                    Console.WriteLine("Enter _userName : \n" +
                                      "Size of _userName must be between greater than 2 and smaller than 29: ");
                    r = new Regex(@"^[A-Za-z][A-Za-z0-9_]{3,29}$");
                    var input = InputCheck.RegexCheck(r," Username ");
                    //adapter = new SqlDataAdapter();
                    var query = @"SELECT userName from Employee where userName = '" + input + "'";
                    using (var cmd = new SqlCommand(query, sqlconnection))
                    {
                        var rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            checkUserName = rdr.GetString(0);
                            break;
                        }

                        rdr.Close();
                    }

                    if (input == checkUserName)
                    {
                        Console.WriteLine(
                            "-------------->!!! _userName already Exists !!!\n-------------->Please Use a different _userName:   ");
                        continue;
                    }

                    _userName = string.Copy(input);
                    break;
                }

                Console.WriteLine("Create a _password: ");
                _password = InputCheck.ComputeSha256Hash(InputCheck.ReadPassword());
                Console.WriteLine("\nAppoint _position : ");
                _position = InputCheck.StringCheck("_position ");
                Console.WriteLine("Enter Date Of Joining: yyyy-MM-dd ");
                _dojDateTime = InputCheck.DateCheck();
                Console.WriteLine("Enter Per_month _salary : ");
                _montlyFixedSalary = Convert.ToString(InputCheck.NumericCheck("Per Month _salary"));
                Console.WriteLine("Enter _mobile : ");
                r = new Regex(@"^[0-9]{10}$");
                _mobile = InputCheck.RegexCheck(r," Mobile");
                Console.WriteLine("Enter _email: ");
                r = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                _email = InputCheck.RegexCheck(r," Email Id ");
                Console.WriteLine("Select the Type of Employee\n");
                _isAdmin = InputCheck.IsBoolean();
                var date = "'" + Convert.ToString(_dojDateTime) + "'";
                
                sql = @"insert into Employee values (" + Convert.ToString(_empID) + "," + "'" + _firstName + "'" + "," + "'" + _lastName + "'" +
                      "," + "'" + _userName + "'" + "," + "'" + _position + "'" + "," + date + "," + "'" + _mobile + "'" + "," + "'" + _email + "'" + "," + "NULL" + "," +
                      _montlyFixedSalary + ")";
                SqlQuery.ExecuteInsertQuery(sql, sqlconnection);
                sql = @"insert into Credentials values (" + "'" + _userName + "'" + "," + "'" + _password + "'" + "," +
                      Convert.ToInt32(_isAdmin) + ")";
                SqlQuery.ExecuteInsertQuery(sql, sqlconnection);
                Console.WriteLine("Data saved successfully: " + "\nAuto-Redirecting to previous menu:");
                Thread.Sleep(1500);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                //return false;
            }
        }

        public void GetEmployeeDetails(SqlConnection sqlconnection)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Enter EMP ID To find the details: ");
                    _empID = InputCheck.NumericCheck("Emp Id");
                    if (EmpId_check(_empID, sqlconnection))
                    {
                        var query = @"SELECT * from Employee where empID = " + _empID;
                        var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
                        while (rdr.Read())
                        {
                            var arr = new string[10]
                            {
                                "Employee ID: ", "Employee _firstName: ", "Employee Last Name: ", "User Name: ",
                                "position: ", "Date of Joining: ", "_mobile: ", "_email ID: ", "salary: ",
                                "Monthly Fixed Payment"
                            };
                            for (var i = 0; i < 10; i++) Console.WriteLine(arr[i] + " " + rdr.GetValue(i) + "\n");
                            break;
                        }

                        rdr.Close();
                        Console.WriteLine("\n-------------->Press Any key to return");
                        Console.ReadLine();
                        break;
                    }

                    Console.WriteLine("-------------->_empID Is not present in Database: ");
                    Console.WriteLine("-------------->press:" + "\n-------------->1. Re-enter _empID: " +
                                      "\n-------------->2.Press any key to Return previous menu:");
                    var check = InputCheck.NumericCheck("choice");
                    if (check == 1) continue;
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                //return false;
            }
        }

        public void DeleteQueryExecution(SqlConnection sqlconnection)
        {
            try
            {
                SqlCommand cmd;
                var adapter = new SqlDataAdapter();
                var sql = "";
                sql = @"DELETE from Employee where empID = " + _empID;
                cmd = new SqlCommand(sql, sqlconnection);
                adapter.DeleteCommand = new SqlCommand(sql, sqlconnection);
                adapter.DeleteCommand.ExecuteNonQuery();
                _userName = "'" + _userName + "'";
                sql = @"DELETE from Credentials where userName = " + _userName;
                cmd = new SqlCommand(sql, sqlconnection);
                adapter.DeleteCommand = new SqlCommand(sql, sqlconnection);
                adapter.DeleteCommand.ExecuteNonQuery();
                Console.WriteLine("\n-------------->Deleted Successfully\n-------------->Press Any key to return");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                //return false;
            }
        }

        public void DelEmployeeDetails(SqlConnection sqlconnection)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Enter EMP ID To Delete the details: ");
                    _empID = InputCheck.NumericCheck("Emp Id");
                    if (EmpId_check(_empID, sqlconnection))
                    {
                        var query = @"SELECT userName from Employee where empID = " + _empID;
                        var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
                        while (rdr.Read())
                        {
                            _userName = rdr.GetString(0);
                            break;
                        }

                        rdr.Close();
                        if (IsAdmin_Check(_userName, sqlconnection))
                        {
                            if (IsAdmin_count(_userName, sqlconnection))
                            {
                                DeleteQueryExecution(sqlconnection);
                                break;
                            }

                            Console.WriteLine("-------------->Sir !!! You are the single Admin: ");
                            Thread.Sleep(3000);
                            break;
                        }

                        DeleteQueryExecution(sqlconnection);
                        break;
                    }

                    Console.WriteLine("-------------->This _empID is not present in Data base :");
                    Console.WriteLine("-------------->press:" + "\n-------------->1. Re-enter _empID: " +
                                      "\n-------------->2.Press any key to Return previous menu:");
                    var check = InputCheck.NumericCheck("Choice");
                    if (check == 1) continue;
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                //return false;
            }
        }

        public void Payslip()
        {
            string fileName = @"E:\C# training\EMS\SalarySlip\" + _userName + "_" + _empID + ".txt";
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                string text = "Salary of this Month: --------------> " + _salary;
                System.IO.File.WriteAllText(fileName, text);
                Console.WriteLine("Pay slip for EmpID: " + _empID + " is generated at " + fileName+"\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                
            }
        }

        public void calculateSalary(SqlConnection sqlconnection)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Enter EMP ID To calculate the _salary: ");
                    _empID = InputCheck.NumericCheck("Emp Id");
                    if (EmpId_check(_empID, sqlconnection))
                    {
                        Console.WriteLine("Enter the Number of working days: ");
                        var wrokingDays = InputCheck.NumericCheck("Days");
                        var query = @"SELECT DailyWages and username from Employee where empID = " + _empID;
                        var rdr = SqlQuery.ExecuteSelectQuery(query, sqlconnection);
                        double tmp = 0;
                        while (rdr.Read())
                        {
                            _userName = rdr.GetString(0); 
                            tmp = rdr.GetDouble(1);
                            _salary = wrokingDays * tmp / 30;
                            break;
                        }

                        rdr.Close();
                        if (tmp > 0)
                        {
                            var adapter = new SqlDataAdapter();
                            var sql = "";
                            var sal = Convert.ToString(_salary);
                            sql = @"update  Employee set salary = " + sal + "where empID = " + _empID;
                            SqlQuery.ExecuteUpdateQuery(sql, sqlconnection);
                            Payslip();
                            Console.WriteLine("Salary of this Month :--------------> " + _salary +
                                              "\nPress Any key to return");
                            Console.ReadLine();
                            break;
                        }
                        Console.WriteLine("Update  Per_month _salary First :");
                        Console.WriteLine("Enter any key to return: ");
                        Console.ReadLine();
                        break;
                    }

                    Console.WriteLine("-------------->_empID Is not present in Database: ");
                    Console.WriteLine("-------------->press:" + "\n-------------->1. Re-enter _empID: " +
                                      "\n-------------->2.Press any key to Return previous menu:");
                    var check = InputCheck.NumericCheck("Choice");
                    if (check == 1) continue;
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                //return false;
            }
        }
    }
}