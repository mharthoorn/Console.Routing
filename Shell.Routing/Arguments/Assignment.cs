namespace Shell.Routing
{
    public class Assignment : IArgument
    {
        public string Name;
        public string Value { get; }
        public bool Provided { get; private set; }

        public Assignment(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public bool Match(string name)
        {
            return string.Compare(this.Name, name, ignoreCase: true) == 0;
        }

        public static implicit operator bool(Assignment assignment)
        {
            return assignment.Provided;
        }

        public static implicit operator string(Assignment assignment)
        {
            return assignment.Value;
        }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }


}