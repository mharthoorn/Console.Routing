using System;

namespace Shell.Routing
{

    public class Command : Attribute
    {
        public string[] Names { get; }
        public Command(params string[] names)
        {
            this.Names = names;
        }
    }

    public class Default : Attribute
    {

    }

    public class Hidden : Attribute
    {

    }

    public class Help : Attribute
    {
        public string Description { get; }

        public Help(string description = null)
        {
            this.Description = description;
            
        }

        
    }


}