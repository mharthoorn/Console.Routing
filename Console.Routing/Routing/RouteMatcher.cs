using System.Collections.Generic;

namespace ConsoleRouting
{
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


