using System;
using System.Linq;

namespace ConsoleRouting;


public class ArgumentsBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type == typeof(Arguments);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, out object result)
    {
        
        if (param.TakeAll)
        {
            result = arguments.Clone();
        }
        else
        {
            result = new Arguments(arguments.Unused());
        }
        arguments.UseAll();
        return BindStatus.Success;
    }
}

public class StringArrayBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type == typeof(string[]);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, out object result)
    {
        
        if (param.TakeAll) 
        {
            result = arguments.Select(a => a.Original).ToArray();
        } 
        else
        { 
            result = arguments.Unused().Select(a => a.Original).ToArray();
        }
        arguments.UseAll();
        return BindStatus.Success;
    }
}
