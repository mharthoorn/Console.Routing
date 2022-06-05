using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting;


public class RouterBuilder
{
    public RouteDiscoverer Discovery = new();
    public List<IBinder> Binders = new();
    public IServiceCollection Services = new ServiceCollection();
    
    Action<Router, Exception> exceptionHandler;
    private bool WithDocumentation { get; set; } = true;

    public RouterBuilder AddXmlDocumentation()
    {
        WithDocumentation = true;
        return this;
    }

    private Documentation BuildDocumentation()
    {
        if (WithDocumentation)
        {
            var docs =
               new DocumentationBuilder()
               .Add(Discovery.Assemblies)
               .Build();

            return docs;
        }
        else return new Documentation();
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
        this.Binders.Add(binder);
        return this;
    }

    /// <summary>
    /// You can add the default binders yourself if you want to append more.
    /// If you don't configure any binders at all, the defaults will be used regardless.
    /// </summary>

    public Router Build()
    {
        var documentation = BuildDocumentation();

        var globals = Discovery.DiscoverGlobals();
        var routes = Discovery.DiscoverRoutes().ToList();
        
        var binder = CreateBinder();
        var parser = new ArgumentParser();
        var writer = new RoutingWriter(documentation);

        Services.AddSingleton(writer);
        Services.AddSingleton(routes);
        Services.AddSingleton(binder);

        var provider = Services.BuildServiceProvider();

        return new Router(routes, binder, parser, writer, provider, globals, exceptionHandler);
    }

    private Binder CreateBinder()
    {
        if (Binders.Count == 0) Binders.AddDefaultBinders();
        return new Binder(Binders);
    }

}