using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EMS
{
    class InputCheck
    {
        public static bool IsBoolean()
        {
            while (true)
            {
                Console.WriteLine("1.Admin" + "\n2.Employee");
                var choice = NumericCheck("choice");
                switch (choice)
                {
                    case 1:
                        return true;
                    case 2:
                        return false;
                    default:
                        Console.Clear();
                        Console.WriteLine("!!!Select from the Given Option !!!!");
                        break;
                }
            }
        }

        public static string ReadPassword()
        {
            while (true)
            {
                var password = "";
                var info = Console.ReadKey(true);
                while (info.Key != ConsoleKey.Enter)
                {
                    if (info.Key != ConsoleKey.Backspace)
                    {
                        Console.Write("*");
                        password += info.KeyChar;
                    }
                    else if (info.Key == ConsoleKey.Backspace)
                    {
                        if (!string.IsNullOrEmpty(password))
                        {
                            // remove one character from the list of _password characters
                            password = password.Substring(0,
                                password.Length - 1);
                            // get the location of the cursor
                            int cursorPosition = Console.CursorLeft;
                            // move the cursor to the left by one character
                            Console.SetCursorPosition(cursorPosition - 1,
                                Console.CursorTop);
                            // replace it with space
                            Console.Write(" ");
                            // move the cursor to the left by one character again
                            Console.SetCursorPosition(cursorPosition - 1,
                                Console.CursorTop);
                        }
                    }

                    info = Console.ReadKey(true);
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Password Can't be NuLL");
                    continue;
                }

                return password;
            }
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var builder = new StringBuilder();
                for (var i = 0;
                     i < bytes.Length;
                     i++)
                {
                    builder.Append(bytes[i]
                        .ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static string StringCheck(string s)
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Write("\n!!!Wrong input Typ!!!e\n Try Again\n Enter " + s + ":        ");
                    continue;
                }

                return input;
            }
        }

        public static double DoubleCheck(string s)
        {
            var check = false;
            var i = 0.0;
            while (!check)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Write("\n!!!Wrong input Type\n Enter a Numeric Number\n Enter " + s + ":      ");
                    continue;
                }

                check = double.TryParse(input,
                    out i);
                if (check && i > 0.0)
                    break;
                check = false;
                Console.Write("\n!!!Wrong input Type\n Enter a Numeric Number\n Enter " + s + ":      ");
            }

            return i;
        }

        public static int NumericCheck(string s)
        {
            var check = false;
            var i = 0;
            while (!check)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Write("\n!!!Wrong input Type\n Enter a Numeric Number:\n Enter " + s + ":      ");
                    continue;
                }

                check = int.TryParse(input,
                    out i);
                if (check && i >= 0)
                    break;
                check = false;
                Console.Write("\n!!!Wrong input Type\n Enter a Numeric Number:\n Enter " + s + ":      ");
            }

            return i;
        }

        public static DateTime DateCheck()
        {
            var check = false;
            DateTime i = default;
            while (!check)
            {
                var r = new Regex(@"^\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])$");
                var input = RegexCheck(r,
                    "Date of Joining");
                check = DateTime.TryParse(input,
                    out i);
                var minDate = DateTime.Now.AddYears(-59);
                if (check && (i <= DateTime.Now && i > minDate))
                {
                    break;
                }

                check = false;
                Console.Write("\n!!!Wrong Input Type!!! \n Enter Date time: ");
            }

            return i;
        }

        public static string RegexCheck(Regex r,
            string s)
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (r.IsMatch(input))
                {
                    return input;
                }

                Console.Write("\n!!!Wrong Input Type!!! \n Enter " + s + " :       ");
            }
        }
    }
}