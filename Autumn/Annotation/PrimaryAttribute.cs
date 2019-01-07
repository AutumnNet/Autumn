using System;

namespace Autumn.Net.Annotation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.ReturnValue)]
    public class PrimaryAttribute : Attribute
    {
    }
}