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


    [Obsolete("Use RoutingWriter instead")]
    public static class RoutingPrinter
    {

    }
      
    public static class RoutingWriter
    {
        public static void Write(RoutingResult result)
        {
            switch(result.Status)
            {
                case RoutingStatus.Ok:
                    Console.WriteLine("The command was found and understood.");
                    break;

                case RoutingStatus.UnknownCommand:
                    var arg = result.Arguments.FirstOrDefault();
                    if (arg is Text)
                    {
                        Console.WriteLine($"Unknown command: {arg}.");
                    }
                    else
                    {
                        Console.WriteLine($"Invalid parameter(s). These are your options:");
                        WriteCandidates(result.Candidates.Routes(RouteMatch.Default));
                    }
                    
                    break;

                case RoutingStatus.PartialCommand:
                    Console.WriteLine("Did you mean:");
                    WriteCandidates(result.Candidates.Routes(RouteMatch.Partial));
                    break;

                case RoutingStatus.InvalidParameters:
                    Console.WriteLine("Invalid parameter(s). These are your options:");
                    WriteCandidates(result.Candidates.Routes(RouteMatch.Full));
                    break;

                case RoutingStatus.AmbigousParameters:
                    Console.WriteLine("Ambigous parameter(s). These are your options:");
                    WriteCandidates(result.Routes);
                    break;

                case RoutingStatus.InvalidDefault:
                    Console.WriteLine("Invalid parameter(s). These are your options:");
                    WriteCandidates(result.Candidates.Routes(RouteMatch.Default));
                    break;
            }
           
        }

        public static void WriteCandidates(IEnumerable<Route> routes)
        {
            
            foreach (var route in routes) Console.WriteLine($"  {route}");
        }

        public static void WriteRoutes(IEnumerable<Route> routes)
        {
            foreach (var group in routes.Where(r => r.Hidden == false).GroupBy(r => r.Module))
            {
                if (group.Count() == 0) continue;

                var title = group.Key.Title ?? group.FirstOrDefault()?.Method.DeclaringType.Name ?? "Module";
                Console.WriteLine($"{title}:");

                foreach (var route in group)
                {
                    WriteRouteDescription(route);
                }
                Console.WriteLine();
            }
        }

        public static void WriteRoutes(Router router) => WriteRoutes(router?.Routes);
        
        public static void WriteWrouteCommand(Route route)
        {
            string path = route.CommandPath();
            Console.WriteLine($"Command:");
            Console.WriteLine($"  {route}");
        }

        public static void WriteWrouteDescription(Route route)
        {
            if (route.Description is not null)
            {
                Console.WriteLine($"\nDescription:");
                Console.WriteLine(route.Description);
            }
        }

        public static void WriteRouteParameters(Route route)
        {
            var parameters = route.GetRoutingParameters().ToList();
            if (parameters.Count > 0)
            {
                Console.WriteLine($"\nParameters:");
                foreach (var parameter in route.GetRoutingParameters())
                {
                    var paramdoc = route.GetParamDoc(parameter.Name);
                    if (paramdoc.HasValue())
                        Console.WriteLine($"  {parameter.AsText(),-20} {paramdoc}");
                    else
                        Console.WriteLine($"  {parameter.AsText()}");
                }
            }
        }

        public static void WriteRouteDocumentation(Route route)
        {
            string doc = route.GetMethodDoc();
            if (doc.HasValue())
            {
                Console.WriteLine($"\nDocumentation:");
                Console.WriteLine($"{doc}\n");
            }
        }

        public static void WriteRouteHelp(Route route)
        {
            WriteWrouteCommand(route);
            WriteWrouteDescription(route);
            WriteRouteParameters(route);
            WriteRouteDocumentation(route);
        }

        public static void WriteRouteHelp(this Router router, Arguments args)
        {
            var routes = router.GetCandidates(args).Routes(RouteMatch.Full).ToList();
            var route = routes.FirstOrDefault();
            if (route is null) 
            { 
                Console.WriteLine("No matching command was found"); 
                return; 
            }

            WriteRouteHelp(route);

        }

        private static string CommandPath(this Route route)
        {
            var path = string.Join(" ", route.Nodes.Select(n => n.Names.First())).ToLower();
            return path;
        }

        public static void WriteRouteDescription(Route route)
        {
            if (route.Hidden) return;
            var parameters = route.AsText().Trim();
            var command = route.CommandPath();
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