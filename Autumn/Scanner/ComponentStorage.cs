using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autumn.Annotation;

namespace Autumn.Scanner
{
    
    public class ComponentStorage
    {
        public class BeanContainer
        {
            public object Target { get;}
            public MethodInfo Method { get; }

            public BeanContainer(object target, MethodInfo method)
            {
                Method = method;
                Target = target;
            }
        }


        
        
        public HashSet<object> ComponentSet { get; }
        public Dictionary<Type, HashSet<object>> ComponentMap { get; }
        
        public Dictionary<Type, BeanContainer> BeanMap { get; }

        
        /// <summary>
        /// Add Compinent into Injection Storage
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        private void Add(Type type, object obj)
        {
            if (!ComponentMap.ContainsKey(type))
                ComponentMap.Add(type, new HashSet<object>());
            ComponentMap[type].Add(obj);
            ComponentSet.Add(obj);
        }
        
        
        public ComponentStorage()
        {
            ComponentSet = new HashSet<object>();
            ComponentMap = new Dictionary<Type, HashSet<object>>();
            BeanMap = new Dictionary<Type, BeanContainer>();
        }
    }
}