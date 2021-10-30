using System;

namespace ConsoleRouting
{
    public static class StringHelpers
    {
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static bool TryParseEnum(Type type, string value, out object result)
        {
            try
            {
                result = Enum.Parse(type, value, ignoreCase: true);
                return true;

            }
            catch
            {
                result = null;
                return false;
            }

        }
    }

    

}