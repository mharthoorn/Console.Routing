using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{


    public class RoutingWriter
    {
        private readonly Documentation documentation;

        public RoutingWriter(Documentation doc)
        {
            this.documentation = doc;
        }

        public void WriteResult(RoutingResult result)
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
                        WriteCandidates(result.Candidates.Matching(RouteMatch.Default));
                    }
                    
                    break;

                case RoutingStatus.PartialCommand:
                    Console.WriteLine("Did you mean:");
                    WriteCandidates(result.Candidates.Matching(RouteMatch.Partial));
                    break;

                case RoutingStatus.InvalidParameters:
                    Console.WriteLine("Invalid parameter(s). These are your options:");
                    WriteCandidates(result.Candidates.Matching(RouteMatch.Full));
                    break;

                case RoutingStatus.AmbigousParameters:
                    Console.WriteLine("Ambigous parameter(s). These are your options:");
                    WriteCandidates(result.Routes);
                    break;

                case RoutingStatus.InvalidDefault:
                    Console.WriteLine("Invalid parameter(s). These are your options:");
                    WriteCandidates(result.Candidates.Matching(RouteMatch.Default));
                    break;
            }
           
        }

        public void WriteRoutes(IEnumerable<Route> routes)
        {
            var groups = routes.Where(r => r.Hidden == false).GroupBy(r => r.Module);
            foreach (var group in groups)
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
        
        public void WriteRouteHelp(Route route)
        {
            WriteWrouteCommand(route);
            WriteWrouteDescription(route);
            var doc = documentation.GetDoc(route);
            WriteRouteParameters(route, doc);
            WriteRouteDocumentation(doc);
        }

        public void WriteRouteHelp(RoutingResult result)
        {
            var routes = result.Candidates.Matching(RouteMatch.Full).ToList();
            if (routes.Count == 0) routes = result.Candidates.Matching(RouteMatch.Partial).ToList();

            if (routes.Count > 1)
            {
                Console.WriteLine("There are multiple routes that match: ");
                WriteRoutes(routes);
                return;
            }

            var route = routes.FirstOrDefault();
            if (route is null)
            {
                Console.WriteLine("Cannot help. No matching command was found");
                return;
            }

            WriteRouteHelp(route);

        }



        private void WriteCandidates(IEnumerable<Route> routes)
        {
            foreach (var route in routes) Console.WriteLine($"  {route}");
        }

        private void WriteWrouteCommand(Route route)
        {
            //string path = route.GetCommandPath();
            Console.WriteLine($"Command:");
        
            string commands = string.Join(" ", route.Nodes).ToLower();
            var parameters = ParametersAsText(route.Method);

            string s = commands;
            if (parameters.Length > 0) s += " " + parameters;
            Console.WriteLine($"  {s}");
        }

        private static string ParametersAsText(MethodInfo method)
        {
            var parameters = method.GetParameters().AsRoutingParameters();
            return string.Join(" ", parameters.Select(p => ParameterAsText(p)));
        }

        public static string ParameterAsText(Parameter parameter)
        {
            return ParameterAsText(parameter.Type, parameter.Name, parameter.Optional);
        }

        public static string ParameterAsText(Type type, string name, bool optional = false)
        {
            string rep;

            if (type == typeof(Flag) || type == typeof(bool))
            {
                rep = $"--{name}";
            }
            else if (type == typeof(Assignment))
            {
                rep = $"{name}=<value>";
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Flag<>))
            {
                rep = $"--{name} <value>";
            }
            else if (type == typeof(Arguments))
            {
                rep = $"<{name}>...";
            }
            else if (type == typeof(string))
            {
                rep = $"<{name}>";
            }
            else if (type.HasAttribute<Bucket>())
            {
                rep = "<flags>";
            }
            else
            {
                rep = $"{name}";
            }
            if (optional) rep = $"({rep})";

            return rep;
        }


     

        private void WriteWrouteDescription(Route route)
        {
            if (route.Description is not null)
            {
                Console.WriteLine($"\nDescription:");
                Console.WriteLine(route.Description);
            }
        }

        private void WriteRouteParameters(Route route, MemberDoc doc)
        {
            var parameters = route.GetRoutingParameters().ToList();
            if (parameters.Count > 0)
            {
                Console.WriteLine($"\nParameters:");
                foreach (var parameter in route.GetRoutingParameters())
                {
                    if (parameter.Type.HasAttribute<Bucket>())
                        WriteBucketParameters(parameter.Type);
                    else 
                        WriteRoutingParameter(parameter, doc);
                }
            }
        }

        private void WriteRoutingParameter(Parameter parameter, MemberDoc? memberdoc)
        {
            string text = ParameterAsText(parameter);
            var paramdoc = memberdoc?.GetParamDoc(parameter.Name);
            WriteRoutingParameter(text, paramdoc);
        }

        private void WriteRoutingParameter(string display, string doc)
        {
            if (doc is not null)
                Console.WriteLine($"  {display,-20} {doc}");
            else
                Console.WriteLine($"  {display}");
        }

      

        private void WriteBucketParameters(Type type)
        {
            var members = type.GetFieldsAndProperties();
            foreach(var member in members)
            {
                var doc = documentation.GetMemberDoc(member);
                string display = ParameterAsText(member.GetMemberType(), member.Name);
                WriteRoutingParameter(display, doc?.Text);
            }
        }

        private void WriteRouteDocumentation(MemberDoc doc)
        {
            if (doc is not null)
            {
                Console.WriteLine($"\nDocumentation:");
                Console.WriteLine($"{doc.Text}\n");
            }
            
        }

        private void WriteRouteDescription(Route route)
        {
            if (route.Hidden) return;
            var parameters = route.AsText().Trim();
            var command = route.GetCommandPath();
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
        
        public static void WriteException(Exception e, bool stacktrace = false)
        {
            string message = e.GetErrorMessage();
            Console.Write($"Error: {message}");

            if (stacktrace) Console.WriteLine(e.StackTrace);
        }
        
    }

}