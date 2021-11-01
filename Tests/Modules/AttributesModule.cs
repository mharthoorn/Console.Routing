#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

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
        public void DefaultTryMe(string? name = null)

        {

        }

    }
}
