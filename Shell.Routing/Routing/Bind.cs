namespace Shell.Routing
{
    public class Bind
    {
        public Route Route;
        public object[] Arguments;
         
        public Bind(Route endpoint, object[] arguments)
        {
            this.Route = endpoint;
            this.Arguments = arguments;
        }

    }

}