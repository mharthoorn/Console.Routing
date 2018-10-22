using System;

namespace Shell.Routing
{
    public class Parameter
    {
        public string Name;
        public Type Type;
        public bool Optional = false;

        public string AsString
        {
            get
            {
                if (Type == typeof(string))
                {
                    return Optional ? "(<" + Name + ">)" : "<" + Name + ">";
                }
                else if (Type == typeof(Flag))
                {
                    return $"--{Name}";
                }
                else if (Type == typeof(Assignment))
                {
                    return $"{Name}=<value>";
                }
                else if (Type == typeof(FlagValue))
                {
                    return $"--{Name} <value>";
                }
                else if (Type == typeof(Arguments))
                {
                    return $"({Name}...)";
                }
                else return $"--- unknown: {Name} ---"; // shouldn't get here.
            }
        }

        public override string ToString()
        {
            string optional = Optional ? "(optional) " : "";
            return $"{optional} {Type.Name} {Name}";
        }
    }

}