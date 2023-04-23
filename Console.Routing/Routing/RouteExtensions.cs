using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting;

public static class RouteExtensions
{
    //public static IEnumerable<Route> NonDefault(this IEnumerable<Route> routes)
    //{
    //    return routes.Where(r => !r.IsDefault);
    //}

    public static IEnumerable<Parameter> AsRoutingParameters(this IEnumerable<ParameterInfo> parameters)
    {
        foreach (var parameterInfo in parameters)
            yield return parameterInfo.AsRoutingParameter();
        
    }
     
    public static Parameter AsRoutingParameter(this ParameterInfo info)
    {
        return new Parameter
        {
            Name = info.Name,
            Type = info.ParameterType,
            AltName = info.GetCustomAttribute<Alt>()?.Name,
            TakeAll = info.HasAttribute<All>(),
            Optional = info.IsOptionalParameter(),
            HasDefaultValue = info.HasDefaultValue,
            DefaultValue = info.DefaultValue
        };
    }

    
    public static Parameter AsRoutingParameter(this MemberInfo info)
    {
        return new Parameter
        {
            Name = info.Name.ToLower(),
            Type = info.GetMemberType(),
            AltName = info.GetCustomAttribute<Alt>()?.Name,
            Optional = true // for now.
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
        var parameters = AsRoutingParameters(paraminfo);
        return parameters;
    }

    public static Parameters GetRoutingParameters(this MethodInfo method)
    {
        var parameters = method.GetParameters().AsRoutingParameters();
        return new Parameters(parameters);
    }

    public static string AsText(this Route route)
    {
        var parameters = route.GetRoutingParameters();
        return string.Join(" ", parameters.Select(p => AsText(p)));
    }

    public static string ParametersAsText(this MethodInfo method)
    {
        var parameters = method.GetParameters().AsRoutingParameters();
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
        else if (type.IsGenericFlag())
        {
            rep = $"--{name} <value>";
        }
        else if (type == typeof(Arguments))
        {
            rep = $"<{name}>...";
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



    public static Arguments Parse(this Router router, string text)
    {
        return router.Parser.Parse(text);
    }

    public static Arguments Parse(this Router router, params string[] args)
    {
        return router.Parser.Parse(args);
    }

}