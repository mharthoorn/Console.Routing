using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRouting.Tests
{
    [Module]
    public class AttributesModule
    {
     
        [Command]
        public void OptionalTryMe([Optional]string name)
        {

        }

        [Command]
        public void NullableTryMe(string? name)
        {

        }

        [Command]
        public void DefaultTryMe(string name = null)
        {

        }
    }
}
