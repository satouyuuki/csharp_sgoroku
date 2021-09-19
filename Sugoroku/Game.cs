using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sugoroku
{
    public class Game
    {
        /// <summary>
        /// ゴールのマス
        /// </summary>
        private int SquaresLength = 31;

        /// <summary>
        /// プレイヤーの最大人数
        /// </summary>
        private int MaxMember = 5;

        /// <summary>
        /// マスのルート
        /// </summary>
        private List<Square> Squares = new List<Square>();

        /// <summary>
        /// サイコロを振った回数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 今サイコロを持ってるプレイヤー
        /// </summary>
        private Player CurrentPlayer
        {
            get
            {
                return Players.Find(p => p.IsMyTurn(Count));
            }
        }
        
        /// <summary>
        /// プレイヤー達
        /// </summary>
        public List<Player> Players = new List<Player>();

        public Game()
        {
            Console.WriteLine("Game Start....");
            Console.ReadKey();
            SetSquare();
            SetPlayer();
        }

        private void SetSquare()
        {
            int[] squares = Enumerable.Range(1, SquaresLength).ToArray();
            for(int i = 0; i < squares.Length; i++)
            {
                Squares.Add(new Square(squares[i]));
            }
            //Squares[0].AddEffect("everyrest", new EffectF());
            //Squares[5].AddEffect("everyrest", new EffectF());
            //Squares[3].AddEffect("everyrest", new EffectF());
            Squares[11].AddEffect("threeback", new EffectB());
            Squares[15].AddEffect("onerest", new EffectD());
            Squares[17].AddEffect("sixgo", new EffectC());
            Squares[20].AddEffect("double", new EffectA());
            Squares[25].AddEffect("onerest", new EffectD());
        }

        private void SetPlayer()
        {
            Console.WriteLine("名前を入力してください※何も入力しないと「名無しさん」になります。");
            string name = Console.ReadLine();

            Player mainPlayer = !string.IsNullOrEmpty(name) ? new Player(name) : new Player("名無しさん");
            Players.Add(mainPlayer);

            int totalPlayer = 0;
            while(totalPlayer <= 1 || totalPlayer > MaxMember)
            {
                Console.WriteLine("何人対戦にしますか？（２〜５人まで）");
                bool isParsed = int.TryParse(Console.ReadLine(), out totalPlayer);
            }
            Console.WriteLine(totalPlayer + "人対戦モード");
            for (int i = 1; i < totalPlayer; i++)
            {
                Players.Add(new Player("cpu" + i));
            }
        }

        private bool IsGool()
        {
            // 出目の数がゴールを超えたとき
            if (CurrentPlayer.Position > SquaresLength)
            {
                CurrentPlayer.Position = 2 * SquaresLength - CurrentPlayer.Position;
            }
            else if (CurrentPlayer.Position == SquaresLength)
            {
                return true;
            }
            return false;
        }

        public void Start()
        {
            while (CurrentPlayer.Position < SquaresLength)
            {
                // 休み状態を見てスキップする
                if(CurrentPlayer.RestLength > ERestLength.none)
                {
                    Console.WriteLine(CurrentPlayer.Name + "はスキップ");
                    // 
                    if(CurrentPlayer.RestLength != ERestLength.every)
                        CurrentPlayer.RestLength--;
                    Count++;
                    continue;
                }

                int roll = CurrentPlayer.GetDiceNumber();
                Console.WriteLine(roll + "進む");
                CurrentPlayer.Position += roll;

                if (IsGool()) break;
                // 効果マスで、プレイヤーの休みが０の時
                while (Squares[CurrentPlayer.Position - 1].IsEffect && CurrentPlayer.RestLength == ERestLength.none)
                {
                    // プレイヤーのポジションequalマスの目
                    Squares[CurrentPlayer.Position - 1].Execute(CurrentPlayer, roll);

                    if (IsGool()) break;
                }

                Console.WriteLine("残りは" + (SquaresLength - CurrentPlayer.Position) + "マスです");
                Count++;
                // 一秒間隔をあける
                Thread.Sleep(1000);
            }

            Console.WriteLine("ゴール!!" + CurrentPlayer.Name + "の勝ちです");
        }
    }
}
