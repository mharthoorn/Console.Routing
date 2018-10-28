using System;
using System.Reflection;

namespace Shell.Routing
{
    public static class Invoker
    {
        public static void Run(MethodInfo method, Arguments arguments)
        {
            var instance = Activator.CreateInstance(method.DeclaringType);
            method.Invoke(instance, new[] { arguments });
        }

        public static void Run(MethodInfo method, object[] arguments)
        {
            try
            {
                var instance = Activator.CreateInstance(method.DeclaringType);
                method.Invoke(instance, arguments);
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException) throw e.InnerException;
            }
        }

        public static void Run(Bind bind)
        {
            Run(bind.Route.Method, bind.Arguments);
        }

        public static void Run(RoutingResult result)
        {
            if (result.Ok)
            {
                Run(result.Bind);
            }
        }
    }

}


