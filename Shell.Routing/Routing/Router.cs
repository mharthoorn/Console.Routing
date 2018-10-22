using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shell.Routing
{

    public class Router
    {
        List<Route> routes { get; }

        public Router(Assembly assembly)
        {
            this.routes = GetRoutes(assembly).ToList();
        }

        public RoutingResult Route(Arguments arguments)
        {
            var result = new RoutingResult();
            ConsumeCommands(arguments, out var routes);

            result.CommandRoutes = routes.ToList();

            if (result.CommandRoutes.Count == 0)
            {
                result.Status = RoutingStatus.NoCommands;
                return result;
            }

            result.Binds = Bind(result.CommandRoutes, arguments).ToList();

            if (result.Binds.Count == 0)
            {
                result.Status = RoutingStatus.NoMatchingParameters;
            }
            else if (result.Binds.Count == 1)
            {
                result.Status = RoutingStatus.Ok;
            }
            else
            {
                result.Status = RoutingStatus.AmbigousParameters;
            }
            return result;
        }

        public RoutingResult Handle(Arguments arguments)
        {
            RoutingResult result = Route(arguments);

            if (result.Ok)
                Invoker.Run(result.Match);
            
            return result;
        }

        private static IEnumerable<Route> GetRoutes(Assembly assembly)
        {
            var groups = assembly.GetAttributeTypes<Module>().ToList();

            foreach (var (type, group) in groups)
            {
                foreach (var (method, command) in type.GetAttributeAndMethods<Command>())
                {
                    yield return new Route(group, command, type, method);
                }
            }
        }

        public void ConsumeCommands(Arguments arguments, out IEnumerable<Route> routes)
        {
            var commands = new Commands(arguments);
            routes = GetCommandRoutes(commands);
            commands.Consume();
        }

        public IEnumerable<Route> GetCommandRoutes(Commands commands)
        {
            if (commands.TryGetHead(out string group))
            {
                var selection = routes.FindGroup(group).ToList();
                if (selection.Any())
                {
                    commands.UseHead();
                    if (commands.TryGetHead(out string method))
                    {
                        var routes = selection.FindMethod(method);
                        if (routes.Any())
                        {
                            commands.UseHead();
                            return routes;
                        }
                        else
                        {
                            routes = selection.FindMethod(group);
                            if (routes.Any())
                            {
                                return routes;
                            }
                        }
                    }
                    else
                    {
                        return selection.FindMethod(group);
                    }
                }
                else
                {
                    var filter = routes.FindMethod(group).ToList();
                    if (filter != null)
                    {
                        commands.UseHead();
                        return filter;
                    }
                }
            }

            return Enumerable.Empty<Route>();
        }

        public IEnumerable<Bind> Bind(IEnumerable<Route> routes, Arguments arguments)
        {
            foreach (var route in routes)
            {
                if (route.TryBind(arguments, out var values))
                {
                    yield return new Bind(route, values);
                }
            }
        }

    }

}


