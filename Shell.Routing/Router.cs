using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harthoorn.Shell.Routing
{

    public class Router
    {
        List<Route> Routes;

        public Router() : this(Assembly.GetExecutingAssembly())
        {
        }

        public Router(Assembly assembly)
        {
            this.Routes = GetRoutes(assembly).ToList();
        }

        public void Handle(Arguments arguments)
        {
            var routes = GetCommandRoutes(arguments);
            var count = routes.Count();
            switch (count)
            {
                case 0: throw new Exception($"Could not find a command matching {arguments}");
                case 1: BindAndRun(routes, arguments); break;
                default: throw new Exception($"Command is ambiguous. Did you mean: \n" + string.Join("\n", routes));
            }
        }

        private static IEnumerable<Route> GetRoutes(Assembly assembly)
        {
            var groups = assembly.GetAttributeTypes<Section>().ToList();

            foreach (var (type, group) in groups)
            {
                foreach (var (method, command) in type.GetAttributeAndMethods<Command>())
                {
                    yield return new Route(group, command, type, method);
                }
            }
        }

        public IEnumerable<Route> GetCommandRoutes(Arguments arguments)
        {
            if (arguments.TryGetHead(out string group))
            {
                var selection = Routes.FindGroup(group).ToList();
                if (selection.Any())
                {
                    arguments.RemoveHead();
                    if (arguments.TryGetHead(out string method))
                    {
                        var routes = selection.FindMethod(method).ToList();
                        if (Routes.Any())
                        {
                            arguments.RemoveHead();
                            return Routes; 
                        }
                    }
                    else
                    {
                        return selection.FindMethod(group);
                    }
                }
                else
                {
                    var routes = Routes.FindMethod(group).ToList();
                    if (routes != null)
                    {
                        arguments.RemoveHead();
                        return routes;
                    }
                }
            }

            return null;
        }

        public void Run(MethodInfo method, Arguments arguments)
        {
            var instance = Activator.CreateInstance(method.DeclaringType);
            method.Invoke(instance, new[] { arguments });
        }

        public void Run(Route route, Arguments arguments)
        {
            Run(route.Method, arguments);
        }

        private bool BindAndRun(IEnumerable<Route> routes, Arguments arguments)
        {
            foreach (var route in routes)
            {
                var method = route.Method;
                if (method.TryBind(arguments, out var values))
                {
                    Run(method, values);
                    return true;
                }
            }
            return false;
        } 

        public void Run(MethodInfo method, object[] arguments)
        {
            
            var instance = Activator.CreateInstance(method.DeclaringType);
            method.Invoke(instance, arguments);
        }

        public void Test(Arguments arguments)
        {
            var routes = GetCommandRoutes(arguments);
            BindAndRun(routes, arguments);
        }
    }

}