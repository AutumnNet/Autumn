using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autumn.Net.Tools;

namespace Autumn.Net.Object
{
    public class MultiplierType
    {
        public Type Type { get; set; }
        public Type ElementType { get; set; }

        public object GetValue(IEnumerable<object> elements)
        {
            if (Type.IsArray)
                return GetArray(elements.ToArray());
            
            if (Type == typeof(IEnumerable<object>))
                return elements;
            
            if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(List<>))
                return GetList(elements);
            
            if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(HashSet<>))
                return GetHashSet(elements);
            
            if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(SortedSet<>))
                return GetSortedSet(elements);

            if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return GetList(elements);

            return null;
        }

        private object GetArray(object[] array)
        {
            var a = Array.CreateInstance(ElementType, array.Length);
            for(int i=0;i<array.Length;i++)
                a.SetValue(array[i], i);
            return a;
        }
        
        private IEnumerable GetList(IEnumerable<object> array)
        {
            var ctor = typeof(List<>).MakeGenericType(ElementType)
                .GetConstructor(new[] {typeof (IEnumerable<>).MakeGenericType(ElementType)});
            return ctor.Invoke(new[] { GetArray(array.ToArray()) }) as IEnumerable;
        }
        
        private IEnumerable GetHashSet(IEnumerable<object> array)
        {
            var ctor = typeof(HashSet<>).MakeGenericType(ElementType)
                .GetConstructor(new[] {typeof (IEnumerable<>).MakeGenericType(ElementType)});
            return ctor.Invoke(new object[] { GetArray(array.ToArray()) }) as IEnumerable;
        }
        
        private IEnumerable GetSortedSet(IEnumerable<object> array)
        {
            var ctor = typeof(SortedSet<>).MakeGenericType(ElementType)
                .GetConstructor(new[] {typeof (IEnumerable<>).MakeGenericType(ElementType)});
            return ctor.Invoke(new[] { GetArray(array.ToArray()) }) as IEnumerable;
        }
        
        public MultiplierType(Type baseType)
        {
            Type = baseType;
            ElementType = AssemblyHelper.GetMultiplierElementType(baseType);
        }
    }
}