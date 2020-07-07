using System;

namespace ConsoleRouting.Tests
{

    [Module("TestGlobal settings")]
    public class TestGlobalsModule
    {
        [Command]
        public void Log(string word)
        {
            LogWord(word);
        }

        public void LogWord(string word)
        {
            string detail = SomeSettings.Debug ? "DEBUG" : "ERROR";
            System.Console.WriteLine($"{DateTime.Now} {detail} {word}");
        }

        [Command]
        public void Train(Flag dog) // The rest are global settings.
        {

        }

    }

}
