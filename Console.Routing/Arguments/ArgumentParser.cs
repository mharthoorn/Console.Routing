using System.Collections.Generic;

namespace ConsoleRouting;


public class ArgumentParser
{
    public Arguments Parse(string[] args)
    {
        var arguments = ParseArguments(args);
        return new Arguments(arguments);
    }

    private static IEnumerable<IArgument> ParseArguments(string[] args)
    {
        foreach (var arg in args)
        {
            foreach (var argument in ParseArgument(arg))
            {
                yield return argument;
            }
        }
    }

    private static IEnumerable<IArgument> ParseArgument(string arg)
    {
        if (arg.StartsWith("--"))
        {
            yield return new Flag(arg.Substring(2));
        }
        else
        if (arg.StartsWith("-"))
        {
            foreach (var c in arg.Substring(1))
            {
                yield return new Flag(c.ToString());
            }
        }
        else
        {
            yield return new Text(arg);
        }

    }

}