namespace Shell.Routing
{
    public class Literal : IArgument
    {
        public string Value { get; }

        public Literal(string value)
        {
            this.Value = value;
        }

        public bool Match(string name)
        {
            return string.Compare(this.Value, name, ignoreCase: true) == 0;
        }

        public static implicit operator string (Literal literal)
        {
            return literal.Value;
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }


}