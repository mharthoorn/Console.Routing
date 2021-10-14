using System;

namespace ConsoleRouting
{
    public class EnumBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type) => type.IsEnum;

        public BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result)
        {
            if (arguments.TryGetEnum(index, param, out var value))
            {
                used++;
                result = value;
                return BindStatus.Success;
            }
            else
            {
                result = null;
                return BindStatus.Failed;
            }
        }
    }

}
