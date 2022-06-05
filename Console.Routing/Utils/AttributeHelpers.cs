using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting;


public static class AttributeHelpers
{

    public static IEnumerable<Type> GetAttributeTypes<T>(this Assembly assembly) where T : Attribute
    {
        var types = assembly.GetTypes().Where(t => !t.IsNested);
        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<T>();
            if (attribute != null)
            {
                yield return type;
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

    public static IEnumerable<MethodInfo> GetAttributeMethods<T>(this IEnumerable<Type> type, Func<T, bool> predicate) where T : Attribute
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

    public static bool HasAttribute<T>(this ParameterInfo parameter) where T : Attribute
    {
        return parameter.GetCustomAttribute<T>() != null;
    }

    public static bool HasAttribute<T>(this MethodInfo method) where T : Attribute
    {
        return method.GetCustomAttribute<T>() != null;
    }

    public static bool HasAttribute<T>(this Type type) where T : Attribute
    {
        return type.GetCustomAttribute<T>() != null;
    }

}