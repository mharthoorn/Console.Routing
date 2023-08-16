using System;

namespace ConsoleRouting;


public class FlagValueBinder : IBinder
{
    public bool Optional => true;

    public bool Match(Type type) => type.IsGenericFlag();

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, out object result)
    {
        Type innertype = param.Type.GetGenericArguments()[0];

        if (arguments.TryGet(param, out Flag flag))
        {
            if (arguments.TryGetFollowing(flag, out Text text))
            {
                var value = FlagActivator.CreateValueFlag(innertype, param.Name, text.Value);
                if (value is not null)
                {
                    arguments.Use(flag);
                    arguments.Use(text);
                    result = value;
                    return BindStatus.Success;
                }
            } 

            result = FlagActivator.CreateUnsetValueFlag(innertype, param.Name);
            return BindStatus.Failed;
        }
        else
        {
            result = FlagActivator.CreateUnsetValueFlag(innertype, param.Name);
            return BindStatus.NotFound;
        }
    }

   
}

