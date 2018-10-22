using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{
    public class RoutingResult
    {
        public bool Ok => Status == RoutingStatus.Ok;
        public Bind Match => Binds.FirstOrDefault(); // todo: condition on ok.
        public RoutingStatus Status;
        public List<Route> CommandRoutes;
        public List<Bind> Binds;
    }

}


