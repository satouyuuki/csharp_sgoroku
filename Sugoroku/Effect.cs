using System;

namespace Sugoroku
{
    abstract public class Effect
    {
        public abstract Player Execute(int num, Player player);
    }

    public class EffectA: Effect
    {
        public override Player Execute(int num, Player player)
        {
            Console.WriteLine(num + "の目を２倍にします。");
            player.Position += num;
            return player;
        }
    }
}