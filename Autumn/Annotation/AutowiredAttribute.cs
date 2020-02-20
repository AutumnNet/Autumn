using System;

namespace Autumn.Net.Annotation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Constructor)]
    public class AutowiredAttribute : Attribute
    {
        public bool Required { get; set; } = true;
    }
}