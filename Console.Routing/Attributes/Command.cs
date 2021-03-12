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


}