using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harthoorn.Shell.Routing
{

    public class Router
    {
        public List<Route> Routes { get; }

        public Router() : this(Assembly.GetCallingAssembly())
        {
        }

        public Router(Assembly assembly)
        {
            this.Routes = GetRoutes(assembly).ToList();
        }

        public IEnumerable<Bind> GetBoundRoutes(Arguments arguments)
        {
            var routes = GetCommandRoutes(arguments);
            var binds = BindRoutes(routes, arguments);
            return binds;
        }

        public void Handle(Arguments arguments)
        {
            bool ok = false;
            var binds = GetBoundRoutes(arguments).ToList();
            int count = binds.Count();
            
            if (count == 1)
            {
                Run(binds.First());
            }
            else
            {
                //onfail(binds);
            }
        }

        public void DefaultOnFail(List<Bind> binds)
        {
            // no matching parameters (0), see if we can list the other routes.
            // multiple matching parameters... throw, because it's the programmers fault?

            if (binds.Count == 0)
            {
                Console.WriteLine("Could not find a mathing command for these parameters.");
                Console.WriteLine("Did you mean:");
                //foreach (var route in routes)
                //{
                //    Console.WriteLine("  " + route);
                //}
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
                    var routes = Routes.FindMethod(group).ToList();
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

        public IEnumerable<Bind> BindRoutes(IEnumerable<Route> routes, Arguments arguments)
        {
            foreach (var route in routes)
            {
                if (route.TryBind(arguments, out var values))
                {
                    yield return new Bind(route, values);
                }
            }
        }

        private bool BindAndRun(IEnumerable<Route> routes, Arguments arguments)
        {
            var binds = BindRoutes(routes, arguments).ToList();
            if (binds.Count == 0)
            {
                
            }
            if (binds.Count == 1)
            {
                Run(binds.First());
            }

            if (binds.Count > 2)
            {

            }

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

        public void Run(Bind bind)
        {
            Run(bind.Route.Method, bind.Arguments);
        }

        public void Test(Arguments arguments)
        {
            var routes = GetCommandRoutes(arguments);
            BindAndRun(routes, arguments);
        }
    }
   

}