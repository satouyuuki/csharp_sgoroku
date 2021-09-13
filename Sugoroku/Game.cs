using System;
namespace Sugoroku
{
    public class Game
    {
        /// <summary>
        /// ゴールのマス
        /// </summary>
        private int SquaresLength = 31;

        /// <summary>
        /// サイコロを振った回数
        /// </summary>
        private int Count { get; set; }

        private Player CurrentPlayer => Count % 2 == 0 ? player : cp1;

        /// <summary>
        /// MainPlayer
        /// </summary>
        private Player player;

        /// <summary>
        /// TODO: 今後５人まで増える
        /// ComputerPlayer
        /// </summary>
        private Player cp1;

        public Game(Player player, Player cp1)
        {
            this.player = player;
            this.cp1 = cp1;
        }

        public void Start()
        {
            while (CurrentPlayer.Position < SquaresLength)
            {
                int roll = CurrentPlayer.GetDiceNumber();
                CurrentPlayer.Position += roll;

                // 出目の数がゴールを超えたとき
                if (CurrentPlayer.Position > SquaresLength)
                {
                    CurrentPlayer.Position = 2 * SquaresLength - CurrentPlayer.Position;
                }
                else if(CurrentPlayer.Position == SquaresLength)
                {
                    break;
                }
                Console.WriteLine(roll + "進む" + "\t" + "残りは" + (SquaresLength - CurrentPlayer.Position) + "マスです");
                Count++;
            }

            Console.WriteLine("ゴール!!" + CurrentPlayer.Name + "の勝ちです");
        }
    }
}
