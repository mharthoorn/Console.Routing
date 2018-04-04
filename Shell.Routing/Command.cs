using System;

namespace Harthoorn.Shell.Routing
{

    public class Command : Attribute
    {
        public string Description { get; }

        public Command(string description = null)
        {
            this.Description = description;
            
        }
    }

    public class Section: Attribute
    {
        public string Name { get; }

        public Section(string name)
        { 
            this.Name = name;
        }
    }


}