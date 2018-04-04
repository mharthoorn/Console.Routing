using System;

namespace Harthoorn.Shell.Routing 
{

    public class Optional : Attribute { }

    
    public class Option
    {
        public bool Set;

        public static implicit operator bool(Option option)
        {
            return option.Set;
        }
    }

    public class OptionValue
    {
        public bool Set;
        public string Value;

        public static implicit operator bool(OptionValue option)
        {
            return option.Set;
        }

        public static implicit operator string(OptionValue option)
        {
            return option.Value;
        }
    }

    public class RoutingParameter
    {
        public string Name;
        public Type Type;
        public bool Optional;

        public string AsString
        {
            get
            {
                return Optional ? "(<" + Name + ">)" : "<"+Name+">";
            }
        }
    }

}