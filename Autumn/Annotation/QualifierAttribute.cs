using System;

namespace Autumn.Annotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class QualifierAttribute : Attribute, IAutowiredName
    {
        public string Name { get; set; }   
        public string[] Names { get; set; }   
    }
}