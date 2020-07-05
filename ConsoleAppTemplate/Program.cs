using ConsoleRouting;
using System;

namespace ConsoleAppTemplate
{
    [Module]
    public class BasicCommands
    {
        [Command]
        public void Help()
        {
            Routing.PrintHelp();
        }

        [Command, Default, Hidden]
        public void Default([Alt("?")]Flag help)
        {
            // If you provide no parameters, you end up here.
            Console.WriteLine($"Your tool. Version 0.1. Copyright (c) you.");
            if (help) Help();
        }

        [Command]
        public void Greet(string name)
        {
            Console.WriteLine($"Hello {name}!");
        }
    }
    
    
    class Program
    {

        static void Main(string[] args)
        {
            Routing.Handle(args);
        }
    }
}
