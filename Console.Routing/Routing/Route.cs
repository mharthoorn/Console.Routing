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
        public bool HasCapture;
        public Capture Capture;
        public List<Node> Nodes;
        public Help Help; 
        public MethodInfo Method;

        public Route(Module module, IEnumerable<Node> nodes, MethodInfo method, Help help, bool hidden, Capture capture, bool isdefault)
        {
            this.Module = module;
            this.Nodes = nodes.ToList();
            this.Method = method;
            this.Help = help;
            this.Capture = capture;
            this.HasCapture = capture is not null;
            this.Hidden = hidden | this.HasCapture;
            this.Default = isdefault;
        }

        public string Description => Help?.Description;

        public override string ToString()
        {
            string commands = string.Join(" ", Nodes); 
            var parameters = Method.ParametersAsText();

            string s = commands;
            if (parameters.Length > 0) s += " " + parameters;
            return s;
        }
    }

}