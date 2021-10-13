using System;

namespace ConsoleRouting
{
    public class EnumBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type) => type.IsEnum;

        public object TryUse(Arguments arguments, Parameter param, int index, ref int used)
        {
            if (arguments.TryGetEnum(index, param, out var value))
            {
                used++;
                return value;
            }
            else
            {
                return null;
            }
        }
    }

}
