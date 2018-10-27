using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shell.Routing
{
    public class Route
    {
        public List<Node> Nodes;
        public Help Help; 
        public MethodInfo Method;


        public Route(IEnumerable<Node> nodes, MethodInfo method, Help help)
        {
            this.Nodes = nodes.ToList();
            this.Method = method;
            this.Help = help;
        }

        public override string ToString()
        {
            string commands = string.Join(" ", Nodes);
            var parameters = Method.ParametersDescription();
            return $"{commands} {parameters}";

        }
    }

}