using System;

namespace Shell.Routing
{
    public class Module: Attribute
    {
        public string Name { get; }

        public Module(string name) => this.Name = name;
    }


}