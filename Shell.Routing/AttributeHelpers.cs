using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harthoorn.Shell.Routing 
{
    public static class AttributeHelpers
    {

        public static IEnumerable<(Type type, T attribute)> GetAttributeTypes<T>(this Assembly assembly) where T : Attribute
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<T>();
                if (attribute != null)
                {
                    yield return (type, attribute);
                }
            }
        }

        public static IEnumerable<(MethodInfo method, T attribute)> GetAttributeAndMethods<T>(this Type type) where T : Attribute
        {
            foreach (var method in type.GetMethods())
            {
                var attribute = method.GetCustomAttribute<T>();
                if (attribute != null)
                {
                    yield return (method, attribute);
                }
            }
        }

        public static IEnumerable<MethodInfo> GetAttributeMethods<T>(this Type type, Func<T, bool> predicate) where T : Attribute
        {
            return type.GetMethods().Where(m => m.GetCustomAttributes<T>().Any(predicate));
        }

        public static IEnumerable<MethodInfo> GetAttributeMethods<T>(this IEnumerable<Type> type, Func<T, bool> predicate) where T: Attribute
        {
            return type.SelectMany(t => t.GetAttributeMethods<T>(predicate));
        }

        public static IEnumerable<MethodInfo> GetAttributeMethods<T>(this Type type) where T : Attribute
        {
            return type.GetMethods().Where(m => m.GetCustomAttributes<T>().Any());
        }

        public static IEnumerable<MethodInfo> GetAttributeMethods<T>(this IEnumerable<Type> type) where T : Attribute
        {
            return type.SelectMany(t => t.GetAttributeMethods<T>());
        }
    }

}