using System;
using System.Reflection;

namespace Shell.Routing
{
    public static class Routing<T>
    {
        public static Router Router;

        static Routing()
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            Router = new Router(assembly);
        }

        public static void Handle(string[] args)
        { 
            var arguments = new Arguments(args);
            try
            {
                var result = Router.Handle(arguments);
                if (!result.Ok) RoutingPrinter.Write(result);
            }
            catch (Exception e)
            {
                RoutingPrinter.Write(e, stacktrace: false); //todo: re-enable through parameter later.
            }

        }
    }
    
}


