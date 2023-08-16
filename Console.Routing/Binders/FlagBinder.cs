using System;

namespace ConsoleRouting;


public class FlagBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type == typeof(Flag);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, out object result)
    {
        if (arguments.TryUse(param, out Flag flag))
        {
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

