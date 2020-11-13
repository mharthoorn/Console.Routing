using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public static class RouteExtensions
    {
        public static IEnumerable<Route> NonDefault(this IEnumerable<Route> routes)
        {
            return routes.Where(r => !r.Default);
        }
        //public static IEnumerable<OldRoute> FindGroup(this IEnumerable<OldRoute> routes, string group)
        //{
        //    string g = group.ToLower();
        //    return routes.Where(r => string.Compare(r.Module.Title, g, ignoreCase: true) == 0);
        //}

        //public static IEnumerable<OldRoute> FindMethod(this IEnumerable<OldRoute> routes, string methodName)
        //{
        //    var method = methodName.ToLower();
        //    return routes.Where(route => route.MatchName(method));
        //}

        public static IEnumerable<Parameter> GetRoutingParameters(this IEnumerable<ParameterInfo> parameters)
        {
            foreach (var parameter in parameters)
            {
                
                yield return new Parameter
                {
                    Name = parameter.Name.ToLower(),
                    Type = parameter.ParameterType,
                    AltName = parameter.GetCustomAttribute<Alt>()?.Name,
                    Optional = parameter.HasAttribute<Optional>(),
                };
            }
        }

        public static IEnumerable<Parameter> GetRoutingParameters(this Route route)
        {
            var paraminfo = route.Method.GetParameters();
            var parameters = GetRoutingParameters(paraminfo);
            return parameters;
        }

        public static IEnumerable<Parameter> GetRoutingParameters(this MethodInfo method)
        {
            var parameters = method.GetParameters();
            return GetRoutingParameters(parameters);
        }

        public static string AsText(this Route route)
        {
            var parameters = route.GetRoutingParameters();
            return string.Join(" ", parameters.Select(p => AsText(p)));
        }

        public static string ParametersAsText(this MethodInfo method)
        {
            var paraminfo = method.GetParameters();
            var parameters = GetRoutingParameters(paraminfo);
            return string.Join(" ", parameters.Select(p => AsText(p)));
        }

        public static string AsText(this Parameter parameter)
        {
            Type type = parameter.Type;
            string name = parameter.Name;
            
            string rep;

            if (type == typeof(Flag) || type == typeof(bool))
            {
                rep = $"--{name}";
            }
            else if (type == typeof(Assignment))
            {
                rep = $"{name}=<value>";
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Flag<>))
            {
                rep = $"--{name} <value>";
            }
            else if (type == typeof(Arguments))
            {
                rep = $"({name}...)";
            }
            else if (type == typeof(string)) 
            {
                rep = $"<{name}>"; 
            }
            else  
            {
                rep = $"{name}";
            }
            if (parameter.Optional) rep = $"({rep})";

            return rep;
        }

        public static string GetMethodDoc(this Route route)
        {
            var doc = route.Documentation;
            if (doc is null) return null;
            return doc.Summary;
        }

        

        public static string GetParamDoc(this Route route, string name)
        {
            var doc = route.Documentation;
            if (doc is null) return null;
            return doc.Params.TryGetValue(name, out var value) ? value : null;
        }





    }

}