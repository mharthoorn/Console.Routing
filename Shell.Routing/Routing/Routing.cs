using System;
using System.Reflection;

namespace Shell.Routing
{
    public static class Routing<T>
    {
        public static Router Router = CreateRouter();
         
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

        public static Router CreateRouter()
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            return CreateRouter(assembly);
        }

        public static Router CreateRouter(Assembly assembly)
        {
            var builder = new RouteBuilder();
            builder.DiscoverAssembly(assembly);
            return new Router(builder.Endpoints);
        }

    }
    
}


