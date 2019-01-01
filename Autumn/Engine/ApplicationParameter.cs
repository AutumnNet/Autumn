using System;
using System.Collections.Generic;

namespace Autumn.Engine
{
    public abstract class ApplicationParameter : Dictionary<string, object>
    {
        public object GetOrNull(string key) => GetOrDefault(key, null);
        public object GetOrDefault(string key, object def) => ContainsKey(key) ? this[key] : def;
    }
}