using System.Reflection;

namespace ConsoleRouting
{
    public static class RouteBuilderExtensions
    {
        public static RouterBuilder Add(this RouterBuilder builder, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies) builder.AddAssembly(assembly);
            return builder;
        }
    }



}