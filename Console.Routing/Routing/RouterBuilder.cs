using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public class RouterBuilder
    {
        internal bool DebugMode { get; set; }
        internal bool Documentation { get; set;
        }
        private List<Assembly> assemblies = new();
        private List<Route> routes = new();
        private List<IBinder> binders = new();
        private List<Type> globals = new();
        Action<Router, Exception> exceptionHandler;

        public RouterBuilder AddAssemblyOf<T>()
        {
            return Add(typeof(T).Assembly);
        }

        public RouterBuilder Add(Assembly assembly)
        {
            assemblies.Add(assembly);
          
            return this;
        }

        public RouterBuilder AddXmlDocumentation()
        {
            Documentation = true;
            return this;
        }

        public RouterBuilder AddExceptionHandler(Action<Router, Exception> handler)
        {
            this.exceptionHandler = handler;
            return this;
        }
         
        public RouterBuilder NoExceptionHandling()
        {
            this.exceptionHandler = null;
            return this;
        }

        public void AttachDocumentation()
        {
            var docs =
                new AssemblyDocumentationBuilder()
                .Add(assemblies)
                .Build();

            foreach (var route in routes)
            {
                route.Documentation = docs.Get(route.Method);
            }
        }

        public RouterBuilder Debug()
        {
            DebugMode = true;
            return this;
        }
        public void Add(Route route)
        {
            routes.Add(route);
        }

        public Router Build()
        {
            foreach (var assembly in assemblies) 
            { 
                DiscoverGlobals(assembly);
                DiscoverModules(assembly);
            }
            if (Documentation) AttachDocumentation();
          
            var binder = CreateBinder();
            var parser = new ArgumentParser();
            var writer = new RoutingWriter();

            return new Router(routes, binder, parser, writer, globals, exceptionHandler);
        }

        private static List<IBinder> DEFAULTBINDERS = new()
        {
            new StringBinder(),
            new EnumBinder(),
            new IntBinder(),
            new AssignmentBinder(),
            new FlagValueBinder(),
            new FlagBinder(),
            new BoolBinder(),
        };

        private Binder CreateBinder()
        {
            if (binders.Count > 0) return new Binder(binders);
            else return new Binder(DEFAULTBINDERS);
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
            
            var node = type.TryCreateRoutingNode();
            var clone = trail.CloneAndAppend(node);

            DiscoverNestedModules(module, type, clone);
            DiscoverCommands(module, type, clone);
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
                    globals.Add(type);
                }
            }
        }

        private void DiscoverCommand(Module module, MethodInfo method, in List<Node> trail)
        {
            var isdefault = method.HasAttribute<Default>();
            var help = method.GetCustomAttribute<Help>();
            var hidden = method.GetCustomAttribute<Hidden>();
            var node = method.TryCreateRoutingNode();
            var clone = isdefault ? trail : trail.CloneAndAppend(node);
            var route = new Route(module, clone, method, help, hidden, isdefault);
            Add(route);

        }

        
    }



}