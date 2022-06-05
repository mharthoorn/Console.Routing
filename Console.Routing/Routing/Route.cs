using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting;

[Flags]
public enum RouteFlag
{
    Regular     = 1,  // A normal command
    Default     = 2,  // Accepts no parameters
    Fallback    = 4,  // only activated when all other commands fail. 
    Hidden      = 8,  // not visible in help, but callable
    Capturing   = 16, // can hijack a an argument from a single argument
}

public class Route
{
    public Module Module;
    public RouteFlag Flags;
    public Capture Capture;
    public List<Node> Nodes;
    public Help Help; 
    public MethodInfo Method;

    public Route(Module module, IEnumerable<Node> nodes, MethodInfo method, Help help, Capture capture, RouteFlag flags)
    {
        this.Module = module;
        this.Nodes = nodes.ToList();
        this.Method = method;
        this.Help = help;
        this.Capture = capture;
        this.Flags = flags;
    }

    public string Description => Help?.Description;

    public bool Is(RouteFlag flag) => Flags.HasFlag(flag);
    

    public override string ToString()
    {
        string commands = string.Join(" ", Nodes); 
        var parameters = Method.ParametersAsText();

        string s = commands;
        if (parameters.Length > 0) s += " " + parameters;
        return s;
    }
}