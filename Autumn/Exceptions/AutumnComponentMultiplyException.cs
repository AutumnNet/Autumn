using System;
using System.Collections.Generic;
using System.Linq;
using Autumn.Net.Object;

namespace Autumn.Net.Exceptions
{
    public class AutumnComponentMultiplyException : AutumnException
    {
        public AutumnComponentMultiplyException(Type type, IEnumerable<ComponentType> types) : base($"Found multiply components of type {type.FullName}: [{types.Select(item => item.Type.FullName).Aggregate((a, b) => $"{a}, {b}")}]")
        {}
    }
}