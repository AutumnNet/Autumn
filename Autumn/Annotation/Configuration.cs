using System;
using Autumn.Annotation.Base;

namespace Autumn.Annotation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class Configuration : Component
    {
        public int Priority { get;}

        public Configuration(int priority)
        {
            Priority = priority;
        }

        public Configuration() : this(0)
        {
            
        }
    }
}