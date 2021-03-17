namespace ConsoleRouting.Tests
{
    public enum Component
    {
        Major,
        Minor,
        Patch,
        Pre
    }

    [Module("Tool")]
    public class ToolModule
    {
        [Command, Default]
        public void Info([Alt("?")] Flag help)
        {
            System.Console.WriteLine("Info");
        }

        [Command]
        public void Tool()
        {

        }

        [Command]
        public void Action(string name)
        { 
        }

        [Command] 
        public void Action(string name, string alias, Flag foo, Flag<string> bar)
        {
            
        }

        [Command]
        public void Save([Optional]string filename, Flag all, Flag json, Flag xml, Flag<string> pattern)
        {
            System.Console.WriteLine("Saving");
        }


    }
    
}
