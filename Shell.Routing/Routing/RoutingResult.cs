using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{
    public class RoutingResult
    {
        public RoutingResult(Arguments arguments, IList<Route> candidates, IList<Bind> binds)
        {
            this.Arguments = arguments;
            this.Candidates = candidates;
            this.Binds = binds;
            this.Status = SetStatus();
        }

        private RoutingStatus SetStatus()
        {
            if (Binds.Count == 1) return RoutingStatus.Ok;

            if (Candidates.Count == 0) return RoutingStatus.NoMatchingCommands;

            if (Candidates.Count == 1)
            {
                if (Binds.Count == 0) return RoutingStatus.NoMatchingParameters;
                if (Binds.Count > 1) return RoutingStatus.AmbigousParameters;
            }

            if (Candidates.Count > 1)
            {
                if (Binds.Count == 0) return RoutingStatus.NoMatchingParameters;
                if (Binds.Count > 1) return RoutingStatus.AmbigousParameters;
            }

            return RoutingStatus.NoMatchingCommands; // shouldn't be possible.
        }

        public bool Ok => Status == RoutingStatus.Ok;
        public Bind Bind => Binds.SingleOrDefault();
        public Route Route => Bind?.Route;
        public int Count => Binds.Count;
        public IEnumerable<Route> Routes => Binds.Select(b => b.Route);

        public RoutingStatus Status;
        public Arguments Arguments;
        public IList<Route> Candidates; //where the commands match, but not necessarily the parameters
        public IList<Bind> Binds;
    }



}


