using ConsoleRouting;
using System;
using System.Linq;

namespace ConsoleAppTemplate
{
    /// <summary>
    /// 
    /// </summary>
    [Module, Hidden]
    public class BasicCommands
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        [Command, Hidden]
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

        /// <summary>Says hello to the name in question</summary>
        /// <param name="name"> The name that will be greeted. You can use any name in the known universe </param>
        [Command, Hidden, Help("Says hello to the given name")]
        public void Greet(string name)
        {
            Console.WriteLine($"Hello {name}!");
        }

        [Command]
        public void Show()
        {
        }
    }
    
    
    class Program
    {
        static void Main(string[] args)
        {
            Routing.Handle(args);
            //Routing.Interactive();
        }
    }
}
