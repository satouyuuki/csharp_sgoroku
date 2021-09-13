using System;

namespace Sugoroku
{
    class Program
    {
        static void Main(string[] args)
        {
            Player me = new Player("yuuki");
            Game game = new Game(me);
            game.start();
        }
    }
}
