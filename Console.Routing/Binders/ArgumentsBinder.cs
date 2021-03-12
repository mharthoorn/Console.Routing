using System;

namespace ConsoleRouting
{
    public class ArgumentsBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Arguments);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            value = new Arguments(arguments);
            int claim = arguments.Count - index; // just claim the rest.
            return claim;
        }
    }

}
