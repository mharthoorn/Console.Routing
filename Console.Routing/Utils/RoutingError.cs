using System.Collections.Generic;

namespace ConsoleRouting
{
    public class RoutingError
    {
        public string Message;
        public IList<Route> Candidates;
    }

}