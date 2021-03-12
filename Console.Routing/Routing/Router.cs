using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
{

    public class Router
    {
        public List<Route> Routes { get; }
        public Binder Binder;
        private List<Type> Globals;
        public bool DebugMode { get; set; }
        public Action<Router, Exception> HandleException;

        public Router(
            List<Route> routes, 
            Binder binder,
            IEnumerable<Type> globals = null,
            Action<Router, Exception> exceptionhandler = null)
        {
            this.Globals = globals?.ToList();
            this.Routes = routes;
            this.Binder = binder;
            HandleException = exceptionhandler ?? DefaultExceptionHandler.Handle;
        }

        public RoutingResult Handle(Arguments arguments)
        {
            RoutingResult result = Bind(arguments);

            if (result.Ok)
            {
                Run(result);
            }

            return result;
        }

        public void Run(RoutingResult result)
        {
            try
            {
                Invoker.Run(this, result.Bind);
            }
            catch (Exception e)
            {
                if (HandleException is object) HandleException(this, e); else throw;
            }
        }

        public RoutingResult Bind(Arguments arguments)
        {
            Binder.Bind(Globals, arguments);
            var candidates = GetCandidates(arguments).ToList();
            var routes = candidates.GetRoutes(RouteMatch.Full, RouteMatch.Default);
            var binds = Bind(routes, arguments).ToList();

            return CreateResult(arguments, candidates, binds);
        }

        private IEnumerable<Bind> Bind(IEnumerable<Route> routes, Arguments arguments)
        {
            foreach (var route in routes)
            {
                if (Binder.TryBind(route, arguments, out var bind))
                {
                    yield return bind;
                }
            }
        }

        public IEnumerable<Candidate> GetCandidates(Arguments arguments)
        {
            foreach (var route in Routes)
            {
                var match = TestMatch(route, arguments);
                if (match == RouteMatch.Not) continue;
                else yield return new Candidate(match, route);
            }
        }

        private RouteMatch TestMatch(Route route, Arguments arguments)
        {
            int index = 0;
            int count = route.Nodes.Count;

            while (index < count)
            {
                if (arguments.TryGetCommand(index, out string value))
                {
                    if (route.Nodes[index].Matches(value))
                    {
                        index++;
                    }
                    else break;

                }
                else break;
            }
            return MatchType(count, index);
        }

        private static RouteMatch MatchType(int nodeCount, int index)
        {
            if (nodeCount == 0) return RouteMatch.Default;
            if (index == 0) return RouteMatch.Not;
            if (index > 0 && index == nodeCount) return RouteMatch.Full;
            return RouteMatch.Partial;
        }

        private static RoutingResult CreateResult(Arguments arguments, List<Candidate> candidates, List<Bind> bindings)
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
            int partial = candidates.Count(RouteMatch.Partial);
            int def = candidates.Count(RouteMatch.Default);
            int full = candidates.Count(RouteMatch.Full);
            return (partial, def, full);
        }
    }
}


