﻿using System;
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
  
        public static IEnumerable<Parameter> GetRoutingParameters(this IEnumerable<ParameterInfo> parameters)
        {
            foreach (var parameterInfo in parameters)
                yield return parameterInfo.ToParameter();
            
        }
         
        public static Parameter ToParameter(this ParameterInfo info)
        {
            return new Parameter
            {
                Name = info.Name.ToLower(),
                Type = info.ParameterType,
                AltName = info.GetCustomAttribute<Alt>()?.Name,
                Optional = info.IsOptionalParameter(),
            };
        }

        public static bool IsOptionalParameter(this ParameterInfo parameter)
        {
            bool hasAttr = parameter.HasAttribute<Optional>();
            bool maybenull = parameter.IsNullable();
            bool hasdefault = parameter.IsOptional;

            return hasAttr ||  maybenull || hasdefault;
        }

        public static IEnumerable<Parameter> GetRoutingParameters(this Route route)
        {
            var paraminfo = route.Method.GetParameters();
            var parameters = GetRoutingParameters(paraminfo);
            return parameters;
        }

        public static Parameters GetRoutingParameters(this MethodInfo method)
        {
            var parameters = method.GetParameters();
            return new Parameters(GetRoutingParameters(parameters));
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

        public static Arguments Parse(this Router router, string text)
        {
            return router.Parser.Parse(text);
        }

        public static Arguments Parse(this Router router, params string[] args)
        {
            return router.Parser.Parse(args);
        }

    }
}