using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harthoorn.Shell.Routing
{
    public static class RouteExtensions
    {
        public static IEnumerable<Route> FindGroup(this IEnumerable<Route> routes, string group)
        {
            string g = group.ToLower();
            return routes.Where(r => string.Compare(r.Group.Name, g, ignoreCase: true) == 0);
        }

        public static IEnumerable<Route> FindMethod(this IEnumerable<Route> routes, string methodName)
        {
            var m = methodName.ToLower();
            if (Arguments.IsOption(m, out string abbrev))
            {

                return routes.Where(r => r.Method.Name.ToLower().StartsWith(abbrev));
            }
            else
            {
                return routes.Where(r => string.Compare(r.Method.Name, m, ignoreCase: true) == 0);
            }

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


        public static bool TryBind(this Route route, Arguments arguments, out object[] values)
        {
            // group command value value -dev -all -special:souce -route C:\temp
            var count  = route.Method.GetParameters().Count();
            values = new object[count];
            int i = 0;

            foreach (var param in route.GetRoutingParameters())
            {
                if (param.Type == typeof(string))
                {
                    if (arguments.TryTakeHeadValue(out string value) || param.Optional)
                    {
                        values[i++] = value;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (param.Type == typeof(Option))
                {
                    if (arguments.HasOption(param.Name))
                    {
                        values[i++] = new Option { Set = true };
                    }
                    else
                    {
                        values[i++] = new Option { Set = false };
                    }
                }
                else if (param.Type == typeof(OptionValue))
                {
                    string value = arguments.GetOptionValue(param.Name);

                    values[i++] = new OptionValue { Set = !string.IsNullOrEmpty(value), Value = value };
                }
                else if (param.Type == typeof(Arguments))
                {
                    values[i++] = arguments;
                }
                else
                {
                    // this method has a signature with wrong types.
                    return false;
                }
                
            }
            return true;
        }

    }

}