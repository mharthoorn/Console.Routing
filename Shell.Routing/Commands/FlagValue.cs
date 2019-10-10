namespace Shell.Routing
{


    public class FlagValue  
    {
        public string Name;
        public bool Set;
        public bool Provided;
        public string Value;

        public FlagValue(string value, bool provided = true)
        {
            this.Provided = provided;
            this.Set = provided && !string.IsNullOrEmpty(value);
            this.Value = value;
        }

        public static FlagValue Unset => new FlagValue(null, false);

        public static implicit operator bool(FlagValue option)
        {
            return option.Set;
        }

        public static implicit operator string(FlagValue option)
        {
            return option.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }

}