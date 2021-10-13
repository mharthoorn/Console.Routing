using ConsoleRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRouting.Tests
{
    [Module]
    public class MixedFlags
    {
        [Command, Help("Searches for resources on the given server. ")]
        public void Search(string source, string type, Flag<string> pages, Flag split, Arguments arguments)
        {
            // for some reason, the combination of providing both flags, does not resolve the route.

        }
    }
}
