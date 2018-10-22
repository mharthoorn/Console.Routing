using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{

    public class Arguments 
    { 
        internal List<IArgument> items = new List<IArgument>();
        
        public int Count => items.Count;

        public Arguments(string[] args)
        {
            var arguments = Parse(args);
            items.AddRange(arguments);
        }

        public IList<IArgument> Items => items;

        public IList<T> Match<T>(string name) where T: IArgument
        {
            var oftype = items.OfType<T>();
            var matches = oftype.Where(a => a.Match(name)).ToList();
            return matches;
        }

        public ArgResult<T> TryGetHead<T>(int offset = 0) where T: IArgument
        {
            //var matches = items.OfType<T>().ToList();
            if (offset < items.Count)
            {
                var item = items[offset];
                if (item is T result)
                {
                    return result;
                }
            }

            return ArgResult<T>.Fail($"Index {offset} Out of bounds");
        }


        public static IEnumerable<IArgument> Parse(string[] args)
        {
            foreach(var arg in args)
            {
                foreach(var argument in Parse(arg))
                {
                    yield return argument;
                }
            }
        }

        public static IEnumerable<IArgument> Parse(string arg)
        {
            if (arg.StartsWith("--"))
            {
                yield return new Flag(arg.Substring(2));
            }
            else 
            if (arg.StartsWith("-"))
            {
                foreach(var c in arg.Substring(1))
                {
                    yield return new Flag(c.ToString());
                }
            }
            else 
            if (arg.Contains("="))
            {
                var parts = arg.Split('=');
                yield return new Assignment(parts[0], parts[1]);
            }
            else
            {
                yield return new Literal(arg);
            }


        }

        public override string ToString()
        {
            return string.Join(" ", items);
        }
    }

}