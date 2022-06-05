using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting;


public class RouteDiscoverer
{
    List<Route> routes = new();
    List<Node> start = new();
    public HashSet<Assembly> Assemblies = new();
    HashSet<Type> modules = new();

    public RouteDiscoverer AddModules(Assembly assembly)
    {
        Assemblies.Add(assembly);
        var types = assembly.GetTypes().Where(t => t.HasAttribute<Module>());

        foreach (var type in types) modules.Add(type);

        return this;
    }

    public RouteDiscoverer AddModule<T>()
    {
        Assemblies.Add(typeof(T).Assembly);
        modules.Add(typeof(T));
        return this;
    }

    public IEnumerable<Type> DiscoverGlobals()
    {
        return Assemblies.SelectMany(DiscoverGlobals);
    }

    public IEnumerable<Route> DiscoverRoutes()
    {
        // we collect globally, to avoid passing down.
        routes.Clear();
        DiscoverRoutes(modules);
        return routes;
    }

    private IEnumerable<Type> DiscoverGlobals(Assembly assembly)
    {
        var types = assembly.GetTypes().Where(t => t.HasAttribute<Global>());
        foreach (var type in types)
        {
            if (type is object)
            {
                if (!type.IsStatic()) throw new ArgumentException("A global settings class must be static");
                yield return type;
            }
        }
    }

    private void DiscoverRoutes(IEnumerable<Type> types)
    {
        foreach (var type in types)
            DiscoverRoutes(type);
    }

    public void DiscoverRoutes(Type type)
    {
        var module = type.GetCustomAttribute<Module>();
        DiscoverRoutes(module, type, start);
    }

    private void DiscoverRoutes(Module module, IEnumerable<Type> types, in List<Node> trail)
    {
        foreach (var type in types) DiscoverRoutes(module, type, trail);
    }

    private void DiscoverNestedModuleRoutes(Module module, Type type, List<Node> trail)
    {
        var nestedTypes = type.GetNestedTypes().Where(t => t.HasAttribute<Command>());
        DiscoverRoutes(module, nestedTypes, trail);
    }

    private void DiscoverRoutes(Module module, Type type, List<Node> trail)
    {
        var node = type.TryCreateRoutingNode();
        var clone = trail.CloneAndAppend(node);

        DiscoverNestedModuleRoutes(module, type, clone);

        DiscoverCommands(module, type, clone);
    }

    private void DiscoverCommands(Module module, Type type, List<Node> trail)
    {
        var methods = type.GetAttributeMethods<Command>();
        foreach (var method in methods) DiscoverCommand(module, method, trail);
    }

    private void DiscoverCommand(Module module, MethodInfo method, in List<Node> trail)
    {
        var isdefault = method.HasAttribute<Default>();
        var help = method.GetCustomAttribute<Help>();
        var hidden = method.HasAttribute<Hidden>();
        var capture = method.GetCustomAttribute<Capture>();
        var node = method.TryCreateRoutingNode();
        var clone = isdefault ? trail : trail.CloneAndAppend(node);
        var route = new Route(module, clone, method, help, hidden, capture, isdefault);
        routes.Add(route);
    }
  
}