using System;
using System.Linq;

namespace Autumn.Annotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class QualifierAttribute : Attribute, IAutowiredName
    {
        public string[] Names { get; }

        public bool IsName(string name)
        {
            return Names != null && Names.Any(item => item == name);
        }

        public QualifierAttribute(params string[] names)
        {
            Names = names;
        }
    }
}