using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleRouting
{
    [DebuggerDisplay("{Text}")]
    public class Arguments : List<IArgument>
    { 
        
        public Arguments(string[] args)
        {
            var arguments = ParseArguments(args);
            this.AddRange(arguments);
        }

        public Arguments(IEnumerable<IArgument> arguments)
        {
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
         
        public bool TryGetCommand(int i, out string result) 
        {
            if (i < Count)
            {
                result = this[i].Original;
                return true;
            }
            else
            {
                
                result = null;
                return false;
            }
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

        public string Text => string.Join(" ", this);
    }

  
}