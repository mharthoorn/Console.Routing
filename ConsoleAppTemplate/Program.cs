using ConsoleRouting;
using System;

namespace ConsoleAppTemplate
{
    [Module]
    public class BasicCommands
    {
        [Command]
        public void Documentation(Arguments args = null)
        {
            if (args is null | args.Count == 0)
            {
                Routing.WriteRoutes();
            }
            else
            {
                args.RemoveAt(0);
                RoutingWriter.WriteRouteDocumentation(Routing.Router, args);
            }
        }

        [Command, Default, Hidden]
        public void Default()
        {
            // If you provide no parameters, you end up here.
            Console.WriteLine($"Your tool. Version 0.1. Copyright (c) you.");
            
        }

        [Command, Help("Says hello to the given name")]
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
