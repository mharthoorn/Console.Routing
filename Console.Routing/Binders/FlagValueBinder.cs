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

            if (arguments.TryGet(param, out Flag flag))
            {
                if (arguments.TryGetFollowing(flag, out Text text))
                {
                    var value = FlagActivator.CreateValueFlag(innertype, param.Name, text.Value);
                    if (value is not null)
                    {
                        used += 2;
                        return value;
                    }
                } 

                // we do not increment, because it's not a valid parameter.
                // used++;
                return FlagActivator.CreateUnsetValueFlag(innertype, param.Name);
            }
            else
            {
                return FlagActivator.CreateUnsetValueFlag(innertype, param.Name);
            }
        }

       
    }

}
