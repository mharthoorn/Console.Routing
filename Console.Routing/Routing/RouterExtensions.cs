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

}


