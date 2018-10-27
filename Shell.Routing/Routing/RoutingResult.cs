using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{
    public class RoutingResult
    {
        public bool Ok => Status == RoutingStatus.Ok;
        public Bind Bind => Binds.SingleOrDefault();
        public Route Route => Bind.Endpoint;

        public int Count => Binds.Count;
        public IEnumerable<Route> Routes => Binds.Select(b => b.Endpoint);

        public RoutingStatus Status;
        public IList<Route> Candindates; //where the commands match, but not necessarily the parameters
        public List<Bind> Binds;


        
    }



}


