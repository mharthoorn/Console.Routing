using System;
using System.Linq;

namespace ConsoleRouting;


public class ArgumentsBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type == typeof(Arguments);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result)
    {
        if (param.TakeAll)
        {
            // used = does not change. It's non consuming
            result = arguments.Clone();
        }
        else
        {
            used = arguments.Count;
            result = new Arguments(arguments.Skip(index));
        }
        return BindStatus.Success;
    }
}

public class StringArrayBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type == typeof(string[]);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result)
    {
        if (param.TakeAll) 
        {
            // used = does not change. It's non consuming
            result = arguments.Select(a => a.Original).ToArray();
        } 
        else
        { 
            used = arguments.Count;
            result = arguments.Skip(index).Select(a => a.Original).ToArray();
        }
        return BindStatus.Success;
    }
}
