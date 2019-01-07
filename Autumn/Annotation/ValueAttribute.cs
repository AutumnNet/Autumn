using System;
using System.Text.RegularExpressions;

namespace Autumn.Net.Annotation
{
    /// <summary>
    /// Value Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ValueAttribute :  Attribute
    {
        public string Target { get; }
        public string Default { get; }
        
        private static readonly Regex Pattern = new Regex(@"^\{(?<key>[a-zA-Z0-9\.]*)(|:(?<value>.*))}$", RegexOptions.Compiled);
        
        public ValueAttribute(string target)
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