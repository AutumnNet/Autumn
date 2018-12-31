using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                var instance = type.GetConstructor(new Type[0]).Invoke(null);
//                storage.Add(type, instance);
//                foreach (var iType in type.GetInterfaces())
//                    AddTypeIntoMap(iType, instance);
//
//                var cType = type.BaseType;
//                while (cType != null)
//                {
//                    AddTypeIntoMap(cType, instance);
//                    foreach (var iType in cType.GetInterfaces())
//                        AddTypeIntoMap(iType, instance);
//                    cType = cType.BaseType;
//                }

            }
        } 
        
        private static IEnumerable<Type> GetComponents(Assembly assembly)
        {
            return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(Component), false).Length > 0);
        }
        
        public ComponentScanner(ComponentStorage storage)
        {
            this.storage = storage;
        }

        
    }
    
}