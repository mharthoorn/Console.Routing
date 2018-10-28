using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shell.Routing
{

    public class Router
    {
        public IList<Route> Routes { get; }

        public Router(IList<Route> routes)
        {
            this.Routes = routes;
        }

        public RoutingResult Handle(Arguments arguments)
        {
            RoutingResult result = Bind(arguments);

            if (result.Ok)
                Invoker.Run(result.Bind);
            
            return result;
        }

        public IEnumerable<Route> BindCommands(Arguments arguments)
        {
            foreach(var route in Routes)
            {
                if (TryMatchCommands(route, arguments))
                {
                    yield return route;
                }
            }
        }

        public bool TryMatchCommands(Route route, Arguments arguments)
        {
            int index = 0;
            int length = route.Nodes.Count;

            while (index < length)
            {
                if (arguments.TryGetHead(index, out Literal literal))
                {
                    if (route.Nodes[index].Matches(literal))
                    {
                        index++;
                        //if (index == length) return true;
                    }
                    else return false;
                    
                }
                else return false;
            }
            return true;
        }

        private static bool TryBuildParameters(Route route, Arguments arguments, out object[] values)
        {
            var parameters = route.Method.GetRoutingParameters().ToArray();
            var offset = route.Nodes.Count(); // amount of parameters to skip, because they are commands.
            var argcount = arguments.Count - offset;
            var count = parameters.Length;

            values = new object[count];
            int ip = 0; // index of parameters
            int used = 0; // arguments used;

            foreach (var param in parameters)
            {
                int ia = offset + ip; // index of arguments
                if (param.Type == typeof(string))
                {
                    if (arguments.TryGetLiteral(ia, out string value))
                    {
                        values[ip++] = value;
                        used++;
                    }

                    else if (param.Optional)
                    {
                        values[ip++] = null;
                    }
                    else
                    {
                        return false;
                    }
                }

                else if (param.Type == typeof(Assignment))
                {
                    if (arguments.TryGet(param.Name, out Assignment assignment))
                    {
                        values[ip++] = assignment;
                        used++;
                    }
                }

                else if (param.Type == typeof(FlagValue))
                {
                    if (arguments.TryGetFlagValue(param.Name, out string value))
                    {
                        values[ip++] = new FlagValue(value, 2);
                        used += 2;
                    }
                    else
                    {
                        values[ip++] = new FlagValue(null, 0, provided: false);
                    }
                }

                else if (param.Type == typeof(Flag))
                {
                    if (arguments.TryGet(param.Name, out Flag flag))
                    {
                        values[ip++] = flag;
                        used++;
                    }
                    else
                    {
                        values[ip++] = new Flag(param.Name, set: false);
                    }
                }

                else if (param.Type == typeof(Arguments))
                {
                    values[ip++] = arguments;
                    return true;
                }
                else
                {
                    // this method has a signature with an unknown type.
                    return false;
                }
            }
            return (argcount == used);

        }

        public IEnumerable<Bind> Bind(IEnumerable<Route> routes, Arguments arguments)
        {
            foreach (var route in routes)
            {
                if (TryBind(route, arguments, out var bind))
                {
                    yield return bind;
                }
            }
        }

        public static bool TryBind(Route route, Arguments arguments, out Bind bind)
        {
            if (TryBuildParameters(route, arguments, out var values))
            {
                bind = new Bind(route, values);
                return true;
            }
            else
            {
                bind = null;
                return false;
            }
        }

        public RoutingResult Bind(Arguments arguments)
        {
            var candidates = BindCommands(arguments).ToList();
            var binds = Bind(candidates, arguments).ToList();

            return CreateResult(arguments, candidates, binds);
        }

        private static RoutingResult CreateResult(Arguments arguments, IList<Route> commandscandidates, IList<Bind> binds)
        {
            IList<Route> candidates = null;
            RoutingStatus status;

            if (binds.Count == 1)
            {
                status = RoutingStatus.Ok;
            }
            else if (binds.Count == 0)
            {
                candidates = commandscandidates.NonDefault().ToList();
                status = (candidates.Count > 0) ? RoutingStatus.NoMatchingParameters : RoutingStatus.NoMatchingCommands;
            }
            else // if (binds.Count > 1)
            {
                candidates = binds.Select(b => b.Route).NonDefault().ToList();
                status = (candidates.Count > 0) ? RoutingStatus.AmbigousParameters : RoutingStatus.NoMatchingCommands;
            }
            return new RoutingResult(arguments, status, binds, candidates);

        }

    }

}


