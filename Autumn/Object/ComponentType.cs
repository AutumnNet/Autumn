using System;
using System.Collections.Generic;
using System.Reflection;
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
        
        public MethodInfo BeanMethodInfo { get; }
        
        public ConstructorInfo Constructor { get; }
        public IEnumerable<Type> InheritanceTypes { get; }
        
        public bool IsBean { get; }

        public bool Singleton { get; } //TODO: Get from Attr
        
        public bool Lazy { get; } //TODO: Get from Attr
        
        public ComponentType(Type type)
        {
            Type = type;
            InheritanceTypes = Type.GetInheritanceTypes();
            Constructor = Type.GetAutumnConstructor();
            Singleton = true;
            IsBean = false;
        }

        public ComponentType(MethodInfo beanMethod, Type target)
        {
            Type = beanMethod.ReturnType;
            BeanMethodInfo = beanMethod;
            InheritanceTypes = Type.GetInheritanceTypes();
            BeanTargetType = target;
            Singleton = true;
            IsBean = true;
        }
    }
}