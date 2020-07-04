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
        private static Router Router;

        public static void Handle(string[] args)
        {
            Assembly = Assembly.GetCallingAssembly();
            Router = new RouteBuilder().Add(Assembly).Build();
            Router.Handle(args);
        }

        public static void Handle<T>(string[] args)
        {
            Assembly = typeof(T).Assembly;
            Handle(args);
        }

        public static void PrintHelp()
        {
            RoutingPrinter.WriteRoutes(Router.Routes);
        }
            
    }


}


