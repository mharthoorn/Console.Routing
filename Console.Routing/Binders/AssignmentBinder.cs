using System;

namespace ConsoleRouting;


public class AssignmentBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type == typeof(Assignment);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, ref int used, out object result)
    {
        if (arguments.TryGetAssignment(param.Name, out Assignment assignment))
        {
            used++;
            result = assignment;
            return BindStatus.Success;
        }
        else
        {
            result = Assignment.NotProvided;
            return BindStatus.NotFound;
        }
        
    }
}

