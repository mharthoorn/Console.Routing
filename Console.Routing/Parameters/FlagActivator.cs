using System;

namespace ConsoleRouting;

internal static class FlagActivator
{
    public static object CreateValueFlag(Type innertype, string name, string text)
    {
        if (innertype == typeof(string))
        {
            return CreateGenericFlagForObject(innertype, name, (object)text);

        }
        else if (innertype == typeof(int))
        {
            if (int.TryParse(text, out int n))
            {
                return CreateGenericFlagForObject(innertype, name, n);
            }
        }
        else if (innertype.IsEnum)
        {
            if (StringHelpers.TryParseEnum(innertype, text, out object enumvalue))
            {
                return CreateGenericFlagForObject(innertype, name, enumvalue);
            }
        }
        return null;
    }

    public static object CreateUnsetValueFlag(Type type, string name)
    {
        var flagtype = MakeGenericFlagType(type);
        return Activator.CreateInstance(flagtype, name, default, false);
    }

    private static Type MakeGenericFlagType(Type type) 
    {
        Type flagType = typeof(Flag<>).MakeGenericType(type);
        return flagType; 
    }

    private static object CreateGenericFlagForObject(Type type, string name, object value) 
    {
        var flagtype = MakeGenericFlagType(type); 
        return Activator.CreateInstance(flagtype, name, value, true);
    }

    
}