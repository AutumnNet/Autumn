using System;
using Autumn.Annotation.Base;

namespace Autumn.Annotation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ConfigurationAttribute : ComponentAttribute
    {
        public int Priority { get;}

        public ConfigurationAttribute(int priority)
        {
            Priority = priority;
        }

        public ConfigurationAttribute() : this(0)
        {
            
        }
    }
}