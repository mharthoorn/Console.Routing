using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRouting.Tests
{
    [Module("native")]
    public class NativesModule
    {
        [Command] 
        public void ActionB(bool verbose)
        {
            if (verbose)
                System.Console.WriteLine("Hello world!");
        }

    }
}
