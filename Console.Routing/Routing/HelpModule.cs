using System;

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

        [Command("help", "?"), Help("Provides this help text")]
        public void Help(Arguments args = null)
        {
            if (args is null || args.Count == 0)
            {
                RoutingWriter.WriteRoutes(router);
            }
            else
            {
                args.RemoveAt(0);
                RoutingWriter.WriteRouteDocumentation(Routing.Router, args);
            }
        }

        [Command("--help", "-?"), Hidden]
        public void FlagHelp()
        {
            // If you provide no parameters, you end up here.   
            Help();
        }

    }



}