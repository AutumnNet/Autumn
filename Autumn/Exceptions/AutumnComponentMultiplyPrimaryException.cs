using System;
using System.Collections.Generic;
using Autumn.Net.Object;

namespace Autumn.Net.Exceptions
{
    public class AutumnComponentMultiplyPrimaryException : AutumnComponentMultiplyException
    {
        public AutumnComponentMultiplyPrimaryException(Type type, IEnumerable<ComponentType> types) : base(type, types)
        {
        }
    }
}