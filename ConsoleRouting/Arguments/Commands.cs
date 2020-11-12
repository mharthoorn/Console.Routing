﻿using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting
{
    public class Commands
    {
        Arguments arguments;
        List<Text> commands;
        List<Text> used = new List<Text>();

        public Commands(Arguments args)
        {
            this.arguments = args;
            commands = BuildCommands(args);
        }

        public bool Empty => commands.Count == 0;


        private static List<Text> BuildCommands(IEnumerable<IArgument> args)
        {
            var commands = new List<Text>();
            foreach (var arg in args)
            {
                if (arg is Text)
                {
                    commands.Add((Text)arg);
                }
                else break;
            }
            return commands;
        }

        public bool TryGetHead(out string name)
        {
            if (commands.Count > 0)
            {
                name = commands.First().Value;
                return true;
            }
            else
            {
                name = null;
                return false;
            }
        }

        public void UseHead()
        {
            var cmd = commands[0];
            used.Add(cmd);
            commands.RemoveAt(0);
        }

        private void Use(Text arg)
        {
            used.Add(arg);
            commands.Remove(arg);
        }

        public void Consume()
        {
            foreach (var item in used)
                arguments.Remove(item);
        }

        //public ArgResult<T> TryUseHead<T>() where T: IArgument
        //{
        //    var matches = unused.OfType<T>().ToList();
        //    if (matches.Count > 0)
        //    {
        //        var match = matches.First();
        //        Consume(match);
        //        return match;
        //    }
        //    else return ArgResult<T>.Fail(matches);
        //}

        //public ArgResult<T> TryUse<T>(string name) where T : IArgument
        //{
        //    var matches = Match<T>(name);
        //    if (matches.Count == 1)
        //    {
        //        var arg = matches.First();
        //        Consume(arg);
        //        return arg;
        //    }
        //    else
        //    {
        //        return ArgResult<T>.Fail(matches);
        //    }
        //}
    }


}