using System;

namespace ConsoleRouting
{
    public class ArgumentsBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Arguments);

        public object TryUse(Arguments arguments, Parameter param, int index, ref int used)
        {
            used = arguments.Count;
            return new Arguments(arguments);
        }
    }

}
