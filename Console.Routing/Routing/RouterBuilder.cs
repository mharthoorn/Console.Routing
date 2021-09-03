using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public class RouterBuilder
    {
        internal bool Documentation { get; set; } = false;
       
        private RouteDiscoverer discovery = new();
        private List<IBinder> binders = new();
        Action<Router, Exception> exceptionHandler;
        IServiceCollection services = new ServiceCollection();

        public RouterBuilder AddAssembly(Assembly assembly)
        {
            discovery.AddModules(assembly);
            return this;
        }

        [Obsolete("Use AddAssembly instead")]
        public RouterBuilder Add(Assembly assembly)
        {
            discovery.AddModules(assembly);
            return this;
        }

        public RouterBuilder AddService<TService>() 
            where TService : class
        {
            services.AddSingleton<TService>();
            return this;
        }

        public RouterBuilder AddService<TService, TImplementation>(TImplementation instance) 
            where TService : class 
            where TImplementation : class, TService
        {
            services.AddSingleton<TService>(instance);
            return this;
        }

        public RouterBuilder AddService<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            services.AddScoped<TService>(implementationFactory);
            return this;
        }

        public RouterBuilder AddModule<T>()
        {
            discovery.AddModule<T>(); 
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

        public Router Build()
        {
            var globals = discovery.DiscoverGlobals();
            var routes = discovery.DiscoverRoutes(Documentation).ToList();

            var binder = CreateBinder();
            var parser = new ArgumentParser();
            var writer = new RoutingWriter();

            services.AddSingleton(writer);
            services.AddSingleton(routes);
            services.AddSingleton(binder);

            var provider = services.BuildServiceProvider();

            return new Router(routes, binder, parser, writer, provider, globals, exceptionHandler);
        }
      

        private Binder CreateBinder()
        {
            if (binders.Count == 0) AddDefaultBinders();
            return new Binder(binders);
        }



      
    

      
    }



}