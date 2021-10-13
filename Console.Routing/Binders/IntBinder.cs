using System;

namespace ConsoleRouting
{
    public class IntBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type) => type == typeof(int);

        public object TryUse(Arguments arguments, Parameter param, int index, ref int used)
        {
            if (arguments.TryGetInt(index, out int i))
            {
                used++;
                return i;
            }
            else
            {
                return null;
            }
        }
    }

}
