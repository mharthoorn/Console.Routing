namespace ConsoleRouting.Tests
{
    [Module]
    public class PocoCommands
    { 
        [Command]
        public void Curl(CurlSettings settings)
        { 
        }

    }

    [Model]
    public class CurlSettings
    {
        public Flag CrlF;
        public bool Append;
        public Flag<string> Url;
        
        [Alt("use-ascii")]
        public Flag UseAscii { get; set; }
        public Flag Basic { get; set; }
        public Flag<string> RemoteName { get; set; }

        public Flag UnusedFlag;
        public bool UnusedBool;
    }
}
