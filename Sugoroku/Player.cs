using System;
using System.Linq;

namespace Sugoroku
{
    public enum ERestLength
    {
        none,
        one,
        two,
        every
    }

    public class Player
    {
        /// <summary>
        /// 現在地
        /// </summary>
        private int _position;
        public int Position {
            get { return _position; }
            set
            {
                if (value > Game.SquaresLength)
                {
                    _position = 2 * Game.SquaresLength - value;
                }
                else
                {
                    _position = value;
                }
            }
        }

        /// <summary>
        /// プレイヤーの名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// プレイヤーが自分かどうか
        /// </summary>
        public bool IsMainPlayer
        {
            get { return Order == 1; }
        }

        /// <summary>
        /// 休みの長さ
        /// </summary>
        public ERestLength RestLength { get; set; }

        /// <summary>
        /// プレイヤーの順番
        /// </summary>
        private int Order { get; set; }

        /// <summary>
        /// 所持しているサイコロ
        /// </summary>
        public Dice[] Dices = { new Dice(), new Dice()};

        /// <summary>
        /// ゴールしたかどうか
        /// </summary>
        public bool IsGool
        {
            get { return Position == Game.SquaresLength; }
        }

        /// <summary>
        /// インスタンス化された数
        /// </summary>
        private static int InstanceNum = 0;

        public Player(string name)
        {
            InstanceNum++;
            Name = name;
            Order = InstanceNum;
        }

        public static void ResetPlayerInstances()
        {
            Player.InstanceNum = 0;
        }

        public bool IsMyTurn(int count)
        {
            int remain = count % Player.InstanceNum;
            return remain == (Order - 1);
        }

        //サイコロをふる
        public void RollDice()
        {
            Console.WriteLine(Name + "のターンです。");
            if (IsMainPlayer)
            {
                Console.WriteLine("エンターキーを押してください。");
                Console.ReadKey();
            }

            int totalRoll = 0;
            foreach (Dice dice in Dices)
            {
                dice.GetDiceNumber();
                totalRoll += dice.CurrentRoll;
                Console.WriteLine(dice.CurrentRoll + "の目が出ました");
            }
            Position += totalRoll;
            Console.WriteLine(totalRoll + "進む");
            
        }
    }
}
