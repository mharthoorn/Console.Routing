using System;

namespace ConsoleRouting
{
    public class FlagBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Flag);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            if (arguments.TryGet(param, out Flag flag))
            {
                value = flag;
                return 1;
            }
            else
            {
                value = new Flag(param.Name, set: false);
                return 0;
            }
            
        }

    }

}
