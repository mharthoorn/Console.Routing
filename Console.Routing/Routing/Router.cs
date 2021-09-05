using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        public IServiceProvider services;

        public Router(
            List<Route> routes,
            Binder binder,
            ArgumentParser parser,
            RoutingWriter writer,
            IServiceProvider services,
            IEnumerable<Type> globals = null,
            Action<Router, Exception> exceptionhandler = null)
        {
            this.Globals = globals?.ToList();
            this.Routes = routes;
            this.Binder = binder;
            this.Parser = parser;
            this.Writer = writer;
            this.services = services;
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

            if (result.Ok)
            {
                Invoke(result);
            }
            else
            {
                Writer.WriteResult(result);
            }

            return result;
        }

        public void Invoke(RoutingResult result)
        {
            try
            {
                var method = result.Bind.Route.Method;
                var instance = services.CreateInstance(method.DeclaringType, this);
                method.Invoke(instance, result.Bind.Parameters);
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException) e = e.InnerException;
                if (HandleException is object) HandleException(this, e); else throw e;
            }
        }

        private Bind CreateCaptureBind(Arguments arguments, Candidate candidate)
        {
            var args = arguments.WithoutCapture(candidate.Route.Capture);
            if (Binder.TryCreateBind(candidate.Route, args, out Bind bind))
                return bind;
            else 
                throw new Exception("Capture was invoked, but could not be matched");
        }


        public RoutingResult Bind(Arguments arguments)
        {
            Binder.Bind(Globals, arguments);

            if (TryGetCaptureCandidate(arguments, out Candidate candidate))
            {
                var bind = CreateCaptureBind(arguments, candidate);
                return CreateResult(arguments, candidate, bind);
                
            }
            else
            {
                var candidates = GetCandidates(arguments).ToList();
                var routes = candidates.Matching(RouteMatch.Full, RouteMatch.Default, RouteMatch.Capture);
                var binds = Binder.Bind(routes, arguments).ToList();

                return CreateResult(arguments, candidates, binds);
            }
        }


        public bool TryGetCaptureCandidate(Arguments arguments, out Candidate candidate)
        {
            foreach(var route in Routes.Where(r => r.Capture is not null))
            {
                if (route.Capture.Match(arguments))
                {
                    candidate = new Candidate(RouteMatch.Capture, route);
                    return true;
                }
            }
            candidate = null;
            return false;
        }

        private static RoutingResult CreateResult(Arguments arguments, Candidate candidate, Bind binding)
        {
            var candidates = new List<Candidate> { candidate };
            var bindings = new List<Bind> { binding };
            return CreateResult(arguments, candidates, bindings);
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


