using System;

namespace ConsoleRouting
{
    public class AssignmentBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Assignment);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            
            if (arguments.TryGetAssignment(param.Name, out Assignment assignment))
            {
                value = assignment;
                return 1;
            }
            else
            {
                value = Assignment.NotProvided;
                return 0;
            }
            
        }
    }

}
