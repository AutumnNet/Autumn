using System;
using System.Collections.Generic;
using System.Linq;

namespace Autumn.Net.Annotation
{
    public class ProfileAttribute : Attribute
    {
        public string[] Values { get; }
        private readonly HashSet<string> _Values;
        
        private bool Invert { get; set; }

        public bool IsName(string name)
        {
            return Invert ? !_Values.Contains(name) : _Values.Contains(name);
        }

        public bool IsName(IEnumerable<string> names)
        {
            return names.Any(IsName);
        }

        public ProfileAttribute(params string[] names)
        {
            Values = names;
            if (names.All(item => item.StartsWith("!")))
            {
                Invert = true;
                _Values = new HashSet<string>(
                    names.Select(item => item.Substring(0, 1))
                );
            } else if (names.All(item => !item.StartsWith("!")))
            {
                Invert = false;
                _Values = new HashSet<string>(names);                
            }
            else
                throw new ArgumentException("Each value must start with (!) or all value must start without (!)");
        }
    }
}