using ConsoleRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRouting.Tests
{
    [Module]
    public class ArgsModule
    {
        [Command]
        public void AnythingGoes(Arguments args)
        {

        }

        [Command]
        public void AfterTheRain(string rain, string drip, Arguments args)
        {

        }

    }
}
