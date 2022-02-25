using System;
using System.Threading;
using System.Configuration;

namespace EMS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //getting connection string from app.config...
            var _connectionString = ConfigurationManager.ConnectionStrings["str1"].ConnectionString;
            var connectionString1 = ConfigurationManager.ConnectionStrings["str2"].ConnectionString;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Login in " + "\n1. Company A : " + "\n2. Company B : " + "\n3. Exit  : ");
                short choice = 0;
                var input = short.TryParse(Console.ReadLine(), out choice);
                //calling the logging function for login...
                switch (choice)
                {
                    case 1:
                        var log = new login(_connectionString);
                        log.logging();
                        break;
                    case 2:
                        var log1 = new login(connectionString1);
                        log1.logging();
                        break;
                    case 3:
                        Console.WriteLine("!!!THANK YOU!!!");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("-------------->!!!Select from the above Option: -!!!!");
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
    }
}