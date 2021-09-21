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
        public static int SquaresLength = 31;

        /// <summary>
        /// プレイヤーの最大人数
        /// </summary>
        private int MaxMember = 5;

        /// <summary>
        /// 現在ユーザーのマス目
        /// </summary>
        private Square CurrentSquare
        {
            get
            {
                return Squares[CurrentPlayer.Position - 1];
            }
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

        public Game()
        {
            NewGame();
        }

        private void NewGame()
        {
            Console.WriteLine("Game Start....");
            Console.ReadKey();
            SetSquare();
            SetPlayer();
        }

        private void ReSettingsGame()
        {
            Count = 0;
            Player.ResetPlayerInstances();
            Players = new List<Player>();
            Squares = new List<Square>();
            NewGame();
            Start();
        }

        private void RestartGame()
        {
            Count = 0;
            Players = Players.Select(p =>
            {
                p.Position = 0;
                p.RestLength = ERestLength.none;
                return p;
            }).ToList();
            Start();
        }

        private void SetSquare()
        {
            Square[] squares = Enumerable.Range(1, Game.SquaresLength).Select(x => new Square(x)).ToArray();
            Squares.AddRange(squares);

            Squares[1].AddEffect(EEffectName.doublego.ToAliasString(), new EffectA());
            Squares[5].AddEffect(EEffectName.everyrest.ToAliasString(), new EffectF());
            Squares[3].AddEffect(EEffectName.everyrest.ToAliasString(), new EffectF());
            Squares[11].AddEffect(EEffectName.threeback.ToAliasString(), new EffectB());
            Squares[13].AddEffect(EEffectName.redo.ToAliasString(), new EffectG());
            Squares[15].AddEffect(EEffectName.onerest.ToAliasString(), new EffectD());
            Squares[17].AddEffect(EEffectName.sixgo.ToAliasString(), new EffectC());
            Squares[20].AddEffect(EEffectName.doublego.ToAliasString(), new EffectA());
            Squares[22].AddEffect(EEffectName.redo.ToAliasString(), new EffectG());
            Squares[25].AddEffect(EEffectName.onerest.ToAliasString(), new EffectD());
            Squares[27].AddEffect(EEffectName.redo.ToAliasString(), new EffectG());
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

        private void ConfirmContinue()
        {
            int settings = 0;
            while (settings != 1 && settings != 2)
            {
                Console.WriteLine("同じ設定でもう一度遊ぶ: 1, 設定に戻る: 2");
                bool isParsed = int.TryParse(Console.ReadLine(), out settings);
            }
            switch (settings)
            {
                case 1:
                    RestartGame();
                    break;
                case 2:
                default:
                    ReSettingsGame();
                    break;
            }
        }

        public void Start()
        {
            Console.WriteLine("currentplayer = " + CurrentPlayer.Name);
            while (CurrentPlayer.Position < Game.SquaresLength)
            {
                // ゲームカウントを１上げる
                // ゲームをわかりやすくするための補助
                if (CurrentPlayer.IsMainPlayer) ShopPlayerPosition();
                Console.WriteLine("---------" + CurrentPlayer.Name + "------------");

                // スキップ処理
                if (CurrentPlayer.RestLength > ERestLength.none)
                {
                    Console.WriteLine(CurrentPlayer.Name + "はスキップ");
                    if (CurrentPlayer.RestLength != ERestLength.every) CurrentPlayer.RestLength--;
                    Count++;
                    continue;
                }
                // サイコロをふる
                CurrentPlayer.RollDice();

                if (CurrentPlayer.IsGool) break;


                // 効果マスで、プレイヤーの休みが０の時
                while (CurrentSquare.IsEffect && CurrentPlayer.RestLength == ERestLength.none)
                {
                    if(CurrentSquare.EffectName == EEffectName.everyrest.ToAliasString())
                    {
                        CurrentSquare.Execute(CurrentPlayer, Players);
                    } else
                    {
                        CurrentSquare.Execute(CurrentPlayer);
                    }
                }

                if (CurrentPlayer.IsGool) break;

                Console.WriteLine("残りは" + (Game.SquaresLength - CurrentPlayer.Position) + "マスです");
                Count++;
                // 一秒間隔をあける
                Thread.Sleep(1000);
            }

            Console.WriteLine("ゴール!!" + CurrentPlayer.Name + "の勝ちです");
            ConfirmContinue();
            
        }
    }
}
