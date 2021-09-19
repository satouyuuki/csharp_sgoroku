using System;
using System.Collections.Generic;

namespace Sugoroku
{
    abstract public class Effect
    {
        public abstract void Execute(Player player);
        public virtual void Execute(Player player, List<Player> players) { }
    }

    public class EffectA: Effect
    {
        public override void Execute(Player player)
        {
            Console.WriteLine("出た目×2の位置に移動。");
            player.Position += player.DiceNumber;
        }
    }

    public class EffectB : Effect
    {
        public override void Execute(Player player)
        {
            Console.WriteLine("３マス戻ります。");
            player.Position -= 3;
        }
    }

    public class EffectC : Effect
    {
        public override void Execute(Player player)
        {
            Console.WriteLine("6マス進ます。");
            player.Position += 6;
        }
    }

    public class EffectD : Effect
    {
        public override void Execute(Player player)
        {
            Console.WriteLine("1回休み");
            player.RestLength = ERestLength.one;
        }
    }

    public class EffectE : Effect
    {
        public override void Execute(Player player)
        {
            Console.WriteLine("2回休み");
            player.RestLength = ERestLength.two;
        }
    }

    public class EffectF : Effect
    {
        public override void Execute(Player player) { }
        public override void Execute(Player player, List<Player> players)
        {
            Console.WriteLine("他の人が同じマスに止まるまで休み");

            // 同じマスに他のプレーヤーがいる＆他のプレイヤーがずっと休みのマスにいるとき
            var releasedPlayer = players.Find(p => p.Position == player.Position && p.RestLength == ERestLength.every);
            Console.WriteLine("releasedPlayer is null ", releasedPlayer == null);

            if(releasedPlayer != null)
            {
                releasedPlayer.RestLength = ERestLength.none;
                Console.WriteLine(releasedPlayer.Name + "が解放されました。");
            }

            player.RestLength = ERestLength.every;
        }
    }

    public class EffectG : Effect
    {
        public override void Execute(Player player)
        {
            Console.WriteLine("最初のマスまで戻る");
            player.Position = 1;
        }
    }
}