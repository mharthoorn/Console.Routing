using ConsoleRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTemplate
{
    [Module]
    public class FallbackCommands
    {
        [Command, Fallback]
        public void LastResort(Arguments arguments)
        {

        }
    }
}
