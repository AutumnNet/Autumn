using System;
using System.Collections.Generic;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Annotation.Base;
using Autumn.Engine;
using Autumn.Tools;

namespace Autumn.Scanner
{
    
    public class ComponentStorage
    {
        /// <summary>
        /// Components container for Instantiate
        /// </summary>
        public class ComponentContainer : IEqualityComparer<ComponentContainer>
        {
            public Type Type { get; set; }
            public bool Lazy { get; set; }
            public bool Singleton { get; set; }
            public ValueScanner.ValueConstructor ConstructorMethod { get; set; }
            
            private object instance;
            public object Get(ApplicationContext ctx)
            {
                if (Singleton)
                    return instance ?? (instance = ConstructorMethod.Instantiate(ctx));
                return ConstructorMethod.Instantiate(ctx);    
            }
            
            public ComponentContainer(Type type, ConstructorInfo ci)
            {
                var componentAttribute = Type.GetCustomAttribute<ComponentAttribute>();
                Type = type;
                Singleton = componentAttribute.Singleton;
                Lazy = componentAttribute.Lazy;
                ConstructorMethod = new ValueScanner.ValueConstructor(ci);
            }

            public bool Equals(ComponentContainer x, ComponentContainer y)
            {
                return x.GetHashCode() == y.GetHashCode();
            }

            public int GetHashCode(ComponentContainer obj)
            {
                return new HashCodeBuilder()
                    .Add(Type)
                    .GetHashCode();
            }
        }
        
        
        public class BeanContainer : IEqualityComparer<BeanContainer>
        {
            public object Target { get;}
            public bool Lazy { get; set; }
            public bool Singleton { get; set; }

            private object instance;
            
            public ValueScanner.ValueMethod ValueMethod { get; set; }

            public object Get(ApplicationContext ctx)
            {
                if (Singleton)
                    return instance ?? (instance = ValueMethod.Invoke(ctx));
                return ValueMethod.Invoke(ctx);    
            }

            public BeanContainer(object target, MethodInfo mi)
            {
                var attribute = target.GetType().GetCustomAttribute<BeanAttribute>();
                Singleton = attribute.Singleton;
                Target = target;
                ValueMethod = new ValueScanner.ValueMethod(target, mi);
            }

            public bool Equals(BeanContainer x, BeanContainer y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(BeanContainer obj)
            {
                return new HashCodeBuilder()
                    .Add(Target)
                    .Add(ValueMethod)
                    .GetHashCode();
            }
        }


        
        
        public HashSet<object> ComponentSet { get; }
        public Dictionary<Type, HashSet<ComponentContainer>> ComponentMap { get; }
        
        public Dictionary<Type, HashSet<BeanContainer>> BeanMap { get; }

        
        /// <summary>
        /// Add Compinent into Injection Storage
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        public void AddComponent(Type type, ConstructorInfo ci)
        {
            if (!ComponentMap.ContainsKey(type))
                ComponentMap.Add(type, new HashSet<ComponentContainer>());
            ComponentMap[type].Add(new ComponentContainer(type, ci));
            //ComponentSet.Add(type);
        }
        
        public void AddBean(object target, MethodInfo mi)
        {
            var type = target.GetType();
            if (!BeanMap.ContainsKey(type))
                BeanMap.Add(type, new HashSet<BeanContainer>());
            BeanMap[type].Add(new BeanContainer(target, mi));
            //ComponentSet.Add(type);
        }
        
        public ComponentStorage()
        {
            ComponentSet = new HashSet<object>();
            ComponentMap = new Dictionary<Type, HashSet<ComponentContainer>>();
            BeanMap = new Dictionary<Type, HashSet<BeanContainer>>();
        }
    }
}