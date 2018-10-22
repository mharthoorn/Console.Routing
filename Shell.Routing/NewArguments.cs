using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{
    /* 

        -- the order of flags should not matter
        -- flags are: --name or -n
        -- flags are disambiguated by testing.
        -- option values are: name=value
        -- format=xml

        fhir save --all
        fhir save -a

        fhir install --here hl7.fhir.core.stu3 

        fhir 



    

    */

    public interface IArgument
    {
        bool Match(string name);
    }

    public class Assignment : IArgument
    {
        public string Name;
        public string Value;
        public bool Provided { get; private set; }

        public Assignment(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public bool Match(string name)
        {
            return string.Compare(this.Name, name, ignoreCase: true) == 0;
        }

        public static implicit operator bool (Assignment assignment)
        {
            return assignment.Provided;
        }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }

    public class Flag : IArgument
    {
        public bool Short { get; private set; }
        public string Name { get; private set; }
        public bool Set { get; private set; }

        public Flag(string name)
        {
            this.Name = name;
            this.Set = true;
            this.Short = name.Length == 1;
        }

        public Flag(string name, bool set)
        {
            this.Name = name;
            this.Set = set;
            this.Short = name.Length == 1;
        }

        public bool Match(string name)
        {
            if (Short) // short flag
            {
                return name.StartsWith(this.Name, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return string.Compare(this.Name, name, ignoreCase: true) == 0;
            }
        }

        public static implicit operator bool (Flag flag)
        {
            return flag.Set;
        }

        public override string ToString()
        {
            if (Short)
            {
                return $"-{Name}";
            }
            else
            {
                return $"--{Name}";
            }
        }
    }

    public class Literal : IArgument
    {
        public string Value;

        public Literal(string value)
        {
            this.Value = value;
        }

        public bool Match(string name)
        {
            return string.Compare(this.Value, name, ignoreCase: true) == 0;
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }

    public class ArgResult<T> where T: IArgument
    {
        public T Arg;
        public bool Success;
        public IList<T> Matches;
        public int Count => Matches?.Count ?? 0;
        public string Message;

        public static implicit operator ArgResult<T> (T arg)
        {
            return new ArgResult<T> { Arg = arg, Success = true };
        }

        public static implicit operator T (ArgResult<T> result)
        {
            return result.Arg;
        }

        public static ArgResult<T> Fail(IList<T> matches)
        {
            return new ArgResult<T>
            {
                Success = false,
                Arg = default,
                Matches = matches
            };
        }

        public static ArgResult<T> Fail(string message)
        {
            return new ArgResult<T>
            {
                Success = false,
                Arg = default,
                Message = message
            };
        }
    }

    
    public class Arguments 
    { 
        internal List<IArgument> items = new List<IArgument>();
        
        public int Count => items.Count;

        public Arguments(string[] args)
        {
            var arguments = Parse(args);
            items.AddRange(arguments);
        }

        public IEnumerable<IArgument> Items => items;

        public IList<T> Match<T>(string name) where T: IArgument
        {
            var oftype = items.OfType<T>();
            var matches = oftype.Where(a => a.Match(name)).ToList();
            return matches;
        }

        public ArgResult<T> TryGetHead<T>(int offset = 0) where T: IArgument
        {
            var matches = items.OfType<T>().ToList();
            if (matches.Count >= offset)
            {
                var match = matches[offset];
                return match;
            }
            else return ArgResult<T>.Fail($"Index {offset} Out of bounds");
        }


        public static IEnumerable<IArgument> Parse(string[] args)
        {
            foreach(var arg in args)
            {
                foreach(var argument in Parse(arg))
                {
                    yield return argument;
                }
            }
        }

        public static IEnumerable<IArgument> Parse(string arg)
        {
            if (arg.StartsWith("--"))
            {
                yield return new Flag(arg.Substring(2));
            }
            else 
            if (arg.StartsWith("-"))
            {
                foreach(var c in arg.Substring(1))
                {
                    yield return new Flag(c.ToString());
                }
            }
            else 
            if (arg.Contains("="))
            {
                var parts = arg.Split('=');
                yield return new Assignment(parts[0], parts[1]);
            }
            else
            {
                yield return new Literal(arg);
            }


        }

        public override string ToString()
        {
            var shorts = items.OfType<Flag>().Where(a => a.Short);
            var rest = items.Except(shorts);
            return string.Join(" ", rest) + "-"+string.Join("", shorts.Select(a => a.Name)); ;
        }
    }


    public static class ArgExtensions
    {
        public static bool TryGetLiteral(this Arguments args, int offset, out string literal)  
        {
            var result = args.TryGetHead<Literal>(offset);
            literal = (result.Success) ? result.Arg.Value : null;
            return result.Success;
        }

        public static bool TryGetLiteral(this Arguments args, out string literal)
        {
            return TryGetLiteral(args, 0, out literal);
        }

        public static bool HasFlag(this Arguments args, string name)
        {
            var result = args.Match<Flag>(name);
            return result.Count == 1;
        }

        public static bool TryGetAssignment(this Arguments args, string name, out Assignment assignment)
        {
            var matches = args.Match<Assignment>(name);
            assignment = matches.FirstOrDefault();
            return matches.Count == 1;
        }

        public static bool TryGet<T>(this Arguments args, string name, out T item) where T: IArgument
        {
            var items = args.Match<T>(name);
            item = items.FirstOrDefault();
            return items.Count == 1;
        }

        public static IEnumerable<T> OfType<T>(this Arguments args)
        {
            return args.items.OfType<T>();
        }
    }


    public class Commands
    {
        Arguments arguments;
        List<Literal> commands;
        List<Literal> used = new List<Literal>();

        public Commands(Arguments args)
        {
            this.arguments = args;
            commands = BuildCommands(args.items);
        }

        private static List<Literal> BuildCommands(IEnumerable<IArgument> args)
        {
            var commands = new List<Literal>();
            foreach (var arg in args)
            {
                if (arg is Literal)
                {
                    commands.Add((Literal)arg);
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

        private void Use(Literal arg)
        {
            used.Add(arg);
            commands.Remove(arg);
        }

        public void Consume()
        {
            foreach (var item in used)
                arguments.items.Remove(item);
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