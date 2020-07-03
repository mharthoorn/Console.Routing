using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
{
    public static class ArgumentsExtensions
    { 
        
        public static bool TryGetText(this Arguments args, int offset, out string literal)
        {
            var result = args.GetHead<Text>(offset);
            literal = (result.Success) ? result.Arg.Value : null;
            return result.Success;
        }

        public static bool TryGetEnum(this Arguments args, int offset, Parameter param, out object value)
        {
            if (args.TryGetText(offset, out string literal))
            {
                try
                {
                    value = Enum.Parse(param.Type, literal, ignoreCase: true);
                    return true;
                }
                catch
                {
                    value = null;
                    return false;
                }


                //var array = Enum.GetNames(param.Type);
                //if (array.Contains(literal))
                //{/
                //    value = Enum.Parse(param.Type, literal);
                //    return true;
                //}
            }
            else
            {
                value = null;
                return false;
            }
        }


     

        public static bool TryGetAssignment(this Arguments args, string name, out Assignment assignment)
        { 
            var matches = args.OfType<Text>();
            
            foreach (var m in matches) 
            {
                if (m.TryGetAssignment(name, out assignment)) return true;
            }
            assignment = Assignment.NotProvided();
            return false;
        }

        public static bool TryGet<T>(this Arguments args, string name, out T item) where T : IArgument
        {
            var items = args.Match<T>(name);
            item = items.FirstOrDefault();
            return items.Count == 1;
        }

        public static bool TryGet<T>(this Arguments args, Parameter parameter, out T item) where T: IArgument
        {
            var items = args.Match<T>(parameter);
            item = items.FirstOrDefault();
            return items.Count == 1;
        }

        public static bool TryGet<T>(this Arguments args, int i, out T arg) where T: IArgument
        {
            if (i < args.Count)
            {
                var item = args[i];
                if (item is T)
                {
                    arg = (T)item;
                    return true;
                }
            }

            arg = default;
            return false;
        }

        public static bool TryGetFollowing<T>(this Arguments args, IArgument arg, out T item) where T: IArgument
        {
            int index = args.IndexOf(arg);
            if (index >= 0)
            {
                if (TryGet(args, index + 1, out item))
                {
                    return true;
                }
                    
            }

            item = default;
            return false;
        }

        public static bool TryGetFlagValue(this Arguments args, Parameter parameter, out string value)
        { 
            if (args.TryGet(parameter, out Flag flag))
            {
                if (args.TryGetFollowing(flag, out Text literal))
                {
                    value = literal.Value;
                    return true;
                }
            }

            value = null;
            return false;
        }

    }

}