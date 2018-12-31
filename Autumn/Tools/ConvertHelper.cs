using System;
using System.CodeDom;

namespace Autumn.Tools
{
    public class ConvertHelper
    {
        public static object To(Type type, object value)
        {
            if (type == typeof(int)) return ToInt(value);
            if (type == typeof(string)) return ToString(value);
            if (type == typeof(long)) return ToLong(value);
            if (type == typeof(byte)) return ToByte(value);
            if (type == typeof(double)) return ToDouble(value);
            return value;
        }
        
        public static int ToInt(object value)
        {
            switch (value)
            {
                default:
                    return default(int);
                case int i:
                    return i;
                case long l:
                    return Convert.ToInt32(l);
                case string s:
                    
                    return string.IsNullOrEmpty(s) ? 0 : int.Parse(s);
                case double d:
                    return Convert.ToInt32(d);
                case float f:
                    return Convert.ToInt32(f);
                case byte b:
                    return Convert.ToInt32(b);
                case bool t:
                    return t ? 1 : 0;
            }
        }
        
        public static long ToLong(object value)
        {
            switch (value)
            {
                default:
                    return default(long);
                case int i:
                    return Convert.ToInt64(i);
                case long l:
                    return l;
                case string s:
                    return long.Parse(s);
                case double d:
                    return Convert.ToInt64(d);
                case float f:
                    return Convert.ToInt64(f);
                case byte b:
                    return Convert.ToInt64(b);
                case bool t:
                    return t ? 1 : 0;
            }
        }
        
        public static double ToDouble(object value)
        {
            switch (value)
            {
                default:
                    return default(long);
                case int i:
                    return Convert.ToDouble(i);
                case long l:
                    return Convert.ToDouble(l);
                case string s:
                    return double.Parse(s);
                case double d:
                    return d;
                case float f:
                    return Convert.ToDouble(f);
                case byte b:
                    return Convert.ToDouble(b);
                case bool t:
                    return t ? 1 : 0;
            }
        }

        public static byte ToByte(object value)
        {
            switch (value)
            {
                default:
                    return default(byte);
                case int i:
                    return Convert.ToByte(i);
                case long l:
                    return Convert.ToByte(l);
                case string s:
                    return byte.Parse(s);
                case double d:
                    return Convert.ToByte(d);
                case float f:
                    return Convert.ToByte(f);
                case byte b:
                    return Convert.ToByte(b);
                case bool t:
                    return t ? (byte)1 : (byte)0;
            }
        }
        
        
        public static string ToString(object value)
        {
            return value.ToString();
        }


    }
}