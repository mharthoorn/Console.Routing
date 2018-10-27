using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shell.Routing
{
    public class RouteBuilder
    {
        public List<Route> Endpoints { get;  }

        public RouteBuilder()
        {
            Endpoints = new List<Route>();
        }

        public void DiscoverAssembly(Assembly assembly)
        {
            List<Node> trail = new List<Node>();
            var types = assembly.GetAttributeTypes<Module>().ToList();
            DiscoverTypes(types, trail);
        }

        public void DiscoverTypes(IEnumerable<Type> types, in List<Node> trail)
        {
            foreach (var type in types) DiscoverType(type, trail);
        }

        public void DiscoverType(Type type, in List<Node> trail)
        {
            var command = type.GetCustomAttribute<Command>();
            var t = trail.Retail(command);
            DiscoverTypesOf(type, t);
            DiscoverCommands(type, t);
        }

        public void DiscoverTypesOf(Type type, in List<Node> trail)
        {
            var nestedTypes = type.GetNestedTypes().Where(t => t.HasAttribute<Module>());
            DiscoverTypes(nestedTypes, trail);
        }

        public void DiscoverCommands(Type type, in List<Node> trail)
        {
            var methods = type.GetAttributeMethods<Command>();
            foreach (var method in methods) DiscoverCommand(method, trail);
        }

        public void DiscoverCommand(MethodInfo method, in List<Node> trail)
        {
            var t = trail.Retail(method);
            var help = method.GetCustomAttribute<Help>();
            var endpoint = new Route(t, method, help);
            Register(endpoint);

        }

        public void Register(Route endpoint)
        {
            Endpoints.Add(endpoint);
        }
    }
    
}