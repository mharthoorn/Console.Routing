using ConsoleRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRouting.Tests
{
    public enum Format { Xml, Json, Markdown };

    [Module]
    public class FlagTestModule
    {
        [Command]
        public void Parse(Flag<string> format)
        {

        }

        [Command]
        public void TypedParse(Flag<Format> format)
        {

        }
    };
}
