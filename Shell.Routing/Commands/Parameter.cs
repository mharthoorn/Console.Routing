using System;

namespace Shell.Routing
{
    public class Parameter
    {
        public string Name;
        public Type Type;
        public bool Optional = false;

    public override string ToString()
        {
            string optional = Optional ? "(optional) " : "";
            return $"{optional}{Type.Name} {Name}";
        }
    }

    
}