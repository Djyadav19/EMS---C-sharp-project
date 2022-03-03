using System;
using System.Configuration;
using System.Threading.Tasks;

namespace EMS
{
    internal class Program
    {
        public static string GettingConnectionString()
        {
            ERROR1:
            Console.Write("Enter Company Domain: ");
            var domain = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(domain))
            {
                Console.WriteLine("\nDomain can't be null: ");
                Console.WriteLine("\npress:" + "\n1. To Re-try : " + "\n   Press any key to Return previous menu:");
                var check = Console.ReadLine();
                if (check == "1") goto ERROR1;
                return "";
            }

            try
            {
                domain = domain.ToLower();
                Console.WriteLine(domain);
                var connectionString = ConfigurationManager.ConnectionStrings[domain].ConnectionString;
                return connectionString;
            }
            catch 
            {
                Console.WriteLine("!!! Enter a Valid Domain!!!" + "\npress:" + "\n1. To Re-try : " +
                                  "\n   Press any key to Return previous menu:");
                var check = Console.ReadLine();
                if (check == "1") goto ERROR1;
                return "";
            }
        }

        public static void HowToRegister()
        {
            Console.WriteLine("Please Visit this site : " +
                              " https://github.com/Djyadav19/EMS---C-sharp-project/blob/main/sql_Query.pdf ");
            Console.WriteLine("Press any key to return: ");
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Login in " + "\n1. Login : " + "\n2. To Read how to register with us: " +
                                  "\n3. Exit : ");
                var input = short.TryParse(Console.ReadLine(), out var choice);
                
                switch (choice)
                {
                    case 1:
                        var connectionString = GettingConnectionString();
                        if (!string.IsNullOrWhiteSpace(connectionString))
                        {
                            var log = new login(connectionString);
                            log.Logging();
                        }

                        break;
                    case 2:
                        HowToRegister();
                        break;
                    case 3:
                        Console.WriteLine("!!!THANK YOU!!!");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("!!! Select from the above Option !!!");
                        Task.Delay(1500).GetAwaiter().GetResult();
                        break;
                }
            }
        }
    }
}