namespace Shell.Routing
{


    public class FlagValue  
    {
        public string Name;
        public bool Set;
        public bool Provided;
        public string Value;
        public int Count;

        public FlagValue(string value, int count, bool provided = true)
        {
            this.Provided = provided;
            this.Set = provided && !string.IsNullOrEmpty(value);
            this.Value = value;
            this.Count = count;
        }

        public static FlagValue Unset => new FlagValue(null, 0, false);

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