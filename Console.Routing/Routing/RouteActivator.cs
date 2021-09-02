using System;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public static class RouteActivator
    {
        //public static void Run(MethodInfo method, Router router, Arguments arguments)
        //{
        //    var instance = CreateInstance(method.DeclaringType, router);
        //    method.Invoke(instance, new[] { arguments } );
        //}

        //public static void Run(Router router, RoutingResult result)
        //{
        //    if (result.Ok)
        //    {
        //        Run(router, result.Bind);
        //    }
        //}

        public static void Invoke(this IServiceProvider services, Bind bind)
        {
            try
            {
                var method = bind.Route.Method;
                var instance = services.CreateInstance(method.DeclaringType);
                method.Invoke(instance, bind.Parameters);
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException) throw e.InnerException;
                else throw e;
            }
        }

        //private static void Invoke(IServiceProvider services, MethodInfo method, object[] arguments)
        //{
        //    try
        //    {
        //        var instance = CreateInstance(method.DeclaringType, router);
        //        method.Invoke(instance, arguments);
        //    }
        //    catch (Exception e)
        //    {
        //        if (e is TargetInvocationException) throw e.InnerException;
        //    }
        //}

        //private static object CreateInstance(Type type, Router router)
        //{
        //    if (type.GetConstructor(new Type[] { typeof(Router) }) is object)
        //    {
        //        return Activator.CreateInstance(type, router);
        //    }
        //    else
        //    {
        //        return Activator.CreateInstance(type);
        //    }

        //}

        public static object CreateInstance(this IServiceProvider services, Type type)
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
                for(int i = 0; i <= parameters.Length-1; i++)
                {
                    var service = services.GetService(parameters[i].ParameterType);
                    if (service is null) 
                        throw new ArgumentException($"Class {type.Name} could not be constructed. The parameter '{parameters[i].Name}' could not be injected");

                    args[i] = service;
                }
                var instance = Activator.CreateInstance(type, args);
                return instance;
            }
        }


    }

}


