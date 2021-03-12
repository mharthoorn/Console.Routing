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
            else Writer.WriteResult(result);
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
            var routes = candidates.Matching(RouteMatch.Full, RouteMatch.Default);
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
}


