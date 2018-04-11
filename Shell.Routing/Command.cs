using System;

namespace Shell.Routing
{

    public class Command : Attribute
    {
        public string Description { get; }

        public Command(string description = null) => this.Description = description;
    }

    public class Module: Attribute
    {
        public string Name { get; }

        public Module(string name) => this.Name = name;
    }


}