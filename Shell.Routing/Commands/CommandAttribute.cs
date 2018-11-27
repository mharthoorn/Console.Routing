using System;

namespace Shell.Routing
{

    public class Command : Attribute
    {
        public string[] Names { get; }
        public bool IsGeneric;

        public Command(params string[] names)
        {
            IsGeneric = (names.Length == 0);
            this.Names = names;
        }

        public override string ToString()
        {
            if (Names.Length > 0)
            {
                return "Command: " + string.Join(" | ", Names);
            }
            else
            {
                return "Generic Command";
            }
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

    public class Alt : Attribute
    {
        public string Name { get; }

        public Alt(string name)
        {
            this.Name = name;
        }
    }


}