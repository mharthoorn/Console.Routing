using System;

namespace ConsoleRouting
{
    public class FlagBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Flag);

        public object TryUse(Arguments arguments, Parameter param, int index, ref int used)
        {
            if (arguments.TryGet(param, out Flag flag))
            {
                used++;
                return flag;
            }
            else
            {
                return new Flag(param.Name, set: false);
            }
            
        }

    }

}
