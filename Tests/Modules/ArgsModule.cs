namespace ConsoleRouting.Tests
{
    [Module("Args commands")]
    public class ArgsModule
    {
        [Command]
        public void AnythingGoes(Arguments args)
        {

        }

        [Command]
        public void AfterTheRain(string rain, string drip, Arguments args)
        {

        }

    }
}
