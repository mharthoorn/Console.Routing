using System;

namespace ConsoleRouting
{
    public class FlagValueBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Flag<>);
    
        public object TryUse(Arguments arguments, Parameter param, int index, ref int used)
        {
            Type innertype = param.Type.GetGenericArguments()[0];

            if (arguments.TryGetOptionString(param, out string s))
            {
                var value = FlagActivator.CreateInstance(innertype, param.Name, s);
                if (value is not null) used += 2;
                return value;
            } 

            return FlagActivator.CreateNotSetInstance(innertype, param.Name);
        }

       
    }

}
