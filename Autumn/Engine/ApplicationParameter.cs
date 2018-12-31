using System;
using System.Collections.Generic;

namespace Autumn.Engine
{
    public abstract class ApplicationParameter : Dictionary<string, object>
    {
        public object GetOrNull(string key) => ContainsKey(key) ? this[key] : null;
    }
}