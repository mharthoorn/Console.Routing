using ConsoleRouting;
using System;
using System.Collections.Generic;

namespace ConsoleAppTemplate
{
    [Module]
    public class InjectionCommands
    {
        private readonly Cow cow;
        private readonly Dog dog;
        private readonly List<Route> routes;

        public InjectionCommands(Cow cow, Dog dog, List<Route> routes)
        { 
            this.cow = cow;
            this.dog = dog;
            this.routes = routes;
        }

        [Command]
        public void Hear()
        {
            Console.WriteLine($"Hear the dog {dog.Sound}, and the cow: {cow.Sound}");
        }

        [Command]
        public void Routes()
        {
            foreach(var route in routes)
            {
                Console.WriteLine($"{route}");
            }
        }
    }

    public class Cow
    {
        public string Sound => "Moo";
    }

    public class Dog
    {
        public string Sound => "Wraf";
    }

}
