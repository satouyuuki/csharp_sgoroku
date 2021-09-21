using System;
namespace Sugoroku
{
    public class Dice
    {
        /// <summary>
        /// サイコロの面の数
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 現在の出目
        /// </summary>
        public int CurrentRoll { get; set; }

        public Dice(int number = 6)
        {
            Number = number;
        }

        public void GetDiceNumber()
        {
            CurrentRoll = new Random().Next(1, Number + 1);
        }
    }
}
