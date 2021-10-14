using System;

namespace ConsoleRouting
{
    public class Flag : IArgument
    {
        public string Original { 
            get 
            {
                string dash = Short ? "-" : "--";
                return dash + Name;
            } 
        }
        public bool Short { get; private set; }
        public string Name { get; private set; }

        public bool IsSet { get; private set; }

        [Obsolete("Replaced by IsSet")]
        public bool Set => IsSet;

        public string Value { get => IsSet.ToString(); }

        public Flag(string value)
        {

            this.Name = value.TrimStart('-');
            this.IsSet = true;
            this.Short = value.Length == 1;
        }

        public Flag(string name, bool set)
        {
            this.Name = name;
            this.IsSet = set;
            this.Short = name.Length == 1;
        }

        public bool Match(string name)
        {
            if (name is null) return false;

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
            return flag?.IsSet ?? false;
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