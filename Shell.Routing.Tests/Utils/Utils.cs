using Shell.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shell.Routing.Tests
{
    public class Utils
    {
        public static Arguments ParseArguments(string s)
        {
            var args = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return new Arguments(args);
        }

        public static Arguments CreateArguments(params string[] args)
        {
            return new Arguments(args);
        }
    }
}
