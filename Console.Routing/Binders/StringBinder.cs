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

        public object TryUse(Arguments arguments, Parameter param, int index, ref int used)
        {

            if (arguments.TryGetText(index, out Text Text))
            {
                used++;
                return Text.Value;
                
            }
            return null;
        }
    }

}
