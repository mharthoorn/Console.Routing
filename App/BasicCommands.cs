using ConsoleRouting;
using System;

namespace ConsoleAppTemplate
{
 
    [Module, Hidden]
    internal class BasicCommands
    {
        private Router router; 

        public BasicCommands(Router router)
        {
            this.router = router;
        }

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
                var result = router.Bind(args);
                router.Writer.WriteRouteHelp(result);
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
        /// <param name="uppercase">Transforms the name to all caps</param>
        /// <param name="repeats">How many times the greeting should be repeated</param>
        [Command, Help("Says hello to the given name")]
        public void Greeting([Optional]string name, bool uppercase, Flag<int> repeats)
        {
            if (uppercase) name = name.ToUpper();
            for(int i = 0; i < (repeats.HasValue ? repeats.Value : 1); i++)
            {
                Console.WriteLine($"Hello {name}!");
            }
        }


        /// <summary>
        /// This command shows how much fun it is to create commands. Although the show command does not do anything, 
        /// it helps understand that you can write extensive documentation on commands using ConsoleRouting.
        /// 
        /// And here you see how that works. This line was after a double new line. Do you think we correctly deal with spaces?
        /// </summary>
        [Command]
        public void Show()
        {
        }

        [Command]
        public void Throw()
        {
            throw new Exception();
        }

        [Command]
        public void Hello()
        {
            Console.WriteLine("Hello world");
        }
    }
}
