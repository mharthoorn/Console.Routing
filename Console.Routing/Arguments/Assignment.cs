namespace ConsoleRouting
{
    public class Assignment 
    {
        public string Name{ get; private set; }
        public string Value { get; private set; }
        public bool Provided { get; private set; }

        public Assignment(string name, string value)
        {
            this.Name = name;
            this.Value = value;

            Provided = true;
        }
        private Assignment() { } // to allow assignment construction

        public static Assignment NotProvided()
        {
            return new Assignment { Name = null, Value = null, Provided = false };
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
            if (!Provided) return "(Not provided)";
            return Value;
        }

        public bool Match(string name)
        {
            return string.Compare(this.Name, name, ignoreCase: true) == 0;
        }
    }


}