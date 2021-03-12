using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
{

    public class Router
    {
        public List<Route> Routes { get; }
        public Binder Binder;
        public ArgumentParser Parser;
        public RoutingWriter Writer;
        private List<Type> Globals;
        public bool DebugMode { get; set; }
        public Action<Router, Exception> HandleException;

        public Router(
            List<Route> routes,
            Binder binder,
            ArgumentParser parser,
            RoutingWriter writer,
            IEnumerable<Type> globals = null,
            Action<Router, Exception> exceptionhandler = null)
        {
            this.Globals = globals?.ToList();
            this.Routes = routes;
            this.Binder = binder;
            this.Parser = parser;
            this.Writer = writer;
            HandleException = exceptionhandler ?? DefaultExceptionHandler.Handle;
        }

        public RoutingResult Handle(string[] args)
        {
            Arguments arguments = Parser.Parse(args);
            return Handle(arguments);
        }

        public RoutingResult Handle(Arguments arguments)
        {
            RoutingResult result = Bind(arguments);

            if (result.Ok) Run(result);
            else Writer.Write(result);
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
            var binds = Binder.Bind(routes, arguments).ToList();

            return CreateResult(arguments, candidates, binds);
        }

        private static RoutingResult CreateResult(Arguments arguments, List<Candidate> candidates, List<Bind> bindings)
        {
            (int partial, int full) = RouteMatcher.Tally(candidates);
            int binds = bindings.Count;
            var status = RouteMatcher.MapRoutingStatus(binds, partial, full);

            return new RoutingResult(arguments, status, bindings, candidates);

        }

        public IEnumerable<Candidate> GetCandidates(Arguments arguments)
        {
            foreach (var route in Routes)
            {
                var match = RouteMatcher.Match(route, arguments);
                if (match == RouteMatch.Not) continue;
                else yield return new Candidate(match, route);
            }
        }

      

    
    
   
  
    }

    public static class RouteMatcher
    {
        public static RouteMatch Match(Route route, Arguments arguments)
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

        public static RoutingStatus MapRoutingStatus(int binds, int partial, int full)
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

        public static (int partial, int full) Tally(this IEnumerable<Candidate> candidates)
        {
            int partial = candidates.Count(RouteMatch.Partial);
            int full = candidates.Count(RouteMatch.Full);
            return (partial, full);
        }

    }
}


