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
        /// マスのルート
        /// </summary>
        private List<Square> Squares = new List<Square>();

        /// <summary>
        /// サイコロを振った回数
        /// </summary>
        private int Count { get; set; }

        /// <summary>
        /// トータルのゲーム参加人数
        /// </summary>
        private int totalPlayer;
        private int TotalPlayer {
            get { return this.totalPlayer; }
            set {
                this.totalPlayer = value;
                for (int i = 1; i < value; i++)
                {
                    CPUs.Add(new Player("cpu" + i));
                }
            }
        }
        /// <summary>
        /// 今サイコロを持ってるプレイヤー
        /// </summary>
        private Player CurrentPlayer
        {
            get
            {
                int remain = Count % TotalPlayer;
                if(remain == 0)
                {
                    MainPlayer.IsMainPlayer = true;
                    return MainPlayer;
                } else
                {
                    MainPlayer.IsMainPlayer = false;
                    return CPUs[remain - 1];
                }
            }
        }

        /// <summary>
        /// MainPlayer
        /// </summary>
        private Player MainPlayer { get; set; }

        /// <summary>
        /// ComputerPlayer
        /// </summary>
        private List<Player> CPUs = new List<Player>();

        public Game()
        {
            Console.WriteLine("Game Start....");
            Console.ReadKey();
            setMainPlayer();
            SetSquare();
        }

        private void SetSquare()
        {
            int[] squares = Enumerable.Range(1, SquaresLength).ToArray();
            for(int i = 0; i < squares.Length; i++)
            {
                //Squares[i] = new Square(squares[i]);
                Squares.Add(new Square(squares[i]));
                Console.WriteLine(Squares[i].Number);
            }
            Squares[0].AddEffect("double", new EffectA());
            Squares[5].AddEffect("double", new EffectA());
            Squares[7].AddEffect("double", new EffectA());
            Squares[10].AddEffect("double", new EffectA());
            Squares[15].AddEffect("double", new EffectA());
            Squares[17].AddEffect("double", new EffectA());
            Squares[20].AddEffect("double", new EffectA());
            Squares[25].AddEffect("double", new EffectA());
            Console.WriteLine(Squares[4].EffectName);
        }

        private void setMainPlayer()
        {
            Console.WriteLine("名前を入力してください※何も入力しないと「名無しさん」になります。");
            string name = Console.ReadLine();
            MainPlayer = !string.IsNullOrEmpty(name) ? new Player(name) : new Player("名無しさん");
        }

        private void SetTotalPlayer()
        {
            int totalPlayer = 0;

            while(totalPlayer < 2 || totalPlayer > 5)
            {
                Console.WriteLine("何人対戦にしますか？（２〜５人まで）");
                bool isParsed = int.TryParse(Console.ReadLine(), out totalPlayer);
            }
            Console.WriteLine(totalPlayer + "人対戦モード");
            TotalPlayer = totalPlayer;
        }

        public void Start()
        {
            SetTotalPlayer();

            while (CurrentPlayer.Position < SquaresLength)
            {
                int roll = CurrentPlayer.GetDiceNumber();
                Console.WriteLine(roll + "進む");
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

                while (Squares[CurrentPlayer.Position - 1].IsEffect)
                {
                    // プレイヤーのポジションequalマスの目
                    Squares[CurrentPlayer.Position - 1].Execute(roll, CurrentPlayer);
                    // 出目の数がゴールを超えたとき
                    if (CurrentPlayer.Position > SquaresLength)
                    {
                        CurrentPlayer.Position = 2 * SquaresLength - CurrentPlayer.Position;
                    }
                    else if (CurrentPlayer.Position == SquaresLength)
                    {
                        break;
                    }
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
