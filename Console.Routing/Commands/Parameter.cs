using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleRouting
{
    public class Parameter
    {
        public string Name;
        public Type Type;
        public string AltName;
        public bool Optional = false;

        public override string ToString()
        {
            string optional = Optional ? "(optional) " : "";
            return $"{optional}{Type.Name} {Name}";
        }

        public static Parameter Create<T>(string name, string alt = null, bool optional = false)
        {
            return new Parameter
            {
                Name = name,
                Type = typeof(T),
                AltName = alt,
                Optional = optional
            };
        }
    }

    [DebuggerDisplay("{Text}")]
    public class Parameters : List<Parameter>
    {
        public Parameters(IEnumerable<Parameter> parameters)
        {
            this.AddRange(parameters);
        }

        public  string Text => $"({string.Join(", ", this)})";

    }


}