using System;

namespace ConsoleRouting
{

    public class ArgumentsBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Arguments);

        public BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result)
        {
            used = arguments.Count;
            result = new Arguments(arguments);
            return BindStatus.Success;
        }
    }

}
