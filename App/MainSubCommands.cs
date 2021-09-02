using System;

namespace ConsoleRouting.AppTemplate
{
    [Module("Sub")]
    public class MainSubCommands
    {

        [Command]     
        public void Greet()
        {

        }
        
        [Command]
        public void Greet(string hello)
        {

        }
        /// <summary>
        /// Documentation for main. Yada
        /// </summary>
        [Command, Help("Help for main")]
        public class Main
        {
            /// <summary>
            /// Documentation for sub. Yadayada
            /// </summary>
            [Command("Sub"), Help("Help for sub")]
            public void Sub()
            {
                Console.WriteLine("Main... sub...");
            }

            [Command, Help("Help for sub2")]
            public void Sub2()
            {
                Console.WriteLine("Main... sub2...");
            }
        }
    }
}
