using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{
    
    public class RoutingResult
    {
        public RoutingResult(Arguments arguments, RoutingStatus status, IList<Bind> binds, IList<Route> candidates)
        {
            this.Arguments = arguments;
            this.Candidates = candidates;
            this.Binds = binds;
            this.Status = status;
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

        public override string ToString()
        {
            string okstr = Ok ? "Ok" : "Failed";
            string nrs = (Count == 1) ? "" : $" {Count}";
            if (!(Candidates is null)) nrs += $"/{Candidates?.Count}";
            return $" {okstr} {nrs}";
        }
    }



}


