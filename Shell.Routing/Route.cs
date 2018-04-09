using System;
using System.Linq;
using System.Reflection;

namespace Shell.Routing
{
    public class Route
    {
        public Section Section { get; }
        public Command Command { get; }
        public Type Type { get; }
        public MethodInfo Method { get; }

        public Route(Section section, Command command, Type type, MethodInfo method)
        {
            Section = section;
            Command = command;
            Type = type;
            Method = method;
        }


        public override string ToString()
        {
            var pars = this.ParametersDescription();
            return $"{Section.Name.ToLower()} {Method.Name.ToLower()} {pars}";
        }
    }

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