using System;

namespace Autumn.Exceptions
{
    public class AutumnComponentNotFoundException : AutumnException
    {
        public AutumnComponentNotFoundException(Type type) : base($"Component {type.FullName} not found")
        {            
        }
    }
}