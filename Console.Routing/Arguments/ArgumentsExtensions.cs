using System;
using System.Linq;

namespace ConsoleRouting
{

    public static class ArgumentsExtensions
    {
        public static Arguments RemoveCommands(this Arguments arguments, Route route)
        {
            var offset = route.Nodes.Count();
            return new Arguments(arguments.Skip(offset));
        }

        public static bool TryGetText(this Arguments args, int offset, out Text literal)
        {
            return args.TryGet<Text>(offset, out literal);
        }

        public static bool TryGetEnum(this Arguments args, int offset, Parameter param, out object value)
        {
            if (args.TryGetText(offset, out Text literal))
            {
                try
                {
                    value = Enum.Parse(param.Type, literal, ignoreCase: true);
                    return true;
                }
                catch
                {
                }
            }
            value = null;
            return false;
        }

        public static bool TryGetInt(this Arguments args, int i, out int value)
        {
            if (args.TryGet(i, out Text s))
            {
                return int.TryParse(s, out value);
            }
            value = default;
            return false;
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

        public static bool TryGet<T>(this Arguments args, int i, out T item) where T: IArgument
        {
            if (i < args.Count && args[i] is T arg)
            {
                item = arg;
                return true;
            }

            item = default;
            return false;
        }

        public static bool TryGetFollowing<T>(this Arguments args, IArgument arg, out T item) where T: IArgument
        {
            int index = args.IndexOf(arg);
            if (index >= 0)
            {
                if (TryGet(args, index + 1, out item)) return true;
            }

            item = default;
            return false;
        }

        public static bool TryGetOptionString(this Arguments args, Parameter parameter, out string value)
        { 
            if (args.TryGet(parameter, out Flag flag))
            {
                if (args.TryGetFollowing(flag, out Text text))
                {
                    value = text.Value;
                    return true;
                }
            }

            value = null;
            return false;
        }

    }

}