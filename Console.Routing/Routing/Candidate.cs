using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
{

    public enum CommandMatch
    {
        Default,
        Not,
        Partial,
        Full, // commands match

    }

    public class Candidate
    {
        public CommandMatch Match;
        public Route Route;

        public Candidate(CommandMatch match, Route route)
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
        public static IEnumerable<Route> Routes(this IEnumerable<Candidate> candidates, params CommandMatch[] matches)
        {
            return candidates.Where(c => matches.Contains(c.Match)).Select(c => c.Route);
        } 

        public static int Count(this IEnumerable<Candidate> candidates, CommandMatch match)
        {
            return candidates.Count(c => c.Match == match);
        }
    }

}