using System;
namespace Sugoroku
{
    public class Game
    {
        //ゴールのマス
        public int squaresLength = 31;
        public Player player;

        public Game(Player player)
        {
            this.player = player;
        }

        public void start()
        {
            //出たマスの合計
            int i = player.Position;
            while(i < squaresLength)
            {
                int roll = player.rollTheDice();
                i += roll;
                // 出目の数がゴールを超えたとき
                if(i > squaresLength)
                {
                    i = 2 * squaresLength - i;
                }
                else if(i == squaresLength)
                {
                    break;
                }
                Console.WriteLine(roll + "進む" + "\t" + "残りは" + (squaresLength - i) + "マスです");
            }
            Console.WriteLine("ゴール!!");
        }
    }
}
