using System;
using Autumn.Net.Annotation.Base;

namespace Autumn.Net.Annotation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ServiceAttribute : ComponentAttribute
    {
        
    }
}