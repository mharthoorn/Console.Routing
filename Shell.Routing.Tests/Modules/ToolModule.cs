using Shell.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.Routing.Tests
{

    [Module]
    public class ToolModule
    {
        [Command]
        public void Tool()
        {

        }

        [Command]
        public void Action(string name)
        { 
        }

        [Command] 
        public void Action(string name, string alias, Flag foo, FlagValue bar)
        {
            
        }

        [Command]
        public void Save([Optional]string filename, Flag all, Flag json, Flag xml, FlagValue pattern)
        {
            Console.WriteLine("Saving");
        }
    }

    
}
