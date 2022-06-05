using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting;


public class RoutingResult
{
    public RoutingStatus Status;
    public Arguments Arguments;
    public List<Candidate> Candidates; //where the commands match, but not necessarily the parameters
    public List<Bind> Binds;

    public RoutingResult(Arguments arguments, RoutingStatus status, List<Bind> binds, List<Candidate> candidates)
    {
        this.Arguments = arguments;
        this.Candidates = candidates;
        this.Binds = binds;
        this.Status = status;
    }

    public bool Ok => Status == RoutingStatus.Ok;

    public Bind Bind => Binds.SingleOrDefault();

    public Route Route => Bind?.Route;

    public int BindCount => Binds.Count;

    public IEnumerable<Route> Routes => Binds.Select(b => b.Route);
    
    public override string ToString()
    {
        string tally = $"Binds: {BindCount}, Candidates: {Candidates?.Count ?? 0})";
        if (Ok)
        {
            return $"Ok. {tally}";
        }
        else
        {
            return $"Failed. {tally}";
        }
        
    }
}





