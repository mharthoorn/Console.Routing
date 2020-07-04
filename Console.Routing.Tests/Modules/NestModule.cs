namespace ConsoleRouting.Tests
{
    [Module("Nest"), Command("main")]
    public class NestModule
    {
        [Command] 
        public void Action(string message)
        {
            System.Console.WriteLine(message);
        }

        [Module("Sub"), Command("sub")]
        public class SubModule
        {
            [Command]
            public void Detail(string message)
            {
                System.Console.WriteLine(message);
            }
   
        }
    }
}
