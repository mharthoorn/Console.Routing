using System;

namespace Shell.Routing
{

    public class Command : Attribute
    {
        public string Description { get; }
        public string[] Aliases { get; }
        public Command(string description = null, params string[] aliases)
        {
            this.Description = description;
            this.Aliases = aliases;
        }
    }

    public class Default : Attribute
    {

    }

    public class Hidden : Attribute
    {

    }


}