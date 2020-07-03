using System;
using System.Runtime.CompilerServices;

namespace ConsoleRouting
{
    public class Module: Attribute
    {
        public string Title { get; }

        
        public Module(string title = null)
        {
            this.Title = title;
            
        }
    }


}