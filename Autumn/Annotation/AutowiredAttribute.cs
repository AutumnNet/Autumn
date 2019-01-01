using System;

namespace Autumn.Annotation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Constructor)]
    public class AutowiredAttribute : Attribute
    {
        
    }
}