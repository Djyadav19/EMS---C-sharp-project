using System;
using System.Threading;
using System.Configuration;

namespace EMS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var _connectionString =
                (@"Data Source=DESKTOP-29CLIQB\SQLEXPRESS;Initial Catalog=EMS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            var connectionString1 =
                (@"Data Source=DESKTOP-29CLIQB\SQLEXPRESS;Initial Catalog=EMS1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            while (true)
            {
                Console.Clear();
                Console.WriteLine(@"Login in 
1. Company A :
2. Company B :
3. Exit  : ");
                short choice = 0;
                var input = short.TryParse(Console.ReadLine(), out choice);
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