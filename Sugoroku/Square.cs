using System;
using System.Collections.Generic;

namespace Sugoroku
{
    public class Square
    {
        /// <summary>
        /// マス目の数字
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// マスの効果名
        /// </summary>
        public string EffectName { get; set; }

        /// <summary>
        /// マスの効果
        /// </summary>
        public Dictionary<string, Effect> EffectMap = new Dictionary<string, Effect>();

        public bool IsEffect
        {
            get { return EffectMap.Count != 0; }
        }

        public Square(int number)
        {
            Number = number;
        }

        public void AddEffect(string effectName, Effect effect)
        {
            EffectName = effectName;
            EffectMap.Add(effectName, effect);
        }

        public void Execute(int diceNum, Player player)
        {
            Console.WriteLine(EffectName + "マスの効果発動。マスの目は" + Number);
            if(EffectMap.ContainsKey(EffectName))
            {
                EffectMap[EffectName].Execute(diceNum, player);
            }
        }
    }
}
