using System;

namespace Autumn.Net.Annotation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class EnableAssemblyAttribute : Attribute
    {
        public string[] Values { get; }

        public EnableAssemblyAttribute(params string[] values)
        {
            Values = values;
        }
    }
}