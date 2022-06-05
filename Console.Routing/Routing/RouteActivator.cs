using System;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting;

public static class RouteActivator
{
   
    public static object CreateInstance(this IServiceProvider services, Type type, Router router)
    {
        var constructor = type.GetConstructors().FirstOrDefault();
        if (constructor is null)
        {
            return Activator.CreateInstance(type);
        }
        else
        {
            var parameters = constructor.GetParameters();
            var args = new object[parameters.Length];
            for (int i = 0; i <= parameters.Length - 1; i++)
            {
                if (parameters[i].ParameterType == typeof(Router))
                {
                    args[i] = router;
                }
                else 
                {
                    var service = services.GetService(parameters[i].ParameterType);
                    if (service is null)
                        throw new ArgumentException($"Class {type.Name} could not be constructed. The parameter '{parameters[i].Name}' could not be injected");

                    args[i] = service;
                }
            }
            var instance = Activator.CreateInstance(type, args);
            return instance;
        }
    }


}



