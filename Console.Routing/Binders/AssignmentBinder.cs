using System;

namespace ConsoleRouting;


public class AssignmentBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type == typeof(Assignment);

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, out object result)
    {
        if (arguments.TryUseAssignment(param.Name, out Assignment assignment))
        {
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

