using System;

namespace ConsoleRouting
{
    public class BoolBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(bool);

        public object TryUse(Arguments arguments, Parameter param, int index, ref int used)
        {
            if (arguments.TryGet(param, out Flag _))
            {
                used++;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
