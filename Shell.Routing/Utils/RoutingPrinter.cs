using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{
    public class RoutingError
    {
        public string Message;
        public IList<Route> Candidates;
    }

    public static class RoutingPrinter
    {
        public static void Write(RoutingResult result)
        {
            if (result.Status == RoutingStatus.NoMatchingCommands)
            {
                string command = result.Arguments.items.First().ToString();
                Console.WriteLine($"Unknown command: {command}.");
            }
            else
            {
                Console.WriteLine("You did not supply correct arguments.");

                if (result.Status == RoutingStatus.NoMatchingParameters)
                {
                    var candidates = result.Candidates.NonDefault();
                    if (candidates.Count() > 0) DidYouMean(candidates);
                }
                else if (result.Status == RoutingStatus.AmbigousParameters)
                {
                    DidYouMean(result.Routes);
                }
            }
            
        }

        public static void DidYouMean(IEnumerable<Route> routes)
        {
            Console.WriteLine("Did you mean:");
            foreach (var route in routes) Console.WriteLine($"  {route}");
        }

        public static void PrintRoutes(IEnumerable<Route> routes)
        {

            foreach (var group in routes.GroupBy(r => r.Module))
            {
                var title = group.Key.Title; // Module.Title
                Console.WriteLine($"{title}:");

                foreach (var route in group)
                {
                    if (route.Hidden) continue;

                    var parameters = route.ParametersDescription().Trim();
                    var command = string.Join(" ", route.Nodes.Select(n => n.Names.First())).ToLower();
                    var description = route.Description;
                    var text = parameters;
                    if (!string.IsNullOrEmpty(parameters) && !string.IsNullOrEmpty(description)) text += " | ";
                    text += description;

                    if (command.Length > 15)
                    {
                        Console.WriteLine($"  {command,-15}");
                        Console.WriteLine($"  {"",-12}{text}");
                        
                    }
                    else
                    {
                        Console.WriteLine($"  {command,-15} {text}");
                    }
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