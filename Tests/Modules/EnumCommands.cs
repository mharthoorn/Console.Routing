using System;

namespace ConsoleRouting.Tests
{
    [Module]
    public class EnumCommands
    {

        [Command]
        public void Bump(Component component)
        {
            Console.WriteLine(component.ToString());
        }
    }
    
}
