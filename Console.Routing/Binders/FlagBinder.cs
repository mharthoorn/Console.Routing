using System;

namespace ConsoleRouting
{
    public class FlagBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Flag);

        public BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result)
        {
            if (arguments.TryGet(param, out Flag flag))
            {
                used++;
                result = flag;
                return BindStatus.Success;
            }
            else
            {
                result = new Flag(param.Name, set: false);
                return BindStatus.NotFound;
            }
            
        }

    }

}
