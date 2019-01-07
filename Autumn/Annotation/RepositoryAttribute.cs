using System;
using Autumn.Net.Annotation.Base;

namespace Autumn.Net.Annotation
{
    /// <summary>
    /// Repository
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RepositoryAttribute : ComponentAttribute
    {
            
    }
}