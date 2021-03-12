using System;

namespace ConsoleRouting
{
    public class StringBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type)
        {
            return type == typeof(string);
        }

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            int used = 0;

            if (arguments.TryGetText(index, out Text Text))
            {
                value = Text.Value;
                used++;
            }
            else value = null;

            return used;
        }
    }

}
