using System;

namespace ConsoleRouting
{
    public class EnumBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type) => type.IsEnum;

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            if (arguments.TryGetEnum(index, param, out value))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

}
