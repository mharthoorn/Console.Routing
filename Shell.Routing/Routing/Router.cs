using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shell.Routing
{

    public class Router
    {
        IList<Route> endpoints;

        public Router(IList<Route> endpoints)
        {
            this.endpoints = endpoints;
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
            foreach(var endpoint in endpoints)
            {
                if (TryMatchCommands(endpoint, arguments))
                {
                    yield return endpoint;
                }
            }
        }

        public bool TryMatchCommands(Route endpoint, Arguments arguments)
        {
            int index = 0;
            int length = endpoint.Nodes.Count;

            while (true)
            {
                if (arguments.TryGetHead(index, out Literal literal))
                {
                    if (endpoint.Nodes[index].Matches(literal))
                    {
                        index++;
                        if (index == length) return true;
                    }
                    else return false;
                    
                }
                else return false;
            }
        }

        private static bool TryBuildParameters(Route endpoint, Arguments arguments, out object[] values)
        {
            var parameters = endpoint.Method.GetRoutingParameters().ToArray();
            var offset = endpoint.Nodes.Count(); // amount of parameters to skip, because they are commands.
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

        public IEnumerable<Bind> Bind(IEnumerable<Route> endpoints, Arguments arguments)
        {
            foreach (var endpoint in endpoints)
            {
                if (TryBind(endpoint, arguments, out var bind))
                {
                    yield return bind;
                }
            }
        }

        public static bool TryBind(Route endpoint, Arguments arguments, out Bind bind)
        {
            if (TryBuildParameters(endpoint, arguments, out var values))
            {
                bind = new Bind(endpoint, values);
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
            var result = new RoutingResult();
            result.Candindates = BindCommands(arguments).ToList();
            
            if (result.Candindates.Count == 0)
            {
                result.Status = RoutingStatus.NoCommands;
                return result;
            }

            result.Binds = Bind(result.Candindates, arguments).ToList();

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


    }

}


