using ConsoleRouting;

namespace ConsoleAppTemplate
{
    [Module]
    internal class CaptureCommands
    {
        private Router router;

        public CaptureCommands(Router router)
        {
            this.router = router;
        }

        [Command, Capture("--info", "-i", "info")]
        public void HandleConfig(Arguments args)
        {
            
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
