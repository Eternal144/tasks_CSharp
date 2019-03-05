using System;

namespace hello
{
    public class HelloWorldApp
    {
        public void Greet()
        {
            Console.WriteLine("Hello World!");
        }
        public static void Greet_static()
        {
            Console.WriteLine("Hello World!");
        }
    }
    class Execute
    {
        static void Main(string[] args)
        {
            HelloWorldApp.Greet_static();
            HelloWorldApp obj = new HelloWorldApp();
            obj.Greet();
            if (args.Length >= 2)
            {
                Console.WriteLine($"your inputs: {args[0]} {args[1]}");
            }
            else
            {
                Console.WriteLine("your input is not enough");
            }
            Console.ReadKey();
        }
    }
}
