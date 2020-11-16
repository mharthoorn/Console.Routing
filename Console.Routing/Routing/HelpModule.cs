namespace ConsoleRouting
{
    /// <summary>
    /// This is the default help module, that is in included in your routing system when you use
    /// RouteBuilder.AddHelp()
    /// </summary>
    [Module("Help")]
    public class HelpModule
    {
        private Router router;

        public HelpModule(Router router)
        {
            this.router = router;
        }

        [Command("help", "?", "--help", "-?", "-h"), Help("Provides this help text")]
        public void Help(Arguments commands = null)
        {
            if (commands is null || commands.Count == 1)
            {
                RoutingWriter.WriteRoutes(router);
            }
            else
            {
                commands.RemoveAt(0);
                RoutingWriter.WriteRouteHelp(Routing.Router, commands);
            }
        }

    }



}