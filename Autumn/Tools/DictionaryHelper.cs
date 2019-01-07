using System.Collections.Generic;
using System.Reflection;

namespace Autumn.Net.Tools
{
    public class DictionaryHelper
    {
        public static Dictionary<T1, T2> UnionDictionaries<T1, T2>(Dictionary<T1, T2> D1, Dictionary<T1, T2> D2)
        {
            var rd = new Dictionary<T1, T2>(D1);
            foreach (var key in D2.Keys)
            {
                if (!rd.ContainsKey(key))
                    rd.Add(key, D2[key]);
                else if(rd[key].GetType().IsGenericType)
                {
                    if (rd[key].GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        var mBase = MethodBase.GetCurrentMethod();
                        MethodInfo info = mBase is MethodInfo ? (MethodInfo)mBase : typeof(DictionaryHelper).GetMethod("UnionDictionaries", BindingFlags.Public | BindingFlags.Static);
                        var genericMethod = info.MakeGenericMethod(rd[key].GetType().GetGenericArguments()[0], rd[key].GetType().GetGenericArguments()[1]);
                        var invocationResult = genericMethod.Invoke(null, new object[] { rd[key], D2[key] });
                        rd[key] = (T2)invocationResult;
                    }
                }
            }
            return rd;
        }

        /// <summary>
        /// Get Value in RecursiveDictionary
        /// </summary>
        /// <param name="D1"></param>
        /// <param name="path"></param>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public static T2 Path<T1, T2>(Dictionary<T1, T2> D1, IEnumerable<T1> path)
        {
            var res = default(T2);
            foreach(var key in path)
            {
                res = D1[key];
                if (res.GetType() != typeof(Dictionary<T1, T2>))
                    return default(T2);
            }

            return res;
        }


        public static Dictionary<string, object> FlatString(Dictionary<string, object> D1)
        {
            var result = new Dictionary<string, object>();
            foreach (var key in D1.Keys)
            {
                if (D1[key].GetType() == typeof(Dictionary<string, object>))
                {
                    Dictionary<string, object> subDictionary = FlatString((Dictionary<string, object>)D1[key]); 
                    foreach (var subKey in subDictionary.Keys)
                    {
                        result.Add($"{key}.{subKey}", subDictionary[subKey]);
                    }
                }
                else
                    result.Add(key, D1[key]);
            }
            return result;
        }
    }
}