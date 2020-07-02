using System;
using System.Reflection;

namespace ConsoleRouting
{
    public static class Routing<T>
    {
        public static Assembly Assembly => Assembly.GetAssembly(typeof(T));

        public static Router Router = CreateRouter();
         
        public static void Handle(string[] args)
        {
            var arguments = new Arguments(args);
            Globals.Bind(Assembly, arguments);
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
            return CreateRouter(Assembly);
        }

        public static Router CreateRouter(Assembly assembly)
        {
            var builder = new RouteBuilder();
            builder.DiscoverAssembly(assembly);
            return new Router(builder.Routes);
        }

    }
    
}


