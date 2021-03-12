using System;

namespace ConsoleRouting
{
    internal static class FlagActivator
    {
        public static Type MakeType(Type type) 
        {
            Type flagType = typeof(Flag<>).MakeGenericType(type);
            return flagType; 
        }

        public static object CreateInstance(Type innertype, string name, string text)
        {
            if (innertype == typeof(string))
            {
                return CreateInstance(innertype, name, (object)text);

            }
            else if (innertype == typeof(int))
            {
                if (int.TryParse(text, out int n))
                {
                    return CreateInstance(innertype, name, n);
                }
            }
            else if (innertype.IsEnum)
            {
                if (StringHelpers.TryParseEnum(innertype, text, out object enumvalue))
                {
                    return CreateInstance(innertype, name, enumvalue);
                }
            }
            return null;
        }

        public static object CreateInstance(Type type, string name, object value) 
        {
            var flagtype = MakeType(type); 
            return Activator.CreateInstance(flagtype, name, value, true, true);
        }

        public static object CreateNotSetInstance(Type type, string name)
        {
            var flagtype = MakeType(type);
            return Activator.CreateInstance(flagtype, name, default, false, false);
        }
    }


}