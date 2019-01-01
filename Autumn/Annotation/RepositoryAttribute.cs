using System;
using Autumn.Annotation.Base;

namespace Autumn.Annotation
{
    /// <summary>
    /// Repository
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RepositoryAttribute : ComponentAttribute
    {
            
    }
}