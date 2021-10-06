using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcomeo485();
            Welcome5223();
            Console.ReadKey();
        }

        static partial void Welcome5223();
        private static void Welcomeo485()
        {
            Console.WriteLine("Enter your name: ");
            string Name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", Name);
        }
    }
}
