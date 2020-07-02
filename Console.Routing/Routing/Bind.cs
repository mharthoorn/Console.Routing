using System;
using System.Linq;

namespace ConsoleRouting
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

        private static string ArgumentString(object arg)
        {
            switch (arg)
            {
                case IArgument a: return a.Value;
                case string s: return s;
                default: return arg.ToString();
            }
        }

        public override string ToString()
        {
            var parameters = Route.Method.GetRoutingParameters();
            var assignments = parameters.Zip(Arguments, (p, a) => $"{p}={ArgumentString(a)}");
            var paramlist = string.Join(",", assignments);

            return $"{Route.Method.Name}({paramlist})";
        }

    }

}