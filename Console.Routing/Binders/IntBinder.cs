using System;

namespace ConsoleRouting;


public class IntBinder : IBinder
{
    public bool Optional => false;

    public bool Match(Type type) => type == typeof(int);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result)
    {
        if (arguments.TryGetInt(index, out int i))
        {
            used++;
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

