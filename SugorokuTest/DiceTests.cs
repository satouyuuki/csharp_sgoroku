using System;
using Xunit;

namespace Sugoroku.Tests
{
    public class DiceTests
    {
        /// <summary>
        /// サイコロのインスタンス
        /// </summary>
        Dice dice;

        public DiceTests()
        {
            dice = new Dice();
        }

        [Fact]
        public void Number_Test()
        {
            Assert.Equal(6, dice.Number);
        }

        [Fact]
        public void GetDiceNumber_Test()
        {
            dice.GetDiceNumber();
            Assert.InRange(dice.CurrentRoll, 1, 6);
        }
    }
}
