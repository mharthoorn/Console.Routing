namespace ConsoleRouting.Tests
{
    [Module("Assignments")]
    public class AssignsModule
    {
        [Command]
        public void Single(Assignment a)
        {

        }

        [Command]
        public void Expression(Flag format, string query)
        {
            // raw allows parsing of any symbol.
            // even though the query might contain an equals sign, it should not be treated as an assignment
        }

        [Command]
        public void Mix(Assignment format, string query)
        {
            // raw allows parsing of any symbol.
            // even though the query might contain an equals sign, it should not be treated as an assignment
        }
    }
    
}
