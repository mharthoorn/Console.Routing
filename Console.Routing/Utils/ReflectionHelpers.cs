using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting;


public static class ReflectionHelpers
{
    public static PropertyInfo GetProperty(this Type type, Type fieldtype, string name)
    {
        var properties = type.GetProperties();

        foreach (var p in properties)
        {
            if (p.PropertyType == fieldtype && string.Compare(p.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return p;
            }
        }
        return null;
    }

    public static bool IsStatic(this Type type)
    {
        return type.IsAbstract && type.IsSealed;
    }

    public static  MemberInfo TryFindMember(this Type type, string name)
    {
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (string.Compare(field.Name, name, ignoreCase: true) == 0)
            {
                return field;
            }
        }

        var properties = type.GetProperties();
        foreach (var prop in properties)
        {
            if (string.Compare(prop.Name, name, ignoreCase: true) == 0)
            {
                return prop;
            }
        }
        return null;
    }

    public static IEnumerable<MemberInfo> GetFieldsAndProperties(this Type type)
    {
        foreach (var field in type.GetFields()) yield return field;
        foreach (var prop in type.GetProperties()) yield return prop;
    }
    public static Type GetMemberType(this MemberInfo member) => member switch
    {
        PropertyInfo p => p.PropertyType,
        FieldInfo f => f.FieldType,
        _ => throw new Exception("Invalid member: value cannot be set.")
    };

    public static bool MemberTypeIs(this MemberInfo member, Type type)
    {
        return member.GetMemberType() == type;
    }

    public static void SetValue(this MemberInfo member, object obj, object value)
    {
        if (member is PropertyInfo p)
        {
            p.SetValue(obj, value);
        }
        else if (member is FieldInfo f)
        {
            f.SetValue(obj, value);
        }
        else throw new Exception("Invalid member: value cannot be set.");
    }

    public static bool IsGenericType(this Type type, Type parent, Type inner)
    {
        if (type.IsGenericType)
        {
            if (type == parent && type.GetGenericArguments()[0] == inner) return true;
        }
        return false;
    }

}