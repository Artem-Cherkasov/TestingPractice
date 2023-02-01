using System;

namespace Triangle
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Undefined Error 1");
                return;
            }

            bool resA = double.TryParse(args[0], out double a);
            bool resB = double.TryParse(args[1], out double b);
            bool resC = double.TryParse(args[2], out double c);

            if (!resA || !resB || !resC)
            {
                Console.WriteLine("Undefined Error 2");
                return;
            }

            if (a <= 0 || b <= 0 || c <= 0)
            {
                Console.WriteLine("Undefined Error 3");
                return;
            }

            if (a > 1000 || b > 1000 || c > 1000)
            {
                Console.WriteLine("Undefined Error 4");
                return;
            }

            if ((a + b <= c) || (a + c <= b) || (b + c <= a))
            {
                Console.WriteLine("Not a triangle");
                return;
            }

            if (a == b && a == c)
            {
                Console.WriteLine("Equilateral");
                return;
            }

            if (a == b || a == c || b == c)
            {
                Console.WriteLine("Isosceles");
                return;
            }

            Console.WriteLine("Simple");

            return;
        }
    }
}
