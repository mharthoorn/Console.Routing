namespace ConsoleRouting;


public static class RoutingWriterExtensions
{
    public static void WriteRoutes(this RoutingWriter writer, RouteCollection routes) => writer.WriteRoutes(routes);
}