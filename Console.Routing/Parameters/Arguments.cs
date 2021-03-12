using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleRouting
{
    [DebuggerDisplay("{Text}")]
    public class Arguments : List<IArgument>
    { 
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
         
        public bool TryGetCommand(int index, out string result) 
        {
            if (index < Count)
            {
                result = this[index].Original;
                return true;
            }
            else
            {
                
                result = null;
                return false;
            }
        }

        public bool TryGet<T>(int index, out T result) where T: IArgument
        {
            if (index < Count && this[index] is T item)
            {
                result = item;
                return true;
            }
            result = default;
            return false;
        }

        public string Text => string.Join(" ", this);
    }

}