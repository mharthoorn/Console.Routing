using System;
using System.Linq;
using System.Reflection;

namespace Shell.Routing
{
    public class Route
    {
        public Module Section { get; }
        //public Command Command { get; }
        public Type Type { get; }
        public MethodInfo Method { get; }
        public string[] Names;
        public string Description;
        public bool Default { get; }
        public bool Hidden { get; }

        public Route(Module section, Command command, Type type, MethodInfo method)
        {
            Section = section;
            //Command = command;
            Type = type;
            Method = method;
            Default = method.HasAttribute<Default>();
            Hidden = method.HasAttribute<Hidden>();
            var help = method.GetCustomAttribute<Help>();
            Description = help?.Description;

            if (command.Names.Any()) { Names = command.Names; } else { Names = new string[] { Method.Name }; }
        }

        public string Name => Names.FirstOrDefault();

        public override string ToString()
        {
            var pars = this.ParametersDescription();
            return $"{Section.Name.ToLower()} {Name} {pars}";
        }

        public bool MatchName(string name)
        {
            return Names.Any(n => string.Compare(n, name, ignoreCase: true) == 0);
        }
    }

}