using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harthoorn.Shell.Routing
{

    public class Router
    {
        List<Route> AllRoutes;

        public Router() : this(Assembly.GetCallingAssembly())
        {
        }

        public Router(Assembly assembly)
        {
            this.AllRoutes = GetRoutes(assembly).ToList();
        }

        public void Handle(Arguments arguments)
        {
            bool ok = false;

            var routes = GetCommandRoutes(arguments);
            int count = routes.Count();
            
            if (count >= 1)
            {
                ok = BindAndRun(routes, arguments);
                // list all routes + parameters.
                //throw new Exception($"Command is ambiguous. Did you mean: \n" + string.Join("\n", routes));
            }

            if (!ok)
            {
                Console.WriteLine("Could not find a mathing command for these parameters.");
                Console.WriteLine("Did you mean:");
                foreach(var route in routes)
                {
                    Console.WriteLine("  "+route);
                }
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
                var selection = AllRoutes.FindGroup(group).ToList();
                if (selection.Any())
                {
                    arguments.RemoveHead();
                    if (arguments.TryGetHead(out string method))
                    {
                        var routes = selection.FindMethod(method).ToList();
                        if (routes.Any())
                        { 
                            arguments.RemoveHead();
                            return routes; 
                        }
                    }
                    else
                    {
                        return selection.FindMethod(group);
                    }
                }
                else
                {
                    var routes = AllRoutes.FindMethod(group).ToList();
                    if (routes != null)
                    {
                        arguments.RemoveHead();
                        return routes;
                    }
                }
            }

            return Enumerable.Empty<Route>(); 
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
                if (route.TryBind(arguments, out var values))
                {
                    Run(route.Method, values);
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