using System;

namespace Shell.Routing.Tests
{

    [Module("TestGlobal settings")]
    public class TestGlobalsModule
    {
        [Command]
        public void Log(string word)
        {
            LogWord(word);
        }

        [Global]
        public void NonSpecifics(Flag debug)
        {
        
        }

        public void LogWord(string word)
        {
            string detail = Globalsettings.Debug ? "DEBUG" : "ERROR";
            Console.WriteLine($"{DateTime.Now} {detail} {word}");
        }

    }

    public static class Globalsettings
    {
        public static bool Debug;

    }

}
