using System;
using System.Linq;

namespace Autumn.Annotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class QualifierAttribute : Attribute, IAutowiredName
    {
        public string Name { get; set; }   
        public string[] Names { get; set; }

        public bool IsName(string name)
        {
            if (!string.IsNullOrEmpty(Name) && Name == name) return true;
            return Names != null && Names.Any(item => item == name);
        }
    }
}