using System;
using System.Text.RegularExpressions;

namespace Autumn.Annotation
{
    /// <summary>
    /// Value Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class Value :  Attribute
    {
        public string Target { get; }
        public string Default { get; }
        
        private static readonly Regex Pattern = new Regex(@"^\{(?<key>[a-zA-Z0-9\.]*)(|:(?<value>.*))}$", RegexOptions.Compiled);
        
        public Value(string target)
        {
            var match = Pattern.Match(target);
            if (!match.Success)
            {
                Default = target;
                Target = null;
            }
            else
            {
                Target = match.Groups["key"].Value;
                Default = match.Groups["value"].Value;
            }
        }
    }
}