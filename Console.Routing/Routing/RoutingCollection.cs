using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting;

public class RouteCollection : List<Route>
{
    public RouteCollection(IEnumerable<Route> routes)
    {
        foreach (var route in routes) this.Add(route);
    }
}

public static class RouteCollectionExtensions
{
    public static IEnumerable<Route> ThatAre(this IEnumerable<Route> routes, RouteFlag flags)
       => routes.Where(r => r.Is(flags));

    public static IEnumerable<Route> ThatAreNot(this IEnumerable<Route> routes, RouteFlag flags)
    => routes.Where(r => !r.Is(flags));
}