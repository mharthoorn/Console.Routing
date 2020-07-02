using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
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
            switch(result.Status)
            {
                case RoutingStatus.Ok:
                    System.Console.WriteLine("The command was found and understood.");
                    break;

                case RoutingStatus.UnknownCommand:
                    var arg = result.Arguments.FirstOrDefault();
                    if (arg is Text)
                    {
                        System.Console.WriteLine($"Unknown command: {arg}.");
                    }
                    else
                    {
                        System.Console.WriteLine($"Invalid parameter(s). These are your options:");
                        PrintCandidates(result.Candidates.Routes(CommandMatch.Default));
                    }
                    
                    break;

                case RoutingStatus.PartialCommand:
                    System.Console.WriteLine("Did you mean:");
                    PrintCandidates(result.Candidates.Routes(CommandMatch.Partial));
                    break;

                case RoutingStatus.InvalidParameters:
                    System.Console.WriteLine("Invalid parameter(s). These are your options:");
                    PrintCandidates(result.Candidates.Routes(CommandMatch.Full));
                    break;

                case RoutingStatus.AmbigousParameters:
                    System.Console.WriteLine("Ambigous parameter(s). These are your options:");
                    PrintCandidates(result.Routes);
                    break;

                case RoutingStatus.InvalidDefault:
                    System.Console.WriteLine("Invalid parameter(s). These are your options:");
                    PrintCandidates(result.Candidates.Routes(CommandMatch.Default));
                    break;
            }
           
        }

        public static void PrintCandidates(IEnumerable<Route> routes)
        {
            
            foreach (var route in routes) System.Console.WriteLine($"  {route}");
        }

        public static void PrintRoutes(IEnumerable<Route> routes)
        {

            foreach (var group in routes.GroupBy(r => r.Module))
            {
                var title = group.Key.Title; // Module.Title
                System.Console.WriteLine($"{title}:");

                foreach (var route in group)
                {
                    if (route.Hidden) continue;

                    var parameters = route.Representation().Trim();
                    var command = string.Join(" ", route.Nodes.Select(n => n.Names.First())).ToLower();
                    var description = route.Description;
                    var text = parameters;
                    if (!string.IsNullOrEmpty(parameters) && !string.IsNullOrEmpty(description)) text += " | ";
                    text += description;

                    if (command.Length > 15)
                    {
                        System.Console.WriteLine($"  {command,-15}");
                        System.Console.WriteLine($"  {"",-12}{text}");
                        
                    }
                    else
                    {
                        System.Console.WriteLine($"  {command,-15} {text}");
                    }
                }
                System.Console.WriteLine();
            }
        }

        public static void Write(string[] arguments)
        {
            int i = 0;
            foreach (var arg in arguments)
            {
                System.Console.WriteLine($" {++i}: '{arg}'");
            }
        }

        public static void Write(Exception e, bool stacktrace = false)
        {
            string message = GetErrorMessage(e);
            System.Console.Write($"Error: {message}");

            if (stacktrace) System.Console.WriteLine(e.StackTrace);
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