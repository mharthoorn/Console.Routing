using System;

namespace ConsoleRouting
{
    public static class DefaultExceptionHandler
    {
        public static void Handle(Router router, Exception e)
        {
            RoutingWriter.WriteException(e, stacktrace: router.DebugMode); //todo: re-enable through parameter later.
            Environment.Exit(-1);
        }
    }

}


