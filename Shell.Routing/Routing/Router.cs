using System;
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

        public RoutingResult Bind(Arguments arguments)
        {
            var candidates = ElectCandidates(arguments).ToList();
            var routes = candidates.Routes(CommandMatch.Full, CommandMatch.Default);
            var binds = Bind(routes, arguments).ToList();

            return CreateResult(arguments, candidates, binds);
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

        public IEnumerable<Candidate> ElectCandidates(Arguments arguments)
        {
            foreach (var route in Routes)
            {
                var match = TryMatchCommands(route, arguments);
                if (match == CommandMatch.Not) continue;
                else yield return new Candidate(match, route);
            }
        }

        private static bool TryBind(Route route, Arguments arguments, out Bind bind)
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

        private CommandMatch TryMatchCommands(Route route, Arguments arguments)
        {
            int index = 0;
            int length = route.Nodes.Count;

            while (index < length)
            {
                if (arguments.TryGet(index, out Literal literal))
                {
                    if (route.Nodes[index].Matches(literal))
                    {
                        index++;
                        //if (index == length) return true;
                    }
                    else break;

                }
                else break;
            }
            return MapCommandMatch(length, index);
        }

        private static CommandMatch MapCommandMatch(int nodeCount, int index)
        {
            if (nodeCount == 0) return CommandMatch.Default;
            if (index == 0) return CommandMatch.Not;
            if (index > 0 && index == nodeCount) return CommandMatch.Full;
            return CommandMatch.Partial;
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

                else if (param.Type.IsEnum)
                {
                    if (arguments.TryGetEnum(ia, param, out object value))
                    {
                        values[ip++] = value;
                        used++;
                    }
                    else
                    {
                        return false;
                    }

                }

                else if (param.Type == typeof(Assignment))
                {
                    if (arguments.TryGet(param, out Assignment assignment))
                    {
                        values[ip++] = assignment;
                        used++;
                    }
                    else
                    {
                        values[ip++] = Assignment.NotProvided();
                    }
                }

                else if (param.Type == typeof(FlagValue))
                {
                    if (arguments.TryGetFlagValue(param, out string value))
                    {
                        values[ip++] = new FlagValue(value);
                        used += 2;
                    }
                    else
                    {
                        values[ip++] = new FlagValue(null, provided: false);
                    }
                }

                else if (param.Type == typeof(Flag))
                {
                    if (arguments.TryGet(param, out Flag flag))
                    {
                        values[ip++] = flag;
                        used++;
                    }
                    else
                    {
                        values[ip++] = new Flag(param.Name, set: false);
                    }
                }

                else if (param.Type == typeof(bool))
                {
                    if (arguments.TryGet(param, out Flag flag))
                    {
                        values[ip++] = flag.Set;
                        used++;
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

        private static RoutingResult CreateResult(Arguments arguments, IList<Candidate> candidates, IList<Bind> bindings)
        {
            (int partial, int def, int full) = Count(candidates);
            int binds = bindings.Count;
            var status = MapRoutingStatus(binds, partial, def, full);

            return new RoutingResult(arguments, status, bindings, candidates);

        }

        private static RoutingStatus MapRoutingStatus(int binds, int partial, int def, int full)
        {
            if (binds == 1)
            {
                return RoutingStatus.Ok;
            }
            else if (binds == 0)
            {
                if (full > 0) return RoutingStatus.InvalidParameters;
                if (partial > 0) return RoutingStatus.PartialCommand;
                //if (def > 0) return RoutingStatus.InvalidDefault;
                return RoutingStatus.UnknownCommand;
            }
            else // if (binds.Count > 1)
            {
                return RoutingStatus.AmbigousParameters;
            }

            throw new System.Exception("Invalid status");
        }

        private static (int partial, int def, int full) Count(IEnumerable<Candidate> candidates)
        {
            int partial = candidates.Count(CommandMatch.Partial);
            int def = candidates.Count(CommandMatch.Default);
            int full = candidates.Count(CommandMatch.Full);
            return (partial, def, full);
        }
    }

}


