using Harthoorn.Shell.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.Routing.Tests
{
    public class Utils
    {
        public static Arguments ParseArguments(string s)
        {
            var args = s.Split(' ');
            return new Arguments(args);
            
            
        }
    }
}
