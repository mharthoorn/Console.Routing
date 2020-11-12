using System;
using System.Reflection;

namespace ConsoleRouting
{
    public static class Invoker
    {
        public static void Run(MethodInfo method, Router router, Arguments arguments)
        {
            var instance = CreateInstance(method.DeclaringType, router);
            method.Invoke(instance, new[] { arguments });
        }

        public static void Run(Router router, MethodInfo method, object[] arguments)
        {
            try
            {
                var instance = CreateInstance(method.DeclaringType, router);
                method.Invoke(instance, arguments);
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException) throw e.InnerException;
            }
        }

        public static object CreateInstance(Type type, Router router)
        {
            if (type.GetConstructor(new Type[] { typeof(Router) }) is object)
            {
                return Activator.CreateInstance(type, router);
            }
            else 
            {
                return Activator.CreateInstance(type);
            }
            
        }

        public static void Run(Router router, Bind bind)
        {
            Run(router, bind.Route.Method, bind.Parameters);
        }

        public static void Run(Router router, RoutingResult result)
        {
            if (result.Ok)
            {
                Run(router, result.Bind);
            }
        }
    }

}


