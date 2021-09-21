using System;
using System.Reflection;
namespace Sugoroku
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class AliasAttribute : Attribute
    {
        private string _text;
        public string Text => _text;
        public AliasAttribute(string text) { _text = text; }
    }

    public enum EEffectName
    {
        [Alias("１回休み")]
        onerest,
        [Alias("２回休み")]
        tworest,
        [Alias("牢屋")]
        everyrest,
        [Alias("最初から")]
        redo,
        [Alias("２倍")]
        doublego,
        [Alias("６進む")]
        sixgo,
        [Alias("３戻る")]
        threeback,
    }
    public static class EnumExtensions
    {
        public static string ToAliasString(this Enum target)
        {
            var attribute = target.GetType().GetMember(target.ToString())[0].GetCustomAttribute(typeof(AliasAttribute));
            return attribute == null ? null : ((AliasAttribute)attribute).Text;
        }
    }
}
