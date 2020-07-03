using System;

namespace ConsoleRouting
{
    public static class RouterExtensions
    {

        public static void Handle(this Router router, string[] args)
        {
            var arguments = new Arguments(args);
            try
            {
                var result = router.Handle(arguments);
                if (!result.Ok) RoutingPrinter.Write(result);
            }
            catch (Exception e)
            {
                RoutingPrinter.Write(e, stacktrace: false); //todo: re-enable through parameter later.
            }

        }
    }

}


