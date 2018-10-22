using Shell.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.Routing.Tests
{
     
    [Module("Tool")]
    public class ToolModule
    {
        [Command(aliases: "-t")]
        public void Tool()
        {

        }

        [Command]
        public void Action(string name)
        { 
        }

        [Command] 
        public void Action(string name, string alias, Option foo, FlagValue bar)
        {
            
        }

        [Command]
        public void Save([Optional]string filename, Option all, Option json, Option xml, FlagValue pattern)
        {
            Console.WriteLine("Saving");
        }
    }

    
}
