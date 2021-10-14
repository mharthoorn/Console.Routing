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

        public BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result)
        {

            if (arguments.TryGetText(index, out Text Text))
            {
                used++;
                result = Text.Value;
                return BindStatus.Success;
                
            }
            else
            {
                result = null;
                return BindStatus.NotFound;
            }
        }
    }

}
