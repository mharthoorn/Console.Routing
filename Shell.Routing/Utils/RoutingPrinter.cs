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
                DidYouMean(result.Candindates);

            }
            else if (result.Status == RoutingStatus.NoMatchingParameters)
            {
                Console.WriteLine("There is no command that matches these parameters");
                DidYouMean(result.Candindates);
            }
        }

        public static void DidYouMean(IList<Route> commandRoutes)
        {
            Console.WriteLine("Did you mean:");
            foreach (var route in commandRoutes) Console.WriteLine($"  {route}");
        }

        public static void PrintRoutes(IEnumerable<OldRoute> routes)
        {

            foreach (var group in routes.GroupBy(r => r.Module))
            {
                Console.WriteLine($"{group.Key.Title}:");

                foreach (var route in group)
                {
                    if (route.Hidden) continue;
                    var name = route.Name;

                    var parameters = route.ParametersDescription().Trim();
                    var description = route.Description?.Trim();

                    var text = parameters;
                    if (!string.IsNullOrEmpty(parameters) && !string.IsNullOrEmpty(description)) text += " | ";
                    text += description;


                    Console.WriteLine($"  {name,-12} {text}");
                }
                Console.WriteLine();
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
            string message = GetErrorMessage(e);
            Console.Write($"Error: {message}");

            if (stacktrace) Console.WriteLine(e.StackTrace);
        }

        public static string GetErrorMessage(Exception exception)
        {
            if (exception.InnerException is null)
            {
                return exception.Message;
            }
            else             
            {
                return GetErrorMessage(exception.InnerException);
            }

        }
    }
}