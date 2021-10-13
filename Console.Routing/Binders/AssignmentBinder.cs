using System;

namespace ConsoleRouting
{
    public class AssignmentBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Assignment);

        public object TryUse(Arguments arguments, Parameter param, int index, ref int used)
        {
            if (arguments.TryGetAssignment(param.Name, out Assignment assignment))
            {
                used++;
                return assignment;
            }
            else
            {
                return Assignment.NotProvided; 
            }
            
        }
    }

}
