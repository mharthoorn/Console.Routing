using System;

namespace ConsoleRouting
{
    public class IntBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type) => type == typeof(int);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            if (arguments.TryGetInt(index, out int i))
            {
                value = i;
                return 1;
            }
            else
            {
                value = default;
                return 0;
            }
        }
    }

}
