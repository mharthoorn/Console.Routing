using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRouting;


public static class ArgumentsExtensions
{
    public static Arguments WithoutCommands(this Arguments arguments, Route route) 
    {
        var offset = route.Nodes.Count();
        var args = arguments.Skip(offset);
        return new Arguments(args);
    }

    public static Arguments Clone(this Arguments arguments)
    {
        return new Arguments(arguments);
    }

    public static Arguments WithoutCapture(this Arguments arguments, Capture capture)
    {
        return new Arguments(arguments.Where(a => !capture.Match(a)));
    }
     
    public static bool TryGetText(this Arguments args, int index, out Text literal)
    {
        return args.TryGet(index, out literal);
    }


    public static IEnumerable<T> Match<T>(this IEnumerable<IArgument> args, string name) where T : IArgument
    {
        var oftype = args.OfType<T>();
        var matches = oftype.Where(a => a.Match(name));
        return matches;
    }

    public static bool TryGet<T>(this Arguments args, string name, out T item) where T : IArgument
    {
        var items = args.Match<T>(name).ToList();
        item = items.FirstOrDefault();
        return items.Count == 1;
    }


    public static IEnumerable<T> Match<T>(this IEnumerable<IArgument> args, Parameter parameter) where T : IArgument
    {
        var oftype = args.OfType<T>();
        var matches = oftype.Where(a => a.Match(parameter)).ToList();
        return matches;
    }

    public static bool TryGet<T>(this Arguments args, Parameter parameter, out T item) where T: IArgument
    {
        var items = args.Match<T>(parameter).ToList();
        item = items.FirstOrDefault();
        return items.Count == 1;
    }

    public static bool TryGet<T>(this Arguments args, int index, out T item) where T: IArgument
    {
        if (index < args.Count && args[index] is T arg)
        {
            item = arg;
            return true;
        }

        item = default;
        return false;
    }

    public static bool TryGetFollowing<T>(this Arguments args, IArgument arg, out T item) where T: IArgument
    {
        int index = args.IndexOf(arg);
        if (index >= 0)
        {
            if (TryGet(args, index + 1, out item)) return true;
        }

        item = default;
        return false;
    }

    public static bool TryGetOptionString(this Arguments args, Parameter parameter, out string value)
    { 
        if (args.TryGet(parameter, out Flag flag))
        {
            if (args.TryGetFollowing(flag, out Text text))
            {
                value = text.Value;
                return true;
            }
        }

        value = null;
        return false;
    }

}

public static class ArgumentUseExtensions
{
    public static bool TryUseInt(this Arguments args, int index, out int value)
    {
        if (args.TryGet(index, out Text text))
        {
            if (!args.IsUsed(text) && int.TryParse(text, out value))
            {
                args.Use(text);
                return true;
            }
        }
        value = default;
        return false;
    }

    public static bool TryUseAssignment(this Arguments args, string name, out Assignment assignment)
    {
        var matches = args.Unused().OfType<Text>();

        foreach (var m in matches)
        {
            if (m.TryGetAssignment(name, out assignment))
            {

                args.Use(m);
                return true;
            }
        }
        assignment = Assignment.NotProvided;
        return false;
    }
    
    public static bool TryUseEnum(this Arguments args, int index, Parameter param, out object value)
    {
        if (args.TryGetText(index, out Text text) && !args.IsUsed(text))
        {
            try
            {
                args.Use(text);
                value = Enum.Parse(param.Type, text, ignoreCase: true);
                return true;
            }
            catch
            {
            }
        }
        value = null;
        return false;
    }

   
    public static bool TryUse<T>(this Arguments args, Parameter param, out T item) where T : IArgument
    {
        var matches = args.Unused().Match<T>(param).ToList();
        if (matches.Count == 1)
        {
            item = matches.FirstOrDefault();
            args.Use(item);
            return true;
        }
        item = default;
        return false;
    }

    public static bool TryUseText(this Arguments args, int index, out Text text)
    {
        if (args.TryGet(index, out text) && !args.IsUsed(text))
        {
            args.Use(text);
            return true;
        }
        else
        {
            return false;
        }
    }
}