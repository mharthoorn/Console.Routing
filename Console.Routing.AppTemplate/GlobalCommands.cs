using ConsoleRouting;
using System;

namespace ConsoleAppTemplate
{

    // experimental: 
    internal class GlobalCommands
    {
        [Command, Global]
        public bool Default(Arguments args)
        {
            return false;
        }

        [Command, Global]
        public void HandleConfig([Consume]bool verbose)
        {
            Config.Verbose = verbose;
        }
    }


    public class Consume : Attribute
    {

    }

    internal static class Config
    {
        public static bool Verbose;
    }

}
