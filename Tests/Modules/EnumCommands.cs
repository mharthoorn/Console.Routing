namespace ConsoleRouting.Tests
{
    [Module]
    public class EnumCommands
    {

        [Command]
        public void Bump(Component component)
        {
            System.Console.WriteLine(component.ToString());
        }
    }
    
}
