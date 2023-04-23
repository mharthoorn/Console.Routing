using System;

namespace ConsoleRouting.Tests
{
    [Module("Enum commands")]
    public class EnumCommands
    {

        [Command]
        public void Bump(Component component)
        {
            Console.WriteLine(component.ToString());
        }
    }
    
}
