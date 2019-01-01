using System;

namespace Autumn.Annotation.Base
{
    /// <summary>
    /// Base Anotation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ComponentAttribute : Attribute
    {
        public bool Singleton { get; set; }
        public bool Lazy { get; set; }
    }
}