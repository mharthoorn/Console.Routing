using System;
using System.Reflection;

namespace Harthoorn.Shell.Routing
{
    public class Route
    {
        public Section Group { get; }
        public Command Command { get; }
        public Type Type { get; }
        public MethodInfo Method { get; }

        public Route(Section group, Command command, Type type, MethodInfo method)
        {
            Group = group;
            Command = command;
            Type = type;
            Method = method;
        }

        public override string ToString()
        {
            return $"{Group.Name.ToLower()} {Method.Name.ToLower()}";
        }
    }

}