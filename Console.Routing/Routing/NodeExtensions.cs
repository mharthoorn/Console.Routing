using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleRouting;

public static class NodeExtensions
{
    public static List<Node> Clone(this List<Node> nodes)
    {
        var result = new List<Node>();
        result.AddRange(nodes);
        return result;
    }

    public static List<Node> CloneAndAppend(this List<Node> nodes, Node tail)
    {
        if (tail is null) return nodes;
       
        var clone = nodes.Clone();
        clone.Add(tail);
        return clone;
    }

    public static Node TryCreateRoutingNode(this MethodInfo method)
    {
        var command = method.GetCustomAttribute<Command>();
        if (command is null) return null;

        var names = command.IsGeneric ? new string[] { method.Name } : command.Names;
        var node = new Node(names);
        return node;
    }

    public static Node TryCreateRoutingNode(this Type type)
    {
        var command = type.GetCustomAttribute<Command>();
        if (command is null) return null;

        var names = command.IsGeneric ? new string[] { type.Name } : command.Names;
        var node = new Node(names);
        return node;
    }

}