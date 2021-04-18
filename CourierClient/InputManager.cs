using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CourierClient
{
	public static class InputManager
	{
		public static SecureString ReadPassword()
		{
            var password = new SecureString();
            Console.WriteLine("Enter password:");

            var nextKey = Console.ReadKey(true);

            while (nextKey.Key != ConsoleKey.Enter)
            {
                if (nextKey.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.RemoveAt(password.Length - 1);
                        Console.Write(nextKey.KeyChar);
                        Console.Write(" ");
                        Console.Write(nextKey.KeyChar);
                    }
                }
                else
                {
                    password.AppendChar(nextKey.KeyChar);
                    Console.Write("*");
                }

                nextKey = Console.ReadKey(true);
            }

            password.MakeReadOnly();

            return password;
        }
	}
}
