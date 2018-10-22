using System;

namespace Shell.Routing
{
    public class Flag : IArgument
    {
        public bool Short { get; private set; }
        public string Name { get; private set; }
        public bool Set { get; private set; }

        public Flag(string name)
        {
            this.Name = name;
            this.Set = true;
            this.Short = name.Length == 1;
        }

        public Flag(string name, bool set)
        {
            this.Name = name;
            this.Set = set;
            this.Short = name.Length == 1;
        }

        public bool Match(string name)
        {
            if (Short) // short flag
            {
                return name.StartsWith(this.Name, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return string.Compare(this.Name, name, ignoreCase: true) == 0;
            }
        }

        public static implicit operator bool (Flag flag)
        {
            return flag.Set;
        }

        public override string ToString()
        {
            if (Short)
            {
                return $"-{Name}";
            }
            else
            {
                return $"--{Name}";
            }
        }
    }


}