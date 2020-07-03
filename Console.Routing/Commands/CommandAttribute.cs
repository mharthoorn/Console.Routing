using System;

namespace ConsoleRouting
{
    /// <summary>
    /// As a default a command gets the name from the method. But you can specify alternate names.
    /// names with symbols are allowed. Example "get-url".
    /// <br /> A command only becomes visible when its method is part of a class that is marked with the Module attribute
    /// <br /> <br/>
    /// Add the <seealso cref="Default"/> attribute if the command should be used when no commands are specified. 
    /// Use <seealso cref="Help"/> to specify additional information to the user.
    /// </summary>
    /// <param name="names"></param>/
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


    /// <summary>
    /// Default attribute makes a command the default route if no commands are specified in the argument list.
    /// </summary>
    public class Default : Attribute
    {

    }

    /// <summary>
    /// Hides the command from the help list
    /// </summary>
    public class Hidden : Attribute
    {

    }

    /// <summary>
    /// Sepcifies the help in the help list.
    /// </summary>
    public class Help : Attribute
    {
        public string Description { get; }

        public Help(string description = null)
        {
            this.Description = description;
            
        }

        
    }

    /// <summary>
    /// Defines an alternative name for a parameter
    /// </summary>
    public class Alt : Attribute
    {
        public string Name { get; }

        public Alt(string name)
        {
            this.Name = name;
        }
    }


    // Work in progress. The route binding should strip the arguments the method [Global] before trying to find routing candidates.
    public class Global : Attribute
    {

    }


}