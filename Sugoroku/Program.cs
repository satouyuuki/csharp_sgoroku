using System;

namespace Sugoroku
{
    class Program
    {
        static void Main(string[] args)
        {
            Player me = new Player("yuuki");
            Player cp1 = new Player("cp1");
            Game game = new Game(me, cp1);
            game.start();
        }
    }
}
