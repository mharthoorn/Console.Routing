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
        public void TryMe([Optional]string name)
        {

        }
    }
}
