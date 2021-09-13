using System;
namespace Sugoroku
{
    public class Player
    {
        
        public Player(string name)
        {
            Name = name;
        }
        // 現在地
        private int _position;
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }
        // プレイヤーの名前
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private bool IsMainPlayer => Name != "cp1";

        //サイコロをふる
        public int rollTheDice()
        {
            Console.WriteLine(Name + "のターンです");
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
