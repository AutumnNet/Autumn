using System;

namespace Autumn.Annotation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.ReturnValue)]
    public class PrimaryAttribute : Attribute
    {
    }
}