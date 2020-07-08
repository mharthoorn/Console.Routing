using System.Reflection;

namespace ConsoleRouting
{
    public static class RouteBuilderExtensions
    {
        public static RouteBuilder Add(this RouteBuilder builder, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies) builder.Add(assembly);
            return builder;
        }
    }



}