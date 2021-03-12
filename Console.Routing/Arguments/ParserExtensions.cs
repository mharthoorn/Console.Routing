using System;

namespace ConsoleRouting
{
    public static class ParserExtensions
    {
        public static Arguments Parse(this ArgumentParser parser, string s)
        {
            var args = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return parser.Parse(args);
        }
    }

}