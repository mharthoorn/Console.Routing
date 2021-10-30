namespace ConsoleRouting
{
    public static class RoutingWriterExtensions
    {
        public static void WriteRoutes(this RoutingWriter writer, Router router) => writer.WriteRoutes(router?.Routes);
    }

}