using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sugoroku
{
    public enum EEffectName
    {
        onerest,
        tworest,
        everyrest,
        redo,
        doublego,
        sixgo,
        threeback,
    }
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
        /// 現在ユーザーのマス目
        /// </summary>
        private Square CurrentSquare
        {
            get { return Squares[CurrentPlayer.Position - 1]; }
        }

        /// <summary>
        /// マスのルート
        /// </summary>
        private List<Square> Squares = new List<Square>();

        /// <summary>
        /// サイコロを振った回数
        /// </summary>
        private int Count { get; set; }

        /// <summary>
        /// 今サイコロを持ってるプレイヤー
        /// </summary>
        private Player CurrentPlayer
        {
            get { return Players.Find(p => p.IsMyTurn(Count)); }
        }

        /// <summary>
        /// プレイヤー達
        /// </summary>
        private List<Player> Players = new List<Player>();

        /// <summary>
        /// プレイヤーが休みのマスにいるかどうか
        /// </summary>
        private bool IsSkipState
        {
            get
            {
                var isSkip = CurrentPlayer.RestLength > ERestLength.none;
                if (isSkip) Console.WriteLine(CurrentPlayer.Name + "はスキップ");
                return isSkip;
            }
        }

        /// <summary>
        /// プレイヤーがずっと休みのマスにいるかどうか
        /// </summary>
        private bool IsEverySkipState
        {
            get { return CurrentPlayer.RestLength == ERestLength.every; }
        }

        public Game()
        {
            Console.WriteLine("Game Start....");
            Console.ReadKey();
            SetSquare();
            SetPlayer();
        }

        private void SetSquare()
        {
            Square[] squares = Enumerable.Range(1, SquaresLength).Select(x => new Square(x)).ToArray();
            Squares.AddRange(squares);

            Squares[1].AddEffect(EEffectName.doublego.ToString(), new EffectA());
            Squares[5].AddEffect(EEffectName.everyrest.ToString(), new EffectF());
            Squares[3].AddEffect(EEffectName.everyrest.ToString(), new EffectF());
            Squares[11].AddEffect(EEffectName.threeback.ToString(), new EffectB());
            Squares[13].AddEffect(EEffectName.redo.ToString(), new EffectG());
            Squares[15].AddEffect(EEffectName.onerest.ToString(), new EffectD());
            Squares[17].AddEffect(EEffectName.sixgo.ToString(), new EffectC());
            Squares[20].AddEffect(EEffectName.doublego.ToString(), new EffectA());
            Squares[22].AddEffect(EEffectName.redo.ToString(), new EffectG());
            Squares[25].AddEffect(EEffectName.onerest.ToString(), new EffectD());
            Squares[27].AddEffect(EEffectName.redo.ToString(), new EffectG());
        }

        private void SetPlayer()
        {
            Console.WriteLine("名前を入力してください※何も入力しないと「名無しさん」になります。");
            string name = Console.ReadLine();

            Player mainPlayer = !string.IsNullOrEmpty(name) ? new Player(name) : new Player("名無しさん");
            Players.Add(mainPlayer);

            int totalPlayer = 0;
            while (totalPlayer <= 1 || totalPlayer > MaxMember)
            {
                Console.WriteLine("何人対戦にしますか？（２〜５人まで）");
                bool isParsed = int.TryParse(Console.ReadLine(), out totalPlayer);
            }
            Console.WriteLine(totalPlayer + "人対戦モード");

            Player[] cpus = Enumerable.Range(1, totalPlayer - 1).Select(i => new Player("cpu" + i)).ToArray();
            Players.AddRange(cpus);
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

        private void ShopPlayerPosition()
        {
            foreach(Square square in Squares)
            {
                string showSquare = square.IsEffect ? square.EffectName : square.Number.ToString();
                string[] shopPlayers = Players
                    .FindAll(x => x.Position == square.Number)
                    .Select(x => x.Name)
                    .ToArray();
                string shopPlayer = shopPlayers != null ? String.Join(",", shopPlayers) : "";
                Console.Write("{0}:{1} \t", showSquare, shopPlayer);
            }
            Console.WriteLine();
        }

        public void Start()
        {
            while (CurrentPlayer.Position < SquaresLength)
            {
                if (IsSkipState)
                {
                    if(!IsEverySkipState) CurrentPlayer.RestLength--;
                    Count++;
                    continue;
                }

                if(CurrentPlayer.IsMainPlayer) ShopPlayerPosition();
                CurrentPlayer.RollDice();

                if (IsGool()) break;


                // 効果マスで、プレイヤーの休みが０の時
                while (CurrentSquare.IsEffect && CurrentPlayer.RestLength == ERestLength.none)
                {
                    // プレイヤーのポジションequalマスの目
                    if(CurrentSquare.EffectName == EEffectName.everyrest.ToString())
                    {
                        CurrentSquare.Execute(CurrentPlayer, Players);
                    } else
                    {
                        CurrentSquare.Execute(CurrentPlayer);
                    }

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
