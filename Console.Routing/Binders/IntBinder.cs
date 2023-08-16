using System;

namespace ConsoleRouting;


public class IntBinder : IBinder
{
    public bool Optional => false;

    public bool Match(Type type) => type == typeof(int);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, out object result)
    {
        if (arguments.TryUseInt(index, out int i))
        {
            result = i;
            return BindStatus.Success;
        }
        else
        {
            result = null;
            return BindStatus.NotFound;
        }
    }
}

