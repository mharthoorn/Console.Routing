using System;

namespace ConsoleRouting;

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

    [Command("help"), Help("Provides this help list or detailed help about a command")]
    public void Help(Arguments args = null)
    {
        if (args is null || args.Count == 0)
        {
            router.Writer.WriteShortRoutes(router.Routes);
        }
        else
        {
            var result = router.Bind(args);
            router.Writer.WriteRouteHelp(result);
        }
    }

    [Command, Capture("?", "--help", "-?", "-h"), Hidden]
    public void CaptureHelp(Arguments args = null)
    {
        if (args is null || args.Count == 0)
        {
            router.Writer.WriteShortRoutes(router.Routes);
        }
        else
        {
            var result = router.Bind(args);
            router.Writer.WriteRouteHelp(result);
        }
    }

}