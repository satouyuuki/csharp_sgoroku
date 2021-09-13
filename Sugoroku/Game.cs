using System;
namespace Sugoroku
{
    public class Game
    {
        //ゴールのマス
        public int squaresLength = 31;
        // サイコロを振った回数
        public int Count { get; set; }

        private Player CurrentPlayer => Count % 2 == 0 ? player : cp1;

        public Player player;
        public Player cp1;

        public Game(Player player, Player cp1)
        {
            this.player = player;
            this.cp1 = cp1;
        }

        public void start()
        {
            while (CurrentPlayer.Position < squaresLength)
            {
                int roll = CurrentPlayer.rollTheDice();
                CurrentPlayer.Position += roll;

                // 出目の数がゴールを超えたとき
                if (CurrentPlayer.Position > squaresLength)
                {
                    CurrentPlayer.Position = 2 * squaresLength - CurrentPlayer.Position;
                }
                else if(CurrentPlayer.Position == squaresLength)
                {
                    break;
                }
                Console.WriteLine(roll + "進む" + "\t" + "残りは" + (squaresLength - CurrentPlayer.Position) + "マスです");
                Count++;
            }

            Console.WriteLine("ゴール!!" + CurrentPlayer.Name + "の勝ちです");
        }
    }
}
