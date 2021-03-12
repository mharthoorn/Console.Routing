using System;

namespace ConsoleRouting
{
    public class Parameter
    {
        public string Name;
        public Type Type;
        public string AltName;
        public bool Optional = false;

        public override string ToString()
        {
            string optional = Optional ? "(optional) " : "";
            return $"{optional}{Type.Name} {Name}";
        }

        public static Parameter Create<T>(string name, string alt = null, bool optional = false)
        {
            return new Parameter
            {
                Name = name,
                Type = typeof(T),
                AltName = alt,
                Optional = optional
            };
        }
    }


}