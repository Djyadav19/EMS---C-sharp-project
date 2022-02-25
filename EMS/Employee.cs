using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using ConsoleTables;

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

        public void SetDataEmployee(SqlConnection sqlconnection)
        {
            try
            {
                //
                Console.Clear();
                _empID = EmpIdAssigning(sqlconnection);
                Console.WriteLine("Your Emp ID is : " + _empID + "\nPlease Note It");
                Console.WriteLine("Enter First Name: ");
                var r = new Regex(@"^[a-zA-Z][a-zA-Z0-9 ]{1,15}[a-z0-9A-Z]{1,15}$");
                ;
                _firstName = InputCheck.RegexCheck(r," First Name");
                Console.WriteLine("Enter Last Name: ");
                r = new Regex(@"^[a-zA-Z][a-zA-Z0-9 ]{1,15}[a-z0-9A-Z]{1,15}$");
                _lastName = InputCheck.RegexCheck(r, " Last name");
                Console.WriteLine("Create a _userName: ");
                string sqlQuery;
                while (true)
                {
                    string checkUserName = null;
                    Console.WriteLine("Enter _userName : \n" +
                                      "Size of _userName must be between greater than 3 and smaller than 29: ");
                    r = new Regex(@"^[A-Za-z][A-Za-z0-9_]{3,30}$");
                    var input = InputCheck.RegexCheck(r," Username ");
                    sqlQuery = @"SELECT userName from Employee where userName = '" + input + "'";
                    using (var cmd = new SqlCommand(sqlQuery, sqlconnection))
                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                checkUserName = rdr.GetString(0);
                                break;
                            }
                            //rdr.Close();
                        }
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

                //sqlQuery sqlQuery for inserting Data in DB
                sqlQuery = @"insert into Employee values (" + Convert.ToString(_empID) + "," + "'" + _firstName + "'" + "," + "'" + _lastName + "'" +
                           "," + "'" + _userName + "'" + "," + "'" + _position + "'" + "," + date + "," + "'" + _mobile + "'" + "," + "'" + _email + "'" + "," + "NULL" + "," +
                           _montlyFixedSalary + ")";
                //calling Function to execute the query...
                SqlQuery.ExecuteInsertQuery(sqlQuery, sqlconnection);
                sqlQuery = @"insert into Credentials values (" + "'" + _userName + "'" + "," + "'" + _password + "'" + "," +
                      Convert.ToInt32(_isAdmin) + ")";
                //calling Function to execute the query...
                SqlQuery.ExecuteInsertQuery(sqlQuery, sqlconnection);
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
                    string check;
                    Console.WriteLine("-------------->press:" + "\n-------------->1.To display All Employee Details :  " +
                                      "\n-------------->2.Enter Emp ID to find the details :" +
                                      "\n-------------->Press any key to Return previous menu:");
                    check = Console.ReadLine();
                    AllDisplay:
                    if (check == "1")
                    {
                        
                        var sqlQuery = @"SELECT * from Employee";
                        using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection))
                        {
                            var table = new ConsoleTable(" Employee ID ", " Employee _firstName ",
                                " Last Name ", " User Name ",
                                " Position ", " Date of Joining ", " Mobile ", " Email ID ", " Salary ",
                                " Monthly Fixed Payment ");
                            
                            while (rdr.Read())
                            {
                                var i = 0;
                                table.AddRow(rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++));
                                
                            }
                            Console.Clear();
                            Console.WriteLine(table);
                            
                        }
                        Console.WriteLine("\n-------------->Press any key to Return previous menu:");
                        Console.ReadLine();
                        break;
                    }

                    if (check == "2")
                    {
                        SingleDisplay:
                        Console.Clear();
                        Console.WriteLine("Enter EMP ID To find the details: ");
                        _empID = InputCheck.NumericCheck("Emp Id");

                        if (EmpIdCheck(_empID, sqlconnection))
                        {
                            var sqlQuery = @"SELECT * from Employee where empID = " + _empID;
                            using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection))
                            {
                                var table = new ConsoleTable(" Employee ID ", " Employee _firstName ",
                                    " Last Name ", " User Name ",
                                    " Position ", " Date of Joining ", " Mobile ", " Email ID ", " Salary ",
                                    " Monthly Fixed Payment ");
                                while (rdr.Read())
                                {
                                    var i = 0;
                                    table.AddRow(rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++), rdr.GetValue(i++));
                                    break;
                                }
                                Console.Clear();
                                Console.WriteLine(table);
                            }
                            Console.WriteLine("-------------->press:" +
                                              "\n-------------->1. Re-enter Another EmpID : " +
                                              "\n-------------->2. To display All Employee Details :" +
                                              "\n-------------->Press any key to Return previous menu:");
                            check = Console.ReadLine();
                            if (check == "1") goto SingleDisplay;
                            if (check == "2")
                            {
                                check = "1";
                                goto AllDisplay;

                            }
                            break;
                        }
                        Console.WriteLine("EmpId doesn't exists: ");
                        Console.WriteLine("-------------->press:" +
                                          "\n-------------->1. Re-enter Another EmpID : " +
                                          "\n-------------->2. To display All Employee Details :" +
                                          "\n-------------->Press any key to Return previous menu:");
                        check = Console.ReadLine();
                        if (check == "1") goto SingleDisplay;
                        if (check == "2")
                        {
                            check = "1";
                            goto AllDisplay;

                        }
                        break;
                    }
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
                var adapter = new SqlDataAdapter();
                var sqlQuery = @"DELETE from Employee where empID = " + _empID;
                SqlQuery.ExecuteDeleteQuery(sqlQuery,sqlconnection);
                _userName = "'" + _userName + "'";
                sqlQuery = @"DELETE from Credentials where userName = " + _userName;
                SqlQuery.ExecuteDeleteQuery(sqlQuery, sqlconnection);
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

        public bool DelEmployeeDetails(SqlConnection sqlconnection)
        {
            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Enter EMP ID To Delete the details: ");
                    _empID = InputCheck.NumericCheck("Emp Id");
                    if (EmpIdCheck(_empID, sqlconnection))
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

                        if (IsAdmin(_userName, sqlconnection))
                        {
                            if (AdminCount(_userName, sqlconnection))
                            {
                                DeleteQueryExecution(sqlconnection);
                                return true;
                            }

                            Console.WriteLine("-------------->Sir !!! You are the single Admin: ");
                            Thread.Sleep(3000);
                            break;
                        }

                        DeleteQueryExecution(sqlconnection);
                        
                    }

                    Console.WriteLine("-------------->This _empID is not present in Data base :");
                    Console.WriteLine("-------------->press:" + "\n-------------->1. Re-enter _empID: " +
                                      "\n-------------->2.Press any key to Return previous menu:");
                    var check = Console.ReadLine();
                    if (check == "1") continue;
                    break;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to return...");
                Console.ReadLine();
                return false;
            }
        }

        public void Payslip()
        {
            string fileName = @"E:\C# training\EMS\SalarySlip\" + _userName + "_" + _empID + ".txt";
            try
            {
                // Check for file already exists. If yes, delete it.     
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
                    if (EmpIdCheck(_empID, sqlconnection))
                    {
                        Console.WriteLine("Enter the Number of working days: ");
                        var wrokingDays = InputCheck.NumericCheck("Days");
                        var sqlQuery = @"SELECT Username, DailyWages from Employee where empID = " + _empID;
                        double tmp = 0;
                        using (var rdr = SqlQuery.ExecuteSelectQuery(sqlQuery, sqlconnection))
                        {
                            
                            while (rdr.Read())
                            {
                                _userName = rdr.GetString(0);
                                tmp = rdr.GetDouble(1);
                                _salary = wrokingDays * tmp / 30;
                                break;
                            }
                            //rdr.Close();
                        }

                        if (tmp > 0)
                        {
                            //var adapter = new SqlDataAdapter();
                            //sqlQuery = "";
                            var sal = Convert.ToString(_salary);
                            sqlQuery = @"update  Employee set salary = " + sal + "where empID = " + _empID;
                            SqlQuery.ExecuteUpdateQuery(sqlQuery, sqlconnection);
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
                    var check = Console.ReadLine();
                    if (check == "1") continue;
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