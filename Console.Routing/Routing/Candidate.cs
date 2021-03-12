using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
{

    public enum RouteMatch
    {
        Default,
        Not,
        Partial,
        Full, 
    }

    public class Candidate
    {
        public RouteMatch Match;
        public Route Route;

        public Candidate(RouteMatch match, Route route)
        {
            this.Match = match;
            this.Route = route;
        }

        public override string ToString()
        {
            return $"{Match} -> {Route}";
        }

    }

    public static class CandidateExtensions
    {
        public static IEnumerable<Route> Matching(this IEnumerable<Candidate> candidates, params RouteMatch[] matches)
        {
            return candidates.Where(c => matches.Contains(c.Match)).Select(c => c.Route);
        } 

        public static int Count(this IEnumerable<Candidate> candidates, RouteMatch match)
        {
            return candidates.Count(c => c.Match == match);
        }
    }

}