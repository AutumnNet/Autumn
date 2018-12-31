using System;

namespace Autumn.Annotation.Base
{
    /// <summary>
    /// Base Anotation
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class Component : Attribute
    {
        
    }
}