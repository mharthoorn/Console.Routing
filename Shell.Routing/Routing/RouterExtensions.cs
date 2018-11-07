namespace Shell.Routing
{
    public static class RouterExtensions
    {
        public static RoutingResult Handle(this Router router, Arguments arguments)
        {
            RoutingResult result = router.Bind(arguments);

            if (result.Ok)
                Invoker.Run(result.Bind);

            return result;
        }
    }

}


