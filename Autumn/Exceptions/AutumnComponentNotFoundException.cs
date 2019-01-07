using System;

namespace Autumn.Net.Exceptions
{
    public class AutumnComponentNotFoundException : AutumnException
    {
        public AutumnComponentNotFoundException(Type type) : base($"Component {type.FullName} not found")
        {            
        }
    }
}