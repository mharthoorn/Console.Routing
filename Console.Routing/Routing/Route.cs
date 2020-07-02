using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public class Route
    {
        public Module Module;
        public bool Hidden;
        public bool Default;
        public List<Node> Nodes;
        public Help Help; 
        public MethodInfo Method;

        public Route(Module module, IEnumerable<Node> nodes, MethodInfo method, Help help, Hidden hidden, bool isdefault)
        {
            this.Module = module;
            this.Nodes = nodes.ToList();
            this.Method = method;
            this.Help = help;
            this.Hidden = !(hidden is null);
            this.Default = isdefault;
        }

        public string Description => Help?.Description;

        public override string ToString()
        {
            string commands = string.Join(" ", Nodes);
            var parameters = Method.Representation();
            return $"{commands} {parameters}";

        }
    }

}