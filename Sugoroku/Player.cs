using System;
namespace Sugoroku
{
    public class Player
    {
        /// <summary>
        /// 現在地
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// プレイヤーの名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// プレイヤーが自分かどうか
        /// </summary>
        public bool IsMainPlayer { get; set; }

        public Player(string name)
        {
            Name = name;
        }

        //サイコロをふる
        public int GetDiceNumber()
        {
            Console.WriteLine(Name + "のターンです。エンターキーを押してください。");
            if (IsMainPlayer)
            {
                Console.ReadKey();
            }
            int roll = new Random().Next(1, 7);
            Console.WriteLine(roll + "の目が出ました");
            return roll;
        }
    }
}
