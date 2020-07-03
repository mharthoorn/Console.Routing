namespace ConsoleRouting
{
    public class Literal : Text
    {
        public Literal(string value) : base(value)
        {
        }

        public override bool Match(string name)
        {
            return string.Compare(this.Value, name, ignoreCase: true) == 0;
        }
    }
}