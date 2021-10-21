using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CommandExecutor.Structures
{
    internal static class Utils
    {
        internal static object CastArray(IEnumerable<object> obj, Type t)
        {
            MethodInfo cast = typeof(Enumerable).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(t);
            MethodInfo toarray = typeof(Enumerable).GetMethod("ToArray", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(t);
            
            return toarray.Invoke(null, new object[] { cast.Invoke(null, new object[] { obj })});
        }
    }
}