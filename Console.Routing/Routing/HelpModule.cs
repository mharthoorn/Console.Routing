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

        [Command("help", "?", "--help", "-?", "-h"), Help("Provides this help list or detailed help about a command")]
        public void Help(Arguments args = null)
        {
            args.RemoveAt(0); // remove the help command
            if (args is null || args.Count == 0)
            {
                router.Writer.WriteRoutes(router);
            }
            else
            {
                var result = router.Bind(args);
                router.Writer.WriteRouteHelp(result);
            }
        }


    }



}