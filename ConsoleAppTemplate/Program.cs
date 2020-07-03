﻿using ConsoleRouting;
using System;

namespace ConsoleAppTemplate
{
    [Module("Program")]
    public class BasicCommands
    {
        [Command]
        public void Help()
        {
            Routing.PrintHelp();
        }

        [Command, Default, Hidden]
        public void Default()
        {
            Console.WriteLine($"Console App template -- If you provide no parameters, you end up here.");
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