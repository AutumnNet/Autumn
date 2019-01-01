using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autumn.Annotation;
using Autumn.Annotation.Base;

namespace Autumn.Scanner
{
    public class ComponentScanner
    {
        private ComponentStorage storage;
        
        public void Scan(Assembly assembly)
        {
            foreach (var type in GetComponents(assembly))
            {
                ConstructorInfo resultConstructor = null;
                foreach (var constructor in type.GetConstructors())
                {
                    if (constructor.GetCustomAttributes(typeof(AutowiredAttribute), false).Length > 0)
                    {
                        resultConstructor = constructor;
                        break;
                    }
                    if (constructor.GetParameters().Length == 0)
                        resultConstructor = constructor;
                }
                storage.AddComponent(type, resultConstructor);
            }
        } 

        
        
        
        private static IEnumerable<Type> GetComponents(Assembly assembly)
        {
            return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ComponentAttribute), false).Length > 0);
        }
        
        public ComponentScanner(ComponentStorage storage)
        {
            this.storage = storage;
        }

        
    }
    
}