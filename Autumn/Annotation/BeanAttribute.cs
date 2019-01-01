using System;

namespace Autumn.Annotation
{
    /// <summary>
    /// Инициализатор
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.ReturnValue)]
    public class BeanAttribute : Attribute
    {
        public bool Singleton { get; set; }
    }
}