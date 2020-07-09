using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public class RouteBuilder
    {
        private List<Route> Routes { get; }
        private List<Type> Globals { get; }
        
        public RouteBuilder()
        {
             Routes = new List<Route>();
             Globals = new List<Type>();
        }

        public RouteBuilder AddAssemblyOf<T>()
        {
            return Add(typeof(T).Assembly);
        }

        public RouteBuilder Add(Assembly assembly)
        {
            DiscoverModules(assembly);
            DiscoverGlobals(assembly);
            return this;
        }

        public void Add(Route route)
        {
            Routes.Add(route);
        }

        public Router Build()
        {
            return new Router(Routes, Globals);
        }

        private void DiscoverModules(Assembly assembly)
        {
            List<Node> trail = new List<Node>();
            var types = assembly.GetAttributeTypes<Module>().ToList();
            DiscoverModules(null, types, trail);
        }

        private void DiscoverModules(Module module, IEnumerable<Type> types, in List<Node> trail)
        {
            foreach (var type in types) DiscoverModule(module, type, trail);
        }

        private void DiscoverModule(Module module, Type type, in List<Node> trail)
        {
            if (module is null) module = type.GetCustomAttribute<Module>();
            var command = type.GetCustomAttribute<Command>();
            var t = trail.Retail(command);
            DiscoverNestedModules(module, type, t);
            DiscoverCommands(module, type, t);
        }

        private void DiscoverNestedModules(Module module, Type type, in List<Node> trail)
        {
            var nestedTypes = type.GetNestedTypes().Where(t => t.HasAttribute<Command>());
            DiscoverModules(module, nestedTypes, trail);
        }

        private void DiscoverCommands(Module module, Type type, in List<Node> trail)
        {
            var methods = type.GetAttributeMethods<Command>();
            foreach (var method in methods) DiscoverCommand(module, method, trail);
        }

        private void DiscoverGlobals(Assembly assembly)
        {
            var types = assembly.GetTypes().Where(t => t.HasAttribute<Global>());
            foreach (var type in types)
            {
                if (type is object)
                {
                    if (!type.IsStatic()) throw new ArgumentException("A global settings class must be static");
                    Globals.Add(type);
                }
            }
        }

        private void DiscoverCommand(Module module, MethodInfo method, in List<Node> trail)
        {
            var isdefault = method.HasAttribute<Default>();
            var help = method.GetCustomAttribute<Help>();
            var hidden = method.GetCustomAttribute<Hidden>();
            var t = isdefault ? trail : trail.Retail(method);
            var route = new Route(module, t, method, help, hidden, isdefault);
            Add(route);

        }

    }



}