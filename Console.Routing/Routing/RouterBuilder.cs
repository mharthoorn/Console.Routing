using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public class RouterBuilder
    {
        internal bool Documentation { get; set; } = false;
        private List<Assembly> assemblies = new();
        private List<Type> modules = new();
        private List<Route> routes = new();
        private List<IBinder> binders = new();
        private List<Type> globals = new();
        Action<Router, Exception> exceptionHandler;

        public RouterBuilder Add(Assembly assembly)
        {
            assemblies.Add(assembly);
          
            return this;
        }

        public RouterBuilder AddModule<T>()
        {
            modules.Add(typeof(T));
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

        public RouterBuilder AddBinder(IBinder binder)
        {
            this.binders.Add(binder);
            return this;
        }

        /// <summary>
        /// You can add the default binders yourself if you want to append more.
        /// If you don't configure any binders at all, the defaults will be used regardless.
        /// </summary>
        public RouterBuilder AddDefaultBinders()
        {
            binders.Add(new StringBinder());
            binders.Add(new EnumBinder());
            binders.Add(new IntBinder());
            binders.Add(new AssignmentBinder());
            binders.Add(new FlagValueBinder());
            binders.Add(new FlagBinder());
            binders.Add(new BoolBinder());
            binders.Add(new ArgumentsBinder());

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



        private void AttachDocumentation()
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

        private Binder CreateBinder()
        {
            if (binders.Count == 0) AddDefaultBinders();
            return new Binder(binders);
        }      

        private void DiscoverModules(Assembly assembly)
        {
            List<Node> trail = new List<Node>();
            var types = assembly.GetAttributeTypes<Module>().ToList();
            DiscoverModules(types, trail);
        }

        private void DiscoverModules(IEnumerable<Type> types, in List<Node> trail)
        {
            foreach (var type in types) DiscoverModule(type, trail);
        }

        private void DiscoverModule(Type type, in List<Node> trail)
        {
            var module = type.GetCustomAttribute<Module>();
          
            var node = type.TryCreateRoutingNode();
            var clone = trail.CloneAndAppend(node);

            DiscoverNestedModules(type, clone);
            DiscoverCommands(module, type, clone);
        }

        private void DiscoverNestedModules(Type type, in List<Node> trail)
        {
            var nestedTypes = type.GetNestedTypes().Where(t => t.HasAttribute<Command>());
            DiscoverModules(nestedTypes, trail);
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

    public static class RouterBuilderExtensions
    {
        public static RouterBuilder AddDefaultHelp(this RouterBuilder builder)
        {
            builder.AddModule<HelpModule>();
            return builder;
        }

        public static RouterBuilder AddAssemblyOf<T>(this RouterBuilder builder)
        {
            return builder.Add(typeof(T).Assembly);
        }

        public static RouterBuilder AddBinders(this RouterBuilder builder, IEnumerable<IBinder> binders)
        {
            foreach(var binder in binders) builder.AddBinder(binder);
            return builder;
        }

    }



}