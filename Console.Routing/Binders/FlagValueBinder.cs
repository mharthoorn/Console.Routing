using System;

namespace ConsoleRouting
{
    public class FlagValueBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Flag<>);
    
        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            Type innertype = param.Type.GetGenericArguments()[0];

            if (arguments.TryGetOptionString(param, out string s))
            {
                value = FlagActivator.CreateInstance(innertype, param.Name, s);
                if (value is not null) return 2;
            } 

            value = FlagActivator.CreateNotSetInstance(innertype, param.Name);
            return 0;

        }

       
    }

}
