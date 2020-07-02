using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public static class Globals
    {
        public static void Bind(Assembly assembly, Arguments arguments)
        {
            var type = assembly.GetTypes().Where(t => t.HasAttribute<Global>()).FirstOrDefault();
            Bind(type, arguments);
        }

        private static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        public static void Bind(Type type, Arguments arguments)
        {
            if (!type.IsStatic()) throw new ArgumentException("A global settings class must be static");
            var globals = new List<IArgument>();

            foreach (var arg in arguments)
            {
                if (arg is Flag f)
                {
                    if (type.GetProperty(typeof(Flag), f.Name) is PropertyInfo pf)
                    {
                        pf.SetValue(null, f);
                        globals.Add(arg);
                    }
                    else if (type.GetProperty(typeof(bool), f.Name) is PropertyInfo pb)
                    {
                        pb.SetValue(null, true);
                        globals.Add(arg);
                    }
                }
                
            }
            foreach(var a in globals) arguments.Remove(a);
        }

    }
}


