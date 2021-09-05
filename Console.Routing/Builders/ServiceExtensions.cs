using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleRouting
{
    public static class ServiceExtensions
    {
        public static RouterBuilder AddService<TService>(this RouterBuilder builder)
           where TService : class
        {
            builder.Services.AddSingleton<TService>();
            return builder;
        }
        public static RouterBuilder AddService<TService, TImplementation>(this RouterBuilder builder, TImplementation instance)
         where TService : class
         where TImplementation : class, TService
        {
            builder.Services.AddSingleton<TService>(instance);
            return builder;
        }


        public static RouterBuilder AddService<TService>(this RouterBuilder builder, Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            builder.Services.AddScoped<TService>(implementationFactory);
            return builder;
        }

    }


}