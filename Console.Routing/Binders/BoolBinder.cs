using System;

namespace ConsoleRouting;


public class BoolBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type == typeof(bool);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result)
    {
        if (arguments.TryGet(param, out Flag flag))
        {
            used++;
            result = true;
            return BindStatus.Success;
        }
        else
        {
            result = false;
            return BindStatus.NotFound;
        }
    }
}

