using System;

namespace Shell.Routing
{
    public class Module: Attribute
    {
        public string Title { get; }

        public Module(string title)
        {
            this.Title = title;
        }
        public Module()
        {

        }
    }


}