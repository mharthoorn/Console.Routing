using System;

namespace Harthoorn.Shell.Routing 
{

    public class Optional : Attribute { }

    
    public class Option
    {
        public bool Set;
        public Option(bool set)
        {
            this.Set = set;
        }
        
        public static implicit operator bool(Option option)
        {
            return option.Set;
        }

        public override string ToString()
        {
            return Set ? "Set" : "Not set";
        }
    }

    public class OptionValue
    {
        public bool Set;
        public bool Provided;
        public string Value;


        public OptionValue(string value, bool provided = true)
        {
            this.Provided = provided;
            this.Set = provided && !string.IsNullOrEmpty(value);
            this.Value = value;
        }
        

        public static OptionValue Unset()
        {
            return new OptionValue(null, false);
        }

        public static implicit operator bool(OptionValue option)
        {
            return option.Set;
        }

        public static implicit operator string(OptionValue option)
        {
            return option.Value;
        }

        public override string ToString()
        {
            return Value;
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
                if (Type == typeof(string))
                {
                    return Optional ? "(<" + Name + ">)" : "<" + Name + ">";
                }
                else if (Type == typeof(Option))
                {
                    return "-" + Name;
                }
                else if (Type == typeof(OptionValue))
                {
                    return "-" + Name + " <value>";
                }
                else if (Type == typeof(Arguments))
                {
                    return "(" + Name + "...)";
                }
                else return "---" + Name + "---"; // shouldn't get here.
                
            }
        }

        public override string ToString()
        {
            string optional = Optional ? "optional " : "";
            return $"{optional}<{Name}> ({Type.Name}) ";
        }
    }

}