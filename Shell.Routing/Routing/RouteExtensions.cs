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

        public static IEnumerable<Parameter> GetRoutingParameters(this IEnumerable<ParameterInfo> parameters)
        {
            foreach (var parameter in parameters)
            {
                yield return new Parameter
                {
                    Name = parameter.Name.ToLower(),
                    Type = parameter.ParameterType,
                    Optional = parameter.HasAttribute<Optional>(),
                };
            }
        }

        public static IEnumerable<Parameter> GetRoutingParameters(this Route route)
        {
            var parameters = route.Method.GetParameters();
            return GetRoutingParameters(parameters);
        }

        public static string ParametersDescription(this Route route)
        {
            var paraminfo = route.Method.GetParameters();
            var parameters = GetRoutingParameters(paraminfo);
            return string.Join(" ", parameters.Select(p => ParameterDescription(p)));
            
        }

        public static string ParameterDescription(Parameter parameter)
        {
            Type type = parameter.Type;
            string name = parameter.Name;

            if (type == typeof(string))
            {
                return parameter.Optional ? "(<" + name + ">)" : "<" + name + ">";
            }
            else if (type == typeof(Flag))
            {
                return $"--{parameter.Name}";
            }
            else if (type == typeof(Assignment))
            {
                return $"{name}=<value>";
            }
            else if (type == typeof(FlagValue))
            {
                return $"--{name} <value>";
            }
            else if (type == typeof(Arguments))
            {
                return $"({name}...)";
            }
            else return $"--- unknown: {name} ---"; // shouldn't get here.
            
        }



        private static bool TryBuildParameters(this Route route, Arguments arguments, out object[] values)
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

                else if (param.Type == typeof(Assignment))
                {
                    if (arguments.TryGet(param.Name, out Assignment assignment))
                    {
                        values[i++] = assignment;
                        used++;
                    }
                }

                else if (param.Type == typeof(FlagValue))
                {
                    if (arguments.TryGetFlagValue(param.Name, out string value))
                    {
                        values[i++] = new FlagValue(value, 2);
                        used += 2;
                    }
                    else
                    {
                        values[i++] = new FlagValue(null, 0, provided: false);
                    }
                }

                else if (param.Type == typeof(Flag))
                {
                    if (arguments.TryGet(param.Name, out Flag flag))
                    {
                        values[i++] = flag;
                        used++;
                    }
                    else
                    {
                        values[i++] = new Flag(param.Name, set: false);
                    }
                }

                else if (param.Type == typeof(Arguments))
                {
                    values[i++] = arguments;
                    return true;
                }
                else
                {
                    // this method has a signature with an unknown type.
                    return false;
                }
            }
            return (arguments.Count == used);
            
        }

        public static bool TryBind(this Route route, Arguments arguments, out Bind bind)
        {
            if (route.TryBuildParameters(arguments, out var values))
            {
                bind = new Bind(route, values);
                return true;
            }
            else
            {
                bind = null;
                return false;
            }
        }
    }

}