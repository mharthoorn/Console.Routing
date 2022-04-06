using System;

namespace ConsoleRouting.Tests
{
    [Module("native")]
    public class NativesModule
    {
        [Command] 
        public void ActionB(bool verbose)
        {
            if (verbose)
                Console.WriteLine("Hello world!");
        }

        [Command]
        public void Add(int a, int b)
        {

        }

    }
}
