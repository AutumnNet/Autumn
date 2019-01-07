using System;

namespace Autumn.Net.Annotation.Base
{
    /// <summary>
    /// Base Anotation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class ComponentAttribute : Attribute
    {
        public bool Singleton { get; set; } = true;
        public bool Lazy { get; set; } = false;
        public string Name { get; set; }
    }
}