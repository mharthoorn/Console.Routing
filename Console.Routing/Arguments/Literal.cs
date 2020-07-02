namespace ConsoleRouting
{
    public class Literal : IArgument
    {
        public string Value { get; private set; }

        public Literal(string value)
        {
            this.Value = value;
        }

        public bool Match(string name)
        {
            return string.Compare(this.Value, name, ignoreCase: true) == 0;
        }
    }
}