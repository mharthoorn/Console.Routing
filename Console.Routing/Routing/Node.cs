using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
{
    public class Node
    {
        public string[] Names;

        public Node(IEnumerable<string> names)
        {
            this.Names = names.ToArray();
        }

        public Node(string name)
        {
            this.Names = new string[] { name };
        }

        public bool Matches(string name)
        {
            for (int i = 0; i < Names.Length; i++)
            {
                if (string.Compare(Names[i], name, ignoreCase: true) == 0) return true; 
            }
            return false;
        }

        public override string ToString()
        {
            return string.Join(" | ", Names);
        }
    }
}