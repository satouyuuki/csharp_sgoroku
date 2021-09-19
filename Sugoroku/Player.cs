using System;
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
        public int Position { get; set; }

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
        /// インスタンス化された数
        /// </summary>
        private static int InstanceNum = 0;

        public Player(string name)
        {
            InstanceNum++;
            Name = name;
            Order = InstanceNum;
        }

        public bool IsMyTurn(int count)
        {
            int remain = count % InstanceNum;
            return remain == (Order - 1);
        }

        //サイコロをふる
        public int GetDiceNumber()
        {
            Console.WriteLine(Name + "のターンです。");
            if (IsMainPlayer)
            {
                Console.WriteLine("エンターキーを押してください。");
                Console.ReadKey();
            }
            int roll = new Random().Next(1, 7);
            Console.WriteLine(roll + "の目が出ました");
            return roll;
        }
    }
}
