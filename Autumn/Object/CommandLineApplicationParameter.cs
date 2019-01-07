using System.Collections.Generic;
using System.Text.RegularExpressions;
using Autumn.Net.Engine;

namespace Autumn.Net.Object
{
    public class CommandLineApplicationParameter : ApplicationParameter
    {
        public static Regex argumentsPattern = new Regex(@"^\-(?<key>[a-zA-Z0-9\.]*)=(?<value>.*)$");

        public CommandLineApplicationParameter(IEnumerable<string> args)
        {
            foreach (var arg in args)
            {
                var m = argumentsPattern.Match(arg);
                if (!m.Success) continue;
                Add(m.Groups["key"].Value, m.Groups["value"].Value);
            }
        }
    }
}