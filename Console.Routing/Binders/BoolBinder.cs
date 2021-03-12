using System;

namespace ConsoleRouting
{
    public class BoolBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(bool);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            if (arguments.TryGet(param, out Flag flag))
            {
                value = true;
                return 1;
            }
            else
            {
                value = false;
                return 0;
            }
        }
    }

}
