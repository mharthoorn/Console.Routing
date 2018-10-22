namespace Shell.Routing
{
    public class Bind
    {
        public Route Route;
        public object[] Arguments;

        public Bind(Route route, object[] arguments)
        {
            this.Route = route;
            this.Arguments = arguments;
        }

    }

}