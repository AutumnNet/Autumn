using System;
using System.Collections.Generic;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Annotation.Base;
using Autumn.Tools;

namespace Autumn.Object
{
    /// <summary>
    /// Configuration Storage 
    /// </summary>
    public class ComponentType
    {
        public Type Type { get; }
        
        public Type BeanTargetType { get; }
        
        public string Name { get; set; }
        
        public MethodInfo BeanMethodInfo { get; }
        
        public ConstructorInfo Constructor { get; }
        public IEnumerable<Type> InheritanceTypes { get; }
        
        public bool IsBean { get; }
        
        public bool IsPrimary { get; }

        public bool Singleton { get; } 
        
        public bool Lazy { get; }

        public override string ToString()
        {
            if (IsBean)
                return $"[ComponentType(BEAN) Target:{Type.FullName}";
            else
                return $"[ComponentType(COMP) Target:{Type.FullName}";
        }

        public ComponentType(Type type)
        {
            Type = type;
            InheritanceTypes = Type.GetInheritanceTypes();
            Constructor = Type.GetAutumnConstructor();
            var attr = type.GetCustomAttribute<ComponentAttribute>();
            Singleton = attr.Singleton;
            IsBean = attr.Lazy;
            Name = attr.Name;
        }

        public ComponentType(MethodInfo beanMethod, Type target)
        {
            Type = beanMethod.ReturnType;
            BeanMethodInfo = beanMethod;
            InheritanceTypes = Type.GetInheritanceTypes();
            BeanTargetType = target;
            var attr = beanMethod.GetCustomAttribute<BeanAttribute>();
            Singleton = attr.Singleton;
            IsPrimary = beanMethod.GetCustomAttribute<PrimaryAttribute>() != null;
            IsBean = true;
            Name = attr.Name;
            
        }
    }
}