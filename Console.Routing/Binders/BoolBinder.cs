using System;

namespace ConsoleRouting;


public class BoolBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type == typeof(bool);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, out object result)
    {
        if (arguments.TryUse(param, out Flag flag))
        {

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

