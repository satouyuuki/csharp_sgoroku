using System;

namespace Sugoroku
{
    abstract public class Effect
    {
        public abstract Player Execute(Player player, int diceNum);
    }

    public class EffectA: Effect
    {
        public override Player Execute(Player player, int diceNum)
        {
            Console.WriteLine(diceNum + "の目を２倍にします。");
            player.Position += diceNum;
            return player;
        }
    }

    public class EffectB : Effect
    {
        public override Player Execute(Player player, int diceNum)
        {
            Console.WriteLine("３マス戻ります。");
            player.Position -= 3;
            return player;
        }
    }

    public class EffectC : Effect
    {
        public override Player Execute(Player player, int diceNum)
        {
            Console.WriteLine("6マス進ます。");
            player.Position += 6;
            return player;
        }
    }

    public class EffectD : Effect
    {
        public override Player Execute(Player player, int diceNum)
        {
            Console.WriteLine("1回休み");
            player.RestLength = ERestLength.one;
            return player;
        }
    }

    public class EffectE : Effect
    {
        public override Player Execute(Player player, int diceNum)
        {
            Console.WriteLine("2回休み");
            player.RestLength = ERestLength.two;
            return player;
        }
    }

    public class EffectF : Effect
    {
        public override Player Execute(Player player, int diceNum)
        {
            Console.WriteLine("毎回休み");
            player.RestLength = ERestLength.every;
            return player;
        }
    }
}