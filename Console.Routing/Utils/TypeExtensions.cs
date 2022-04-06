using System;
using System.Linq;

namespace ConsoleRouting
{
    internal static class TypeExtensions
    {
        public static bool IsGenericFlag(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Flag<>);
        }

        public static Type GetEnumType(this Type type)
        {
            if (!type.IsGenericFlag())
                return null;

            var flagType = type.GenericTypeArguments.First();

            return flagType.IsEnum ? flagType : null;
        }
    }
}
