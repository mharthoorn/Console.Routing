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

        static Routing()
        {
            Assembly = Assembly.GetEntryAssembly();

            Router = new RouterBuilder()
                .AddAssembly(Assembly)
                .AddAssemblyOf<HelpModule>()
                .AddXmlDocumentation()
                .AddExceptionHandler(DefaultExceptionHandler.Handle)
                .Build();
        }

        public static void Handle(string[] args)
        {
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

            Router.Writer.WriteRoutes(Router);
        }

        public static void Interactive()
        {
            // We have to call this directly, otherwise we get the wrong assembly/
            Assembly = Assembly.GetCallingAssembly();

            Router = new RouterBuilder()
                .AddAssembly(Assembly)
                .AddAssemblyOf<HelpModule>()
                .AddXmlDocumentation()
                .Build();

            while (true)
            {
                Console.Write("> ");
                var query = Console.ReadLine();
                var arguments = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Router.Handle(arguments);
            }
        }
            
    }


}


