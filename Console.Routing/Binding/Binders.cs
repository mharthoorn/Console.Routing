using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRouting
{
    public interface IBinder
    {
        bool Optional { get; }
        bool Match(Type type);
        int TryUse(Arguments arguments, Parameter param, int index, out object value);
    }

    public class StringBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type)
        {
            return type == typeof(string);
        }

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            int used = 0;

            if (arguments.TryGetText(index, out Text Text))
            {
                value = Text.Value;
                used++;
            }
            else value = null;

            return used;
        }
    }

    public class EnumBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type) => type.IsEnum;

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            if (arguments.TryGetEnum(index, param, out value))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class IntBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type) => type == typeof(int);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            if (arguments.TryGetInt(index, out int i))
            {
                value = i;
                return 1;
            }
            else
            {
                value = default;
                return 0;
            }
        }
    }

    public class AssignmentBinder : IBinder
    {
        public bool Optional => false;

        public bool Match(Type type) => type == typeof(Assignment);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            
            if (arguments.TryGetAssignment(param.Name, out Assignment assignment))
            {
                value = assignment;
                return 1;
            }
            else
            {
                value = Assignment.NotProvided();
                return 1;
            }
            
        }
    }

    public class FlagValueBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Flag<>);
    
        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            Type innertype = param.Type.GetGenericArguments()[0];

            if (arguments.TryGetOptionString(param, out string s))
            {
                if (innertype == typeof(string))
                {
                    value = new Flag<string>(param.Name, s);
                    return 2;
                }
                else if (innertype == typeof(int))
                {
                    if (int.TryParse(s, out int n))
                    {
                        value = new Flag<int>(param.Name, n);
                        return 2;
                    }
                }
                else if (innertype.IsEnum)
                {

                    if (StringHelpers.TryParseEnum(innertype, s, out object enumvalue))
                    {
                        var flagt = FlagActivator.CreateInstance(innertype, param.Name, enumvalue);
                        value = flagt;
                        return 2;
                    }
                    else
                    {
                        value = default;
                        return 0;
                    }
                }

            }
            
            value = FlagActivator.CreateNotSetInstance(innertype, param.Name);
            return 0;

        }
    }

    public class FlagBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Flag);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
          
            {
                if (arguments.TryGet(param, out Flag flag))
                {
                    value = flag;
                    return 1;
                }
                else
                {
                    value = new Flag(param.Name, set: false);
                    return 0;
                }
            }

        }

    }

    public class BoolBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(bool);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            if (arguments.TryGet(param, out Flag flag))
            {
                value = true;
                return 1;
            }
            else
            {
                value = false;
                return 0;
            }
        }
    }

    public class ArgumentsBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type == typeof(Arguments);

        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            value = new Arguments(arguments);
            int claim = arguments.Count - index; // just claim the rest.
            return claim;
        }
    }

}
