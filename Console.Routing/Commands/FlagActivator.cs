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