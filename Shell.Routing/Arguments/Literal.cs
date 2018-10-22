namespace Shell.Routing
{
    public class Literal : IArgument
    {
        public string Value;

        public Literal(string value)
        {
            this.Value = value;
        }

        public bool Match(string name)
        {
            return string.Compare(this.Value, name, ignoreCase: true) == 0;
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }


}