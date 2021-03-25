using ConsoleRouting;
using System;

namespace ConsoleAppTemplate
{
    [Module]
    internal class GlobalCommands
    {
        [Command, Capture("--info", "-i", "info")]
        public void HandleConfig(Arguments arguments)
        {
            Console.WriteLine("This is information about the arguments");
            foreach(var arg in arguments)
            {
                Console.WriteLine($"- {arg}");
            }
        }
    }



}
