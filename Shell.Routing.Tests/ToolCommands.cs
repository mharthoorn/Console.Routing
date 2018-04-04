using Harthoorn.Shell.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.Routing.Tests
{

    [Section("Tool")]
    public class ToolCommands
    {
        [Command]
        public void Tool()
        {

        }

        [Command]
        public int Single(string name)
        {
            return name.GetHashCode();
        }
    }
}
