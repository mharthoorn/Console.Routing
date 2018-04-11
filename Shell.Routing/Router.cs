using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shell.Routing
{
    public enum RoutingStatus
    {
        Ok,
        NoCommands,
        NoMatchingParameters,
        AmbigousParameters,
    }

    public class RoutingResult
    {
        public bool Ok => Status == RoutingStatus.Ok;
        public Bind Match => Binds.FirstOrDefault(); // todo: condition on ok.
        public RoutingStatus Status;
        public List<Route> CommandRoutes;
        public List<Bind> Binds;
    }

    //public class RouterException : Exception
    //{
    //}

    //public class NoMatchingCommandsException : RouterException
    //{

    //}

    //public class AmbiguousParametersException : RouterException
    //{

    //}

    //public class NoMatchingParametersException : RouterException
    //{
    //    public List<Bind> Binds;
    //}

    public class Router
    {
        public List<Route> Routes { get; }

        public Router(Assembly assembly)
        {
            this.Routes = GetRoutes(assembly).ToList();
        }

        public RoutingResult Route(Arguments arguments)
        {
            var result = new RoutingResult();
            result.CommandRoutes = GetCommandRoutes(arguments).ToList();

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
            if (result.Ok) Run(result.Match);
            
            return result;
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
            var groups = assembly.GetAttributeTypes<Module>().ToList();

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
                        var routes = selection.FindMethod(method);
                        if (routes.Any())
                        {
                            arguments.RemoveHead();
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

        private bool BindAndRun(IEnumerable<Route> routes, Arguments arguments)
        {
            var binds = Bind(routes, arguments).ToList();
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
            try
            {
                var instance = Activator.CreateInstance(method.DeclaringType);
                method.Invoke(instance, arguments);
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException) throw e.InnerException;
            }
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