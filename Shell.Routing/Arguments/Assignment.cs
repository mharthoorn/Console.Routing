namespace Shell.Routing
{
    public class Assignment : IArgument
    {
        public string Name;
        public string Value;
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

        public static implicit operator bool (Assignment assignment)
        {
            return assignment.Provided;
        }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }


}