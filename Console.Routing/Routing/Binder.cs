using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public static class Binder 
    {
        
        public static void Bind(IEnumerable<Type> types, Arguments arguments)
        {
            if (types is null) return;

            foreach(var type in types)
            {
                Bind(type, arguments);
            }
        }

        public static void Bind(Type type, Arguments arguments)
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

        public static bool TryBind(Route route, Arguments arguments, out Bind bind)
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

        private static List<IBinder> DEFAULTBINDERS = new()
        {
            new StringBinder(),
            new EnumBinder(),
            new IntBinder(),
            new AssignmentBinder(),
            new FlagValueBinder(),
            new FlagBinder(),
            new BoolBinder(),
        };
        

        public static bool TryBindParameters_old(Route route, Arguments arguments, out object[] values)
        {
            var parameters = route.Method.GetRoutingParameters().ToArray();
            var offset = route.Nodes.Count(); // amount of parameters to skip, because they are commands.
            var argcount = arguments.Count - offset;
            var count = parameters.Length;

            values = new object[count];
            int pindex = 0; // index of parameters
            int used = 0; // arguments used;

            foreach (var param in parameters)
            {
                int argindex = offset + pindex; // index of arguments
                if (param.Type == typeof(string))
                {
                    if (arguments.TryGetText(argindex, out Text Text))
                    {
                        values[pindex++] = Text.Value;
                        used++;
                    }

                    else if (param.Optional)
                    {
                        values[pindex++] = null;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (param.Type.IsEnum)
                {
                    if (arguments.TryGetEnum(argindex, param, out object value))
                    {
                        values[pindex++] = value;
                        used++;
                    }
                    else
                    {
                        return false;
                    }

                }

                else if (param.Type == typeof(int))
                {
                    if (arguments.TryGetInt(argindex, out int value))
                    {
                        values[pindex++] = value;
                        used++;
                    }
                }

                else if (param.Type == typeof(Assignment))
                {
                    if (arguments.TryGetAssignment(param.Name, out Assignment assignment))
                    {
                        values[pindex++] = assignment;
                        used++;
                    }
                    else
                    {
                        values[pindex++] = Assignment.NotProvided();
                    }
                }
               
                else if (param.Type.IsGenericType && param.Type.GetGenericTypeDefinition() == typeof(Flag<>))
                {
                    Type innertype = param.Type.GetGenericArguments()[0];

                    if (arguments.TryGetOptionString(param, out string value))
                    {
                        if (innertype == typeof(string))
                        {
                            values[pindex++] = new Flag<string>(param.Name, value);
                            used += 2;
                        }
                        else if (innertype == typeof(int))
                        {
                            if (int.TryParse(value, out int n))
                            {
                                values[pindex++] = new Flag<int>(param.Name, n);
                                used += 2;
                            }
                        }
                        else if (innertype.IsEnum)
                        {
                            
                            if (StringHelpers.TryParseEnum(innertype, value, out object enumvalue))
                            {
                                var flagt = Flags.CreateInstance(innertype, param.Name, enumvalue);
                                values[pindex++] = flagt;
                                used += 2;
                            }
                            else
                            {
                                return false;
                            }
                        }

                    }
                    else
                    {
                        values[pindex++] = Flags.CreateNotSetInstance(innertype, param.Name);
                    }

                }

                else if (param.Type == typeof(Flag))
                {
                    if (arguments.TryGet(param, out Flag flag))
                    {
                        values[pindex++] = flag;
                        used++;
                    }
                    else
                    {
                        values[pindex++] = new Flag(param.Name, set: false);
                    }
                }

                else if (param.Type == typeof(bool))
                {
                    if (arguments.TryGet(param, out Flag flag))
                    {
                        values[pindex++] = true;
                        used++;
                    }
                    else
                    {
                        values[pindex++] = false;
                    }
                }

                else if (param.Type == typeof(Arguments))
                {
                    values[pindex++] = arguments;
                    return true;
                }
                else
                {
                    // this method has a signature with an unknown type.
                    return false;
                }
            }
            return (argcount == used);

        }

        /// <summary>
        /// Note that parameters here refer to the parameters of a C# method, and that arguments refer to the values
        /// that may go into those paremeters
        /// </summary>
        public static bool TryBindParameters(Route route, Arguments arguments, out object[] values)
        {
            Parameters parameters = route.Method.GetRoutingParameters();
            var offset = route.Nodes.Count(); // amount of parameters to skip, because they are commands.
            var argcount = arguments.Count - offset;
            var count = parameters.Count;

            values = new object[count];
            int pindex = 0; // index of parameters
            int used = 0; // arguments used;

            List<IBinder> binders = DEFAULTBINDERS;
            
            foreach (var param in parameters)
            {
                int argindex = offset + pindex; // index of arguments

                foreach(var binder in binders)
                {
                    if (binder.Match(param))
                    {
                        int uses = binder.TryUse(arguments, param, argindex, out var value);
                        if (uses > 0)
                        {
                            values[pindex++] = value;
                            used += uses;
                            break;
                        }
                        else if (param.Optional)
                        {
                            values[pindex++] = default;
                            break;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }


                //else if (param.Type == typeof(Arguments))
                //{
                //    values[pindex++] = arguments;
                //    return true;
                //}
                //else
                //{
                //    // this method has a signature with an unknown type.
                //    return false;
                //}
            }
            return (argcount == used);

        }

    }


}


