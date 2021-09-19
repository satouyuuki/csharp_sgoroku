using System;

namespace Sugoroku
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //int Sum(int ret, int n)
            //{
            //    return (n > 0) ? Sum(ret + n, n - 1) : ret;
            //}
            //Console.WriteLine(Sum(0, 2));
            //Sum(0, 10);
            Game game = new Game();
            game.Start();

        }
    }
}
