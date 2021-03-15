using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public class Binder 
    {
        private List<IBinder> binders;

        public Binder(IEnumerable<IBinder> binders)
        {
            this.binders = binders.ToList();
        }


        public IEnumerable<Bind> Bind(IEnumerable<Route> routes, Arguments arguments)
        {
            foreach (var route in routes)
            {
                if (TryBind(route, arguments, out var bind))
                {
                    yield return bind;
                }
            }
        }

        public void Bind(IEnumerable<Type> types, Arguments arguments)
        {
            if (types is null) return;

            foreach(var type in types)
            {
                Bind(type, arguments);
            }
        }

        
        public void Bind(Type type, Arguments arguments)
        {
            if (type is null) return;

            var globals = new List<IArgument>();

            foreach (var arg in arguments)
            {
                if (arg is Flag f && type.GetProperty(typeof(bool), f.Name) is PropertyInfo pb)
                {
                    pb.SetValue(null, true);
                    globals.Add(arg);
                }
                else if (arg is Flag<string> s && type.GetProperty(typeof(string), s.Value) is PropertyInfo ps)
                {
                    ps.SetValue(null, s.Value);
                    globals.Add(arg);
                }
                
            }

            foreach (var a in globals) arguments.Remove(a);
        }

        public bool TryBind(Route route, Arguments arguments, out Bind bind)
        {
            if (TryBindParameters(route, arguments, out var values))
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
      
        public bool TryBindParameters(Route route, Arguments arguments, out object[] values)
        {
            Parameters parameters = route.Method.GetRoutingParameters();
            arguments = arguments.RemoveCommands(route);
            return TryBindParameters(parameters, arguments, out values);
        }

        // Note that parameters here refer to the parameters of a C# method, and that arguments refer to the values
        // from the command line that will set those paremeters.
        private bool TryBindParameters(Parameters parameters, Arguments arguments, out object[] values)
        {
            values = new object[parameters.Count];

            int index = 0; // index of parameters
            int used = 0; // arguments used;

            foreach (var param in parameters)
            {
                var binder = binders.FindMatch(param.Type);
                if (binder is null) return false;

                int uses = binder.TryUse(arguments, param, index, out var value);
                if (uses > 0)
                {
                    values[index++] = value;
                    used += uses;
                }
                else if (binder.Optional | param.Optional)
                {
                    values[index++] = value;
                }
                else
                {
                    return false;
                }

            }
            return (arguments.Count == used);
        }

    }


}


