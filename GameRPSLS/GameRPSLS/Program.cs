using System;
using System.Security.Cryptography;
using System.Text;

namespace GameRPSLS
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            if ((args.Length >= 3) && (args.Length % 2 == 1)&&(!checkDuplicate(args)))
            {
                getWinner(args);
            }
            else
            {
                Console.WriteLine("Error. Try to use 3 or more parametrs % 2 = 1 and dont use duplicate values");
                return;
            }
        }
        public static bool checkDuplicate(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = i+1; j < args.Length; j++)
                {
                    if (args[i]==args[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static void getWinner(string[] args)
        {
            var key = new byte[16];
            rng.GetBytes(key);
            var rnd = new Random();
            var step = rnd.Next(0, args.Length);
            Console.WriteLine("HMAC: "+HMACHASH(args[step], key));
            Console.WriteLine("Available moves: ");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine((i+1)+" - "+args[i]);
            }
            Console.WriteLine("0 - exit");
            Console.Write("Enter your move: ");
            string choose = Console.ReadLine();
            int plrMove = int.Parse(choose) - 1;
            if (plrMove == step)
            {
                Console.WriteLine("Your move: " + args[plrMove]);
                Console.WriteLine("Computer move: " + args[step]);
                Console.WriteLine("Draw");
                Console.WriteLine("HMAC key: "+BitConverter.ToString(key));
                return;
            }
            if ((plrMove>step)&&(plrMove-(args.Length/2)<=step))
            {
                Console.WriteLine("Your move: " + args[plrMove]);
                Console.WriteLine("Computer move: " + args[step]);
                Console.WriteLine("You Win!");
                Console.WriteLine("HMAC key: " + BitConverter.ToString(key));
                return;
            }
            if ((plrMove<step)&&(((step+(args.Length/2))-(args.Length))>=plrMove))
            {
                Console.WriteLine("Your move: " + args[plrMove]);
                Console.WriteLine("Computer move: " + args[step]);
                Console.WriteLine("You Win!");
                Console.WriteLine("HMAC key: " + BitConverter.ToString(key));
                return;
            }
            Console.WriteLine("Your move: " + args[plrMove]);
            Console.WriteLine("Computer move: " + args[step]);
            Console.WriteLine("You Lose..");
            Console.WriteLine("HMAC key: " + BitConverter.ToString(key));
            //дописать вывод ключа для проверки

        }
        private static RandomNumberGenerator rng = RandomNumberGenerator.Create();

        static string HMACHASH(string str, byte[] key)
        {
            using (var hmac = new HMACSHA1(key))
            {
                byte[] bstr = Encoding.Default.GetBytes(str);
                return Convert.ToBase64String(hmac.ComputeHash(bstr));
            }
        }
    }
}
