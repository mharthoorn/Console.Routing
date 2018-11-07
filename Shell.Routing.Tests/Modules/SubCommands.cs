namespace Shell.Routing.Tests
{
    [Module, Command("mainfirst")]
    public class SubCommands
    {
        // this class is here to test giving the user feedback about the options he has
        // when only providing the main command.
        [Command]
        public void SubFirst()
        {

        }
        [Command]
        public void SubSecond(string test)
        {

        }
    }
}
