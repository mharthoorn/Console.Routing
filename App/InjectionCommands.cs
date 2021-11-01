using ConsoleRouting;
using System;
using System.Collections.Generic;

namespace ConsoleAppTemplate
{

    /// <summary>
    /// Injection commands 
    /// </summary>
    [Module]
    public class InjectionCommands
    {
        private readonly Cow cow;
        private readonly Dog dog;
        private readonly List<Route> routes;

        /// <summary>
        /// Constructor with injectable f
        /// </summary>
        /// <param name="cow"></param>
        /// <param name="dog"></param>
        /// <param name="routes"></param>
        internal InjectionCommands(Cow cow, Dog dog, List<Route> routes)
        { 
            this.cow = cow;
            this.dog = dog;
            this.routes = routes;
        }

        /// <summary>
        /// Hear command
        /// </summary>
        [Command]
        public void Hear()
        {
            Console.WriteLine($"Hear the dog {dog.Sound}, and the cow: {cow.Sound}");
        }

        /// <summary>
        /// Routes command
        /// </summary>
        [Command]
        public void Routes()
        {
            foreach(var route in routes)
            {
                Console.WriteLine($"{route}");
            }
        }
    }


    internal class Cow
    {
        public string Sound => "Moo";
    }

    internal class Dog
    {
        public string Sound => "Wraf";
    }

}
