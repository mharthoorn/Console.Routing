using Shell.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{
    public static class RoutingPrinter
    {
        public static void Write(RoutingResult result)
        {
            if (result.Status == RoutingStatus.NoCommands)
            {
                Console.WriteLine("Unknown command.");
            }
            else if (result.Status == RoutingStatus.AmbigousParameters)
            {
                Console.WriteLine("There is more than one command that matches these parameters:");
                DidYouMean(result.CommandRoutes);

            }
            else if (result.Status == RoutingStatus.NoMatchingParameters)
            {
                Console.WriteLine("There is no command that matches these parameters");
                DidYouMean(result.CommandRoutes);
            }
        }

        public static void DidYouMean(List<Route> commandRoutes)
        {
            Console.WriteLine("Did you mean:");
            foreach (var route in commandRoutes) Console.WriteLine($"  {route}");
        }

        public static void PrintRoutes(IEnumerable<Route> routes)
        {

            foreach (var group in routes.GroupBy(r => r.Section))
            {
                Console.WriteLine();
                Console.WriteLine($"{group.Key.Name}:");

                foreach (var route in group)
                {
                    var name = route.Method.Name.ToLower();

                    var parameters = route.ParametersDescription().Trim();
                    var description = route.Command.Description?.Trim();

                    var text = parameters;
                    if (!string.IsNullOrEmpty(parameters) && !string.IsNullOrEmpty(description)) text += " | ";
                    text += description;


                    Console.WriteLine($"  {name,-10} {text}");
                }
            }
        }

        public static void Write(string[] arguments)
        {
            int i = 0;
            foreach (var arg in arguments)
            {
                Console.WriteLine($" {++i}: '{arg}'");
            }
        }

        public static void Write(Exception e, bool stacktrace = false)
        {
            Console.WriteLine(e.Message);
            if (stacktrace) Console.WriteLine("\n" + e.ToString());
            if (e.InnerException != null)
            {
                Write(e, stacktrace);
            }

        }
    }
}