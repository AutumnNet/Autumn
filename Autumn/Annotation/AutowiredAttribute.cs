using System;
using System.Xml;

namespace Autumn.Annotation
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Constructor)]
    public class AutowiredAttribute : Attribute
    {
        // public string[] Values { get; set; }
        // public string Value => Values != null && Values.Length > 0 ? Values[0] : "";
    }
}