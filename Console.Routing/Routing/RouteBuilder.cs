using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public class RouteBuilder
    {
        public List<Route> Routes { get;  }

        public RouteBuilder()
        {
            Routes = new List<Route>();
        }

        public void DiscoverAssembly(Assembly assembly)
        {
            List<Node> trail = new List<Node>();
            var types = assembly.GetAttributeTypes<Module>().ToList();
            DiscoverTypes(null, types, trail);
        }

        public void DiscoverTypes(Module module, IEnumerable<Type> types, in List<Node> trail)
        {
            foreach (var type in types) DiscoverType(module, type, trail);
        }

        public void DiscoverType(Module module, Type type, in List<Node> trail)
        {
            if (module is null) module = type.GetCustomAttribute<Module>();
            var command = type.GetCustomAttribute<Command>();
            var t = trail.Retail(command);
            DiscoverTypesOf(module, type, t);
            DiscoverCommands(module, type, t);
        }

        public void DiscoverTypesOf(Module module, Type type, in List<Node> trail)
        {
            var nestedTypes = type.GetNestedTypes().Where(t => t.HasAttribute<Command>());
            DiscoverTypes(module, nestedTypes, trail);
        }

        public void DiscoverCommands(Module module, Type type, in List<Node> trail)
        {
            var methods = type.GetAttributeMethods<Command>();
            foreach (var method in methods) DiscoverCommand(module, method, trail);
        }

        public void DiscoverCommand(Module module, MethodInfo method, in List<Node> trail)
        {
            var isdefault = method.HasAttribute<Default>();
            var help = method.GetCustomAttribute<Help>();
            var hidden = method.GetCustomAttribute<Hidden>();
            var t = isdefault ? trail : trail.Retail(method);
            var route = new Route(module, t, method, help, hidden, isdefault);
            Register(route);

        }

        public void Register(Route endpoint)
        {
            Routes.Add(endpoint);
        }
    }
    
}