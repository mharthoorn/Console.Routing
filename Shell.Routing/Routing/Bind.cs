namespace Shell.Routing
{
    public class Bind
    {
        public Route Endpoint;
        public object[] Arguments;
         
        public Bind(Route endpoint, object[] arguments)
        {
            this.Endpoint = endpoint;
            this.Arguments = arguments;
        }

    }

}