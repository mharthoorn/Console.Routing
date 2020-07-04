using System.Linq;

namespace ConsoleRouting
{
    public class Bind
    {
        public Route Route;
        public object[] Parameters;
         
        public Bind(Route endpoint, object[] parameters)
        {
            this.Route = endpoint;
            this.Parameters = parameters;
        }

        private static string ParameterString(object arg)
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
            var assignments = parameters.Zip(Parameters, (p, a) => $"{p}={ParameterString(a)}");
            var paramlist = string.Join(",", assignments);

            return $"{Route.Method.Name}({paramlist})";
        }

    }

}