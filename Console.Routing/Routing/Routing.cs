using System;
using System.Reflection;

namespace ConsoleRouting
{
    public static class Routing<T>
    {
        // for backwards compatibility.
        public static void Handle(string[] args) => Routing.Handle<T>(args);
    }


    public static class Routing
    {
        internal static Assembly Assembly { get; set; }  
        public static Router Router { get; private set; }

        public static void Handle(string[] args)
        {
            Assembly = Assembly.GetCallingAssembly();

            Router = new RouteBuilder()
                .Add(Assembly)
                .AddAssemblyOf<HelpModule>()
                .Build();

            Router.Handle(args);
        }

        public static void Handle<T>(string[] args)
        {
            Assembly = typeof(T).Assembly;
            Handle(args);
        }

        public static void WriteRoutes()
        {
            if (Router?.Routes is null)
                throw new Exception("You are not using the default router. Use RoutingPrinter.WriteRoutes() instead.");

            RoutingWriter.WriteRoutes(Router);
        }
        
        [Obsolete("Use Routing.WriteRoutes() instead")]
        public static void PrintHelp()
        {
            WriteRoutes();
        }
            
    }


}


