using System;
using System.Collections.Generic;
using Autumn.Object;

namespace Autumn.Exceptions
{
    public class AutumnComponentMultiplyPrimaryException : AutumnComponentMultiplyException
    {
        public AutumnComponentMultiplyPrimaryException(Type type, IEnumerable<ComponentType> types) : base(type, types)
        {
        }
    }
}