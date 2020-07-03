using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
{

    public class Arguments : List<IArgument>
    { 
        
        public Arguments(string[] args)
        {
            var arguments = ParseArguments(args);
            this.AddRange(arguments);
        }

        public IList<T> Match<T>(string name) where T: IArgument
        {
            var oftype = this.OfType<T>();
            var matches = oftype.Where(a => a.Match(name)).ToList();
            return matches;
        }

        public IList<T> Match<T>(Parameter parameter) where T : IArgument
        {
            var oftype = this.OfType<T>();
            var matches = oftype.Where(a => a.Match(parameter)).ToList();
            return matches;
        }

        public ArgResult<T> GetHead<T>(int i = 0) where T: IArgument
        {
            //var matches = items.OfType<T>().ToList();
            if (i < Count)
            {
                var item = this[i];
                if (item is T result)
                {
                    return result;
                }
            }

            return ArgResult<T>.Fail($"Index {i} Out of bounds");
        }
         
        public bool TryGet<T>(int i, out T result) where T: IArgument
        {
            if (i < Count && this[i] is T item)
            {
                result = item;
                return true;
            }
            result = default;
            return false;
        }

        public bool TryGetHead<T>(out T result) where T : IArgument
        {
            return TryGet(0, out result);
        }

        private static IEnumerable<IArgument> ParseArguments(string[] args)
        {
            foreach(var arg in args)
            {
                foreach(var argument in ParseArgument(arg))
                {
                    yield return argument;
                }
            }
        }

        private static IEnumerable<IArgument> ParseArgument(string arg)
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
            {
                yield return new Text(arg);
            }


        }

        public static Arguments Parse(string s)
        {
            var args = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new Arguments(args);
        }

        public static Arguments Create(params string[] args)
        {
            return new Arguments(args);
        }

        public override string ToString()
        {
            return string.Join(" ", this);
        }
    }

}