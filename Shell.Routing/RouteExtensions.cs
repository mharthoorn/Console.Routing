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

        public static bool TryBind(this MethodInfo method, Arguments arguments, out object[] values)
        {
            // group command value value -dev -all -special:souce -route C:\temp

            var parameters = method.GetParameters();
            values = new object[parameters.Length];
            int i = 0;

            foreach (var parameter in parameters)
            {
                var name = parameter.Name.ToLower();
                var type = parameter.ParameterType;
                bool optional = parameter.GetCustomAttribute<Optional>() != null;

                if (type == typeof(string))
                {
                    if (arguments.TryTakeHeadValue(out string value) || optional)
                    {
                        values[i++] = value;  
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (type == typeof(Option))
                {
                    if (arguments.HasOption(name))
                    {
                        values[i++] = new Option { Set = true };
                    }
                    else
                    {
                        values[i++] = new Option { Set = false };
                    }
                }
                else if (type == typeof(OptionValue))
                {
                    string value = arguments.GetOptionValue(name);

                    values[i++] = new OptionValue { Set = !string.IsNullOrEmpty(value), Value = value };
                }
                
            }
            return true;
        }

    }

}