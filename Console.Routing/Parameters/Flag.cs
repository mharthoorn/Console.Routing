﻿using System;

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
        public bool Set { get; private set; }

        public string Value { get => Set.ToString(); }

        public Flag(string value)
        {

            this.Name = value.TrimStart('-');
            this.Set = true;
            this.Short = value.Length == 1;
        }

        public Flag(string name, bool set)
        {
            this.Name = name;
            this.Set = set;
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
            return flag?.Set ?? false;
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