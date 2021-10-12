using System;
using BL;

namespace ConsoleApp
{
    public class LoginConsole
    {
        public static int check = -1;
        public int CheckLogin()
        {
            UserBl bl = new UserBl();
            Console.Write("User Name: ");
            string userName = Console.ReadLine();
            Console.Write("Password: ");
            string password = GetPassword();
            Console.WriteLine();
            int UserId = bl.Login(userName, password);
            if(UserId > 0){
                Console.WriteLine("Login successfully!");
                return UserId;
            }else {
                Console.WriteLine("Login fail!");
                return -1;
            }
        }
        
        static string GetPassword()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            return pass;
        }
    }
}
