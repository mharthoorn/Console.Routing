using System;
using System.Reflection;

namespace ConsoleRouting
{
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
    }

}