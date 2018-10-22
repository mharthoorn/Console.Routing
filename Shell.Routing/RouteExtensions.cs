using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shell.Routing
{
    public static class RouteExtensions
    {
        public static IEnumerable<Route> FindGroup(this IEnumerable<Route> routes, string group)
        {
            string g = group.ToLower();
            return routes.Where(r => string.Compare(r.Section.Name, g, ignoreCase: true) == 0);
        }

        public static IEnumerable<Route> FindMethod(this IEnumerable<Route> routes, string methodName)
        {
            var method = methodName.ToLower();
            return routes.Where(route => route.MatchName(method));
        }

        public static IEnumerable<RoutingParameter> GetRoutingParameters(this IEnumerable<ParameterInfo> parameters)
        {
            foreach (var parameter in parameters)
            {
                yield return new RoutingParameter
                {
                    Name = parameter.Name.ToLower(),
                    Type = parameter.ParameterType,
                    Optional = (parameter.GetCustomAttribute<Optional>() != null)
                };
            }
        }

        public static IEnumerable<RoutingParameter> GetRoutingParameters(this Route route)
        {
            var parameters = route.Method.GetParameters();
            return GetRoutingParameters(parameters);
        }

        public static string ParametersDescription(this Route route)
        {
            var paraminfo = route.Method.GetParameters();
            var parameters = GetRoutingParameters(paraminfo);
            return string.Join(" ", parameters.Select(p => p.AsString));
            
        }

        public static bool TryBind(this Route route, Arguments arguments, out object[] values)
        {
            // group command value value -dev -all -special:souce -route C:\temp
            var count  = route.Method.GetParameters().Count();
            values = new object[count];
            int i = 0;
            int used = 0; // arguments used;

            foreach (var param in route.GetRoutingParameters())
            {
                if (param.Type == typeof(string))
                {
                    if (arguments.TryGetLiteral(i, out string value))
                    {
                        values[i++] = value;
                        used++;
                    }
                    else if (param.Optional)
                    {
                        values[i++] = null;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (param.Type == typeof(Flag))
                {
                    var hasoption = arguments.HasFlag(param.Name);
                    values[i++] = new Option(hasoption);
                    if (hasoption) used++;
                }
                else if (param.Type == typeof(Assignment))
                {
                    if (arguments.TryGetAssignment(param.Name, out Assignment option))
                    {
                        if (!option.Provided) return false;
                        // invalid option

                        values[i++] = option;
                        used ++;
                    }
                    else
                    {
                        values[i++] = OptionValue.Unset;
                    }
                    
                }
                else if (param.Type == typeof(Arguments))
                {
                    values[i++] = arguments;
                    return true;
                }
                else
                {
                    // this method has a signature with wrong types.
                    return false;
                }
            }
            return (arguments.Count == used);
            
        }
    }

}