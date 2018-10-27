using System;

namespace Shell.Routing.Tests
{
    [Module, Command("main")]
    public class NestModule
    {
        [Module, Command("sub")]
        public class SubModule
        {
            [Command]
            public void Detail(string message)
            {
                Console.WriteLine(message);
            }
            
        }
    }

    
}
