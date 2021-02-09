using System;

namespace ConsoleRouting
{
    public static class RouterExtensions
    {

        public static void Handle(this Router router, string[] args)
        {
            var arguments = new Arguments(args);
            var result = router.Handle(arguments);
            if (!result.Ok) RoutingWriter.Write(result);
        }

        
    }

    public static class DefaultExceptionHandler
    {
        public static void Handle(Router router, Exception e)
        {
            RoutingWriter.Write(e, stacktrace: router.DebugMode); //todo: re-enable through parameter later.
            Environment.Exit(-1);
        }
    }

}


