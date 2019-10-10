using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Routing.Tests
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

    }
}
