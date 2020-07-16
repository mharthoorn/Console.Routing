namespace ConsoleRouting.Tests
{
    public enum Format { Xml, Json, Markdown };

    [Module]
    public class FlagTestModule
    {
        [Command]
        public void Parse(Flag<string> format)
        {

        }

        [Command]
        public void IntParse(Flag<int> number)
        {

        }

        [Command]
        public void TypedParse(Flag<Format> format)
        {

        }
    };
}
