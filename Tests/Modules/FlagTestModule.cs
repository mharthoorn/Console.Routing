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

        [Command]
        public void FlagRun(string command, Flag<string> speed)
        {

        }

        [Command]
        public void FlagWithArgs(string command, Flag<string> speed, Arguments arguments)
        {

        }
    };
}
